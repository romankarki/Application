using Infrastructure.Interface;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/v1/[controller]")]
public class TestController : Controller
{
    private readonly ITestRepository _test;
    public TestController(ITestRepository testRepository)
    {
        _test = testRepository;
    }

    [HttpGet("get-test-data")]
    public async Task<IActionResult> GetDataAsync()
    {
        var result = await _test.GetTestDataAsync();
        return Ok(result);
    }



}