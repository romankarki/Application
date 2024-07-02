using Application.Interfaces.Infrastructure;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;

namespace Infrastructure.Persistence.Repository
{
    public class OfficerRepository : IOfficerRepository
    {
        private readonly AppDbContext _db;
        public OfficerRepository(AppDbContext context) 
        {
           _db = context;
        }

        public async Task<Officer> FindOfficerByIdentificationNumberAsync(string identificationNumber)
        {
            var result =  _db.Officers.AsNoTracking().Where(x => x.IdentificationNumber == identificationNumber).FirstOrDefault();
            return result;
        }
    }
}
