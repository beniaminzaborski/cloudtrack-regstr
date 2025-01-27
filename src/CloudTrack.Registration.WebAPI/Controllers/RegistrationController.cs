using CloudTrack.Registration.Application.CompetitorRegistration;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CloudTrack.Registration.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegistrationController(IRegistrationService registrationService) : ControllerBase
{
    private readonly IRegistrationService _registrationService = registrationService;

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> RegisterAsync([FromBody] RegistrationRequestDto dto)
    {
        var requestId = await _registrationService.RegisterAsync(dto);
        return Accepted(new { RequestId = requestId });
    }
}
