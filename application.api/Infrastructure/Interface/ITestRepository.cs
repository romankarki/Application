namespace Infrastructure.Interface;
public interface ITestRepository
{
    Task<dynamic> GetTestDataAsync();
}