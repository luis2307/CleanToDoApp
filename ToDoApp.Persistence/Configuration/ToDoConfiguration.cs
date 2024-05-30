using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Persistence.Configuration;

public class ToDoConfiguration : IEntityTypeConfiguration<ToDoItem>
{
    public void Configure(EntityTypeBuilder<ToDoItem> builder)
    {
        builder.HasData
        (
            new ToDoItem
            {
                Id = 1,
                Description = "Test Description",
                IsDone = true
            }
        );
    }
}
