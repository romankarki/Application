
using Application.Interfaces.Infrastructure;
using Contracts.Models.Response;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository
{
    public class InmateRepository : IInmateRepository
    {
        private readonly AppDbContext _db;
        private int PageSize = 25;
        public InmateRepository(AppDbContext context) 
        {
            _db = context;
        }

        public async Task<IEnumerable<InmateModel>> GetInmatesAsync(int page, string search, string filter)
        {
            IQueryable<InmateData> query = _db.InmateDatas.Where(x=>x.IsDeleted ==false).OrderByDescending(x=>x.Id);
            if(!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(filter))
            {
                if(filter.Equals("identification", StringComparison.OrdinalIgnoreCase))
                {
                    query  = query.Where(q => q.IdentificationNumber.ToLower().Contains(search.ToLower()));
                }
                else if(filter.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(q=>q.Name.ToLower().Contains(search.ToLower()));
                }
            }
            var list = (from inm in query.AsNoTracking()
                        join fac in _db.Facilities on inm.CurrentFacility equals fac.Id
                        where inm.IsDeleted == false
                        select new InmateModel
                        {
                            Id = inm.Id,
                            IdentificationNumber = inm.IdentificationNumber,
                            FacilityName = fac.FacilityName,
                            FacilityCode = fac.FacilityCode,
                            Email = inm.ContactEmail,
                            PhoneNumber = inm.ContactPhone,
                            InmateName = inm.Name
                        });
            if(!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(filter))
            {

                if(filter.Equals("facility", StringComparison.OrdinalIgnoreCase))
                {
                    list = (from l in list
                            where l.FacilityName.ToLower().Contains(search.ToLower())
                                select l
                        ); 
                }

            }
            if(page > 0)
            {
                list = list.Skip((page - 1) * PageSize).Take(PageSize);
            }
            return await list.ToListAsync();
        }
    }
}
