using Application.Interfaces.Services;
using Contracts.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    public class OfficerController : BaseController 
    {
        private readonly IOfficerService _officerService;
        public OfficerController (IOfficerService officerService)
        {
            _officerService = officerService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate-officer")]
        public async Task<IActionResult> AuthenticateAsync([FromBody] AuthenticateOfficerModel model)
        {
            var result = await _officerService.AuthenticateOfficerAsync(model.IdentificationNumber, model.Password);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("register-officer")]
        public async Task<IActionResult> RegisterOfficerAsync([FromBody] RegisterOfficerModel model)
        {
            var result = await _officerService.SignUpAsync(model);
            return Ok(result);
        }

        [HttpPost("bulk-upload-officer")]
        public async Task<IActionResult> UploadOfficerAsync([FromForm] BulkRegisterModel model)
        {
            if (model.file == null || model.file.Length == 0) return BadRequest("No file Selected");
            var result = await _officerService.BulkRegistrationAsync(model.file, OfficerID); 
            return Ok(result);
        }
       
    }
}
