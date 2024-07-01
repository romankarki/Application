using Contracts.Models.Response;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.Services
{
    public interface IFacilityService
    {
        Task<BulkFacilityRegistrationResponseModel> BulkUploadFacilityAsync(IFormFile file, int officerId);
        Task<IEnumerable<FacilitiesModel>> GetALlFacilitiesAsync(int pageNumber);
    }
}
