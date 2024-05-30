using Microsoft.EntityFrameworkCore;
using ToDoApp.Persistence;

namespace ToDoApp.API.ApplicationBuilderExtensions;

public static class ApplicationBuilderExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using ToDoDbContext dbContext = scope.ServiceProvider.GetRequiredService<ToDoDbContext>();

        dbContext.Database.Migrate();
    }
}