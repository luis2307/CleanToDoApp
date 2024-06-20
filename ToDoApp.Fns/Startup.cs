using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using ToDoApp.Fns.Extensions;


[assembly: FunctionsStartup(typeof(ToDoApp.Fns.Startup))]
namespace ToDoApp.Fns
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {

            builder.Services.AddApplicationDependencies();
            builder.Services.AddPersistenceDependencies(); 

        }
    }
}
