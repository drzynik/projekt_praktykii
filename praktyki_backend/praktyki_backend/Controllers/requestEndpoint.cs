using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using praktyki_backend.Models;
using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace praktyki_backend.Controllers
{
    [ApiController]
    [Route("api/webhook")]
    public class WebhookController : ControllerBase
    {
        private readonly dbcontext _context;

        public WebhookController(dbcontext context)
        {
            _context = context;
        }
        [HttpPost("gamemaster")]
        public async Task<IActionResult> Gamemaster([FromBody] GamemasterRequest model)
        {
            var name = model.Name;
            var type = model.Type;
            var message = model.Message;

            var request = new GamemasterRequest
            {
                Name = name,
                Type = type,
                Message = message
            };
            _context.GamemasterRequests.Add(request);
            await _context.SaveChangesAsync();

            return Ok(request);
        }
        [HttpGet("last")]
        public async Task<IActionResult> GetLastRequest()
        {
            var lastRequest = await _context.GamemasterRequests
                .OrderByDescending(r => r.Request_Id)
                .FirstOrDefaultAsync();

            if (lastRequest == null)
                return NotFound();

            return Ok(lastRequest);
        }
    }
}
