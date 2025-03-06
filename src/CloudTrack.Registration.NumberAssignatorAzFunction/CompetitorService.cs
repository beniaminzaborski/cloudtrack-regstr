using CloudTrack.Registration.Application.CompetitorRegistration;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Threading.Tasks;

namespace CloudTrack.Registration.NumberAssignatorAzFunction;

internal class CompetitorService(ILogger<CompetitorService> logger) : ICompetitorService
{
    private readonly ILogger<CompetitorService> _logger = logger;

    public async Task<(Guid id, string number)> RegisterCompetitorAndReturnNumber(RegisterCompetitor competitor)
    {
        await using var connection = CreateConnection();
        await connection.OpenAsync();
        await using var transaction = await connection.BeginTransactionAsync();

        try
        {
            var id = Guid.NewGuid();

            if (await HasReachedMaxRegistrationsAsync(connection, transaction, competitor.CompetitionId))
            {
                throw new Exception("Registrations have reached the limit");
            }

            var number = await GetNextNumberAsync(connection, transaction, competitor.CompetitionId);
            await InsertCompetitorAsync(connection, transaction, competitor, id, number);

            await transaction.CommitAsync();
            return (id, number.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong for request id: {competitor.RequestId}. Details: {ex.Message}");
            await transaction.RollbackAsync();
            return (Guid.Empty, string.Empty);
        }
    }

    private static async Task<bool> HasReachedMaxRegistrationsAsync(NpgsqlConnection connection, NpgsqlTransaction transaction, Guid competitionId)
    {
        var maxCompetitors = await GetMaxNumberOfCompetitorsAsync(connection, transaction, competitionId);
        var registeredCompetitors = await GetNumberOfRegisteredCompetitorsAsync(connection, transaction, competitionId);
        return registeredCompetitors >= maxCompetitors;
    }

    private static async Task<int> GetMaxNumberOfCompetitorsAsync(NpgsqlConnection connection, NpgsqlTransaction transaction, Guid competitionId)
    {
        const string sql = "SELECT \"maxCompetitors\" FROM competitions WHERE \"id\" = @competitionId;";
        return await connection.ExecuteScalarAsync<int>(sql, new { competitionId }, transaction);
    }

    private static async Task<int> GetNumberOfRegisteredCompetitorsAsync(NpgsqlConnection connection, NpgsqlTransaction transaction, Guid competitionId)
    {
        const string sql = "SELECT COUNT(id) FROM competitors WHERE \"competitionId\" = @competitionId;";
        return await connection.ExecuteScalarAsync<int>(sql, new { competitionId }, transaction);
    }

    private static async Task<long> GetNextNumberAsync(NpgsqlConnection connection, NpgsqlTransaction transaction, Guid competitionId)
    {
        const string sql = @"
            CALL generate_next_number(@competitionId);
            SELECT ""currentNumber"" FROM numerators WHERE ""competitionId"" = @competitionId;";
        return await connection.ExecuteScalarAsync<long>(sql, new { competitionId }, transaction);
    }

    private static async Task InsertCompetitorAsync(NpgsqlConnection connection, NpgsqlTransaction transaction, RegisterCompetitor registerCompetitor, Guid id, long number)
    {
        const string sql = @"
            INSERT INTO competitors (""id"", ""requestId"", ""competitionId"", ""number"", ""firstName"", ""lastName"", ""birthDate"", ""city"", ""phoneNumber"", ""contactPersonNumber"")
            VALUES (@id, @requestId, @competitionId, @number, @firstName, @lastName, @birthDate, @city, @phoneNumber, @contactPersonNumber);";
        
        await connection.ExecuteAsync(sql, new
        {
            id,
            requestId = registerCompetitor.RequestId,
            competitionId = registerCompetitor.CompetitionId,
            number,
            firstName = registerCompetitor.FirstName,
            lastName = registerCompetitor.LastName,
            birthDate = registerCompetitor.BirthDate,
            city = registerCompetitor.City,
            phoneNumber = registerCompetitor.PhoneNumber,
            contactPersonNumber = registerCompetitor.ContactPersonNumber
        }, transaction);
    }

    private static NpgsqlConnection CreateConnection()
    {
        var connectionString = Environment.GetEnvironmentVariable("PostgresConnectionString", EnvironmentVariableTarget.Process);
        return new NpgsqlConnection(connectionString);
    }
}
