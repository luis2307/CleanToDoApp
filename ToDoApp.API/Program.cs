using Asp.Versioning.ApiExplorer;
using ToDoApp.API.ApplicationBuilderExtensions;
using ToDoApp.API.Extensions;
using ToDoApp.API.Extensions.Swagger;
using ToDoApp.API.Extensions.Versioning;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationDependencies();
builder.Services.AddPersistenceDependencies(builder.Configuration);
builder.Services.AddInfrastructureDependencies(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddVersioning();
builder.Services.AddSwagger();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        // build a swagger endpoint for each discovered API version 
        foreach (var description in provider.ApiVersionDescriptions)
        {
            c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });
    app.ApplyMigrations();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();