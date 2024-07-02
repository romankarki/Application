using Domain.Entity;

namespace Application.Interfaces.Infrastructure
{
    public interface IOfficerRepository  
    {
        Task<Officer> FindOfficerByIdentificationNumberAsync(string identificationNumber);

    }
}
