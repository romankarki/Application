using Application.Interfaces.Infrastructure;
using Contracts.Models.Response;
using Domain.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository
{
    public class FacilityRepository : IFacilityRepository
    {

        private readonly AppDbContext _db;
        private int PageSize = 25;
        public FacilityRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Facility>> GetFacilityByPageNumber(int pageNumber)
        {
            IQueryable<Facility> query = _db.Facilities.Where(x=>x.IsDeleted == false).OrderByDescending(x=>x.Id);
            query = query.Skip((pageNumber - 1) * PageSize).Take(PageSize);
            return await query.ToListAsync();
        }
    }
}
