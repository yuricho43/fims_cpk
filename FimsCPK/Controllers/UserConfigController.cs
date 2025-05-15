using FimsCPK.Data;
using FimsCPK.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using Telerik.Blazor.Components;

namespace FimsCPK.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserConfigController : ControllerBase
    {
        private readonly FimsDbContext _context;

        public UserConfigController(FimsDbContext context)
        {
            _context = context;
        }


        [HttpPost("save")]
        public async Task<IActionResult> SaveUserConfig([FromBody] UserConfiguration request)
        {
            var userId = request.UserId;

            var existingState = await _context.UserConfigEntities
                .FirstOrDefaultAsync(gs => gs.UserId == userId && gs.ConfigName == request.ConfigName);

            if (existingState == null)
            {
                existingState = new UserConfigEntity
                {
                    UserId = userId,
                    ConfigName = request.ConfigName
                };
                _context.UserConfigEntities.Add(existingState);
            }

            existingState.StateJson = JsonSerializer.Serialize(request);
            existingState.LastUpdated = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("load")]
        public async Task<ActionResult<UserConfiguration>> LoadUserConfig(string configName, string userId)
        {
            //string userId = "yuricho";
            var stateEntity = await _context.UserConfigEntities
                .FirstOrDefaultAsync(gs => gs.UserId == userId && gs.ConfigName == configName);

            if (stateEntity == null)
            {
                return NotFound();
            }

            UserConfiguration listColumns = JsonSerializer.Deserialize<UserConfiguration> (stateEntity.StateJson);
            return Ok(listColumns);
        }
    }

}
