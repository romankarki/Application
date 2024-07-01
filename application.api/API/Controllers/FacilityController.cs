using API.Attributes;
using Application.Interfaces.Services;
using Contracts.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    public class FacilityController : BaseController
    {
        private readonly IFacilityService _facilityService;
        public FacilityController(IFacilityService facilityService)
        {
            _facilityService = facilityService;
        }

        [HttpGet("get-all-facilites/{pageNumber:int}")]
        public async Task<IActionResult> GetAllFacilitiesAsync(int pageNumber)
        {
            var result = await _facilityService.GetALlFacilitiesAsync(pageNumber);
            return Ok(result);
        }

        [HttpPost("bulk-upload-facilities")]
        public async Task<IActionResult> BulkUploadFacilitiesAsync([FromForm] BulkRegisterModel model)
        {
            if (model.file == null || model.file.Length == 0) return BadRequest("No file Selected");
            var result = await _facilityService.BulkUploadFacilityAsync(model.file, OfficerID);
            return Ok(result);
        }

    }
}
