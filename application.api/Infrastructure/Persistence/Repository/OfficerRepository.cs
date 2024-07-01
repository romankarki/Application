using Application.Interfaces.Infrastructure;
using Domain.Entity;

namespace Infrastructure.Persistence.Repository
{
    public class OfficerRepository : IOfficerRepository
    {
        private readonly AppDbContext _db;
        public OfficerRepository(AppDbContext context) 
        {
           _db = context;
        }
    }
}
