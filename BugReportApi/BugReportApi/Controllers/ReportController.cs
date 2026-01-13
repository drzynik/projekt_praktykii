using Microsoft.AspNetCore.Mvc;
using BugReportApi.Models;
using BugReportApi.Services;

namespace BugReportApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportController : ControllerBase
{
    private readonly IEmailService _emailService;

    public ReportController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost]
    public async Task<IActionResult> SendReport([FromBody] BugReportDto dto)
    {
        if (dto == null || string.IsNullOrWhiteSpace(dto.Description) || string.IsNullOrWhiteSpace(dto.Email)){
            return BadRequest("Opis i email są wymagane.");
        }

        await _emailService.SendAsync(dto.Email, dto.Description);

        return Ok("Zgłoszenie wysłane.");
    }
}

