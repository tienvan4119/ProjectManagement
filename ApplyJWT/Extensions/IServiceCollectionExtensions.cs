using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectManager.API.Services;
using ProjectManager.Domain.Interface;
using ProjectManager.Infrastructure;
using ProjectManager.Infrastructure.Repositories;

namespace ProjectManager.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddScoped(typeof(IRepository<>), typeof(Repository<>))
                .AddScoped<IProjectRepository, ProjectRepository>()
                .AddScoped<ITodoRepository, TodoRepository>()
                .AddScoped<IClientRepository, ClientRepository>();
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddScoped<ProjectService>()
                .AddScoped<ClientService>();
        }
    }
}
