using Application.Implementation;
using Application.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Dependency
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services) 
        {
            services.AddTransient<IOfficerService, OfficerService>();
            services.AddTransient<IInmateService, InmateService>();
            services.AddTransient<IFacilityService, FacilityService>();

            return services;
        }
    }
}
