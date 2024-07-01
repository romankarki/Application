using Contracts.Models.Request;
using Contracts.Models.Response;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.Services
{
    public interface IOfficerService
    {
        Task<OfficerModel> AuthenticateOfficerAsync(string identificationNumbber, string password);
        Task<OfficerModel> GetOfficerByIdAsync(int id);
        Task<OfficerModel> SignUpAsync(RegisterOfficerModel model, int officerId = 0);
        Task<BulkRegisterOfficerModel> BulkRegistrationAsync(IFormFile file, int officerId);
    }
}
