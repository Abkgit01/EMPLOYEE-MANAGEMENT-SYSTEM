using API.Data;
using API.IServices;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.ApplicationServicesExtention
{
    public static class ApplicationServiceExtention
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config, WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ITokenService, TokenService>();

            builder.Services.AddDbContext<DataContext>(x =>
                x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
                ));

            return services;
        }
    }
}
