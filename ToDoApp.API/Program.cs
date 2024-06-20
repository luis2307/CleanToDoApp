using Asp.Versioning.ApiExplorer;
using ToDoApp.API.ApplicationBuilderExtensions;
using ToDoApp.API.Extensions;
using ToDoApp.API.Extensions.Redis;
using ToDoApp.API.Extensions.Swagger;
using ToDoApp.API.Extensions.Versioning;


var builder = WebApplication.CreateBuilder(args);

// Configurar Kestrel para escuchar en los puertos 5000 y 5001
//builder.WebHost.ConfigureKestrel(options =>
//{
//options.ListenAnyIP(5000); // HTTP:localhost:5000 /swager/
//options.ListenAnyIP(5001, listenOptions => listenOptions.UseHttps()); // HTTPS
//});
//builder.Services.AddMemoryCache();
//builder.Services.AddResilientHttpClient("ResilientClient");
  
builder.Services.AddApplicationDependencies();
builder.Services.AddPersistenceDependencies(builder.Configuration);
builder.Services.AddInfrastructureDependencies(builder.Configuration);
builder.Services.AddRedisCache(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddLogging();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddVersioning();
builder.Services.AddSwagger();

var app = builder.Build();


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


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

 