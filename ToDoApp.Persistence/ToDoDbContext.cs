using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Domain.Entities;
using ToDoApp.Persistence.Configuration;

namespace ToDoApp.Persistence;

public class ToDoDbContext : IdentityDbContext<User>
{
    public ToDoDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new ToDoConfiguration());
    }

    public DbSet<ToDoItem> ToDoItems { get; set; }
}
