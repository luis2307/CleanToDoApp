using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using ToDoApp.Application.Commands.CreateToDo;
using ToDoApp.Application.Queries.ToDoItem;

namespace ToDoApp.Fns
{
    public class ToDoItemFunction
    {
        private readonly ISender _sender;

        public ToDoItemFunction(ISender sender)
        {
            _sender = sender;
        }

        [FunctionName("GetToDoItems")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todoitem")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var result = await _sender.Send(new ToDoItemQuery()); 
            return new OkObjectResult(result);
        }


        [FunctionName("CreateToDoItem")]
        public async Task<IActionResult> CreateToDoItem(
          [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "todoitem")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var command = JsonConvert.DeserializeObject<CreateToDoItemCommand>(requestBody);

            var result = await _sender.Send(command);

            return new StatusCodeResult(StatusCodes.Status201Created);
        }
    }
}
