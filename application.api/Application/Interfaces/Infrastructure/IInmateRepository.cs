using Contracts.Models.Response;

namespace Application.Interfaces.Infrastructure
{
    public interface IInmateRepository
    {
        Task<IEnumerable<InmateModel>> GetInmatesAsync(int page, string search, string filter);
    }
}
