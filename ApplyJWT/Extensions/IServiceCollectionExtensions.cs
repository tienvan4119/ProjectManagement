﻿using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectManager.API.Services;
using ProjectManager.API.ViewModels.Project;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Base.Interfaces;
using ProjectManager.Infrastructure.Base.Repositories;
using ProjectManager.Infrastructure.Interfaces;
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
                .AddScoped<IClientRepository, ClientRepository>()
                .AddScoped<IMilestoneRepository, MilestoneRepository>()
                .AddScoped<IDocumentRepository, DocumentRepository>()
                .AddScoped<IAppointmentRepository, AppointmentRepository>();
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddScoped<ProjectService>()
                .AddScoped<ClientService>()
                .AddScoped<MilestoneService>()
                .AddScoped<TaskService>()
                .AddScoped<DocumentService>();
        }
    }
}
