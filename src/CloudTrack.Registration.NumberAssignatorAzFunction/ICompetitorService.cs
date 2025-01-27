
using CloudTrack.Registration.Application.CompetitorRegistration;
using System;
using System.Threading.Tasks;

namespace CloudTrack.Registration.NumberAssignatorAzFunction;

public interface ICompetitorService
{
    Task<(Guid id, string number)> RegisterCompetitorAndReturnNumber(RegisterCompetitor competitor);
}
