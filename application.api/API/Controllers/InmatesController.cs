using API.Attributes;
using Application.Interfaces.Services;
using Contracts.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    public class InmatesController : BaseController 
    {
        private readonly IInmateService _inmateService;
        public InmatesController(IInmateService inmateService)
        {
            _inmateService = inmateService;
        }

        [HttpGet("get-inmates/{pageNum:int}/{search?}/{filter?}")]
        public async Task<IActionResult> GetInmatesPageAsync(int pageNum, string search = null, string filter = null)
        {
            var result = await _inmateService.GetInmatesAsync(pageNum, search, filter);
            return Ok(result);

        }
        [HttpGet("get-inmates-count/{pageNum:int}/{search?}/{filter?}")]
        public async Task<IActionResult> GetInmatesCountasync(string search = null, string filter = null)
        {
            var result = await _inmateService.GetInmatesCountAsync(search, filter);
            return Ok(result);

        }

        [HttpPost("upload-inmates")]
        public async Task<IActionResult> UploadImatesAsync([FromForm] BulkRegisterModel model)
        {
            if (model.file == null || model.file.Length == 0) return BadRequest("No file Selected");
            var result = await _inmateService.BulkUploadInmatesAsync(model.file, OfficerID);
            return Ok(result);
        }

        [HttpPost("transfer-inmates")]
        public async Task<IActionResult> TransferInmatesAsync([FromBody] RequestTransferModel model)
        {
            var result = await _inmateService.TransferInmatesAsync(model, OfficerID); 
            return Ok("");
        }

    }
}
