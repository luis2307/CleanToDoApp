using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using ToDoApp.Domain.Interfaces;
using ToDoApp.Persistence;
using ToDoApp.Persistence.Repository;

namespace ToDoApp.Fns.Extensions;

public static class ServiceExtensions
{  
    public static void AddApplicationDependencies(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Application.AssemblyReference).Assembly));
    }

    public static void AddPersistenceDependencies(this IServiceCollection services)
    {
        services.AddDbContext<ToDoDbContext>(opts =>
            opts.UseSqlServer(Environment.GetEnvironmentVariable("sqlConnection"),
            b => b.MigrationsAssembly("ToDoApp.Persistence")));

        services.AddScoped<IToDoRepository, SqlToDoRepository>();
    } 
}
