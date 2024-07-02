using Contracts.Models.Request;
using Contracts.Models.Response;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.Services
{
    public interface IInmateService 
    {
        Task<IEnumerable<InmateModel>> GetInmatesAsync(int pageNumber, string search, string filter);
        Task<int> GetInmatesCountAsync(string search, string filter);
        Task<BulkInmateRegistrationResponseModel> BulkUploadInmatesAsync(IFormFile file, int officerId);
        Task<bool> TransferInmatesAsync(RequestTransferModel model, int officerId); 
    }
}
