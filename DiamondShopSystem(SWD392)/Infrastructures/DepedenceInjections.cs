using Application.Interfaces;
using Application.Repositories;
using Application.Services;
using Infrastructures.Mappers;
using Infrastructures.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures
{
    public static class DepedenceInjections
    {
        public static IServiceCollection AddInfrastructuresService(this IServiceCollection services, string databaseConnection)
        {
            services.AddScoped<ICurrentTime, CurrentTime>();
            //Account
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAccountService, AccountService>();

            services.AddDbContext<AppDbContext>(option =>
            {
                option.UseSqlServer(databaseConnection);
            });

            services.AddAutoMapper(typeof(MapperConfigurationsProfile).Assembly);
            return services;
        }
    }
}
