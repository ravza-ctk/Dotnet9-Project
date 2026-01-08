using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using WebApplication1.Core.Interfaces;
using WebApplication1.Data.Repositories;
using WebApplication1.Service.Mappings;
using WebApplication1.Service.Services;

namespace WebApplication1.Service;

public static class DependencyInjection
{
    public static IServiceCollection AddServiceLayer(this IServiceCollection services, IConfiguration configuration)
    {
        // AutoMapper
        services.AddAutoMapper(typeof(MapProfile).Assembly);

        // Services
        services.AddScoped(typeof(IGenericService<,>), typeof(GenericService<,>));
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IMerchandiseService, MerchandiseService>();
        services.AddScoped<IPurchaseService, PurchaseService>();
        
        return services;
    }
}
