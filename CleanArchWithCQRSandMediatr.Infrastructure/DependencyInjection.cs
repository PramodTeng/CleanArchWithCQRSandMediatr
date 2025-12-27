using CleanArchWithCQRSandMediatr.Domain.Interface;
using CleanArchWithCQRSandMediatr.Infrastructure.Data;
using CleanArchWithCQRSandMediatr.Infrastructure.Repository;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchWithCQRSandMediatr.Infrastructure;

public static class DependencyInjection
{

    public static IServiceCollection AddInfrastructureDI(this IServiceCollection service, IConfiguration configuration)
    {

        service.AddDbContext<BlogDBContext>(optionsAction: options =>
        {
            options.UseSqlite(configuration.GetConnectionString("BlogDBContext") ?? throw new InvalidOperationException("Connection string not found"));

        });

        service.AddTransient<IBlogRepository, BlogRepository>();
        return service;



    }

}
