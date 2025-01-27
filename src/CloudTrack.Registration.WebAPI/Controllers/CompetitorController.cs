using CloudTrack.Registration.Application.Competitors;
using Microsoft.AspNetCore.Mvc;

namespace CloudTrack.Registration.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CompetitorController(ICompetitorService competitorService) : ControllerBase
{
    private readonly ICompetitorService _competitorService = competitorService;

    [HttpGet("{competitionId:Guid}")]
    [ProducesResponseType(typeof(IEnumerable<CompetitorDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync(Guid competitionId)
    {
        var result = await _competitorService.GetCompetitorsAsync(competitionId);
        return Ok(result);
    }
}
