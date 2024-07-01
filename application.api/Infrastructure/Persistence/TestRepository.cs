using Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class TestRepository : ITestRepository
{
    private readonly AppDbContext _db;
    public TestRepository(AppDbContext context)
    {
        _db = context;
    }

    public async Task<dynamic> GetTestDataAsync()
    {
        var result = await _db.Tests.ToListAsync();
        return result;
    }
}
