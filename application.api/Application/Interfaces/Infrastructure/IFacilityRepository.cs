using Contracts.Models.Response;
using Domain.Entity;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.Infrastructure
{
    public interface IFacilityRepository
    {
        Task<IEnumerable<Facility>> GetFacilityByPageNumber(int pageNumber);
    }
}
