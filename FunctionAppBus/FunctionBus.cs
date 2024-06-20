using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Logging;

namespace FunctionAppBus
{
    public class FunctionBus
    {
        private readonly ILogger<FunctionBus> _logger;


        public FunctionBus(ILogger<FunctionBus> log)
        {
            _logger = log;
        }

        [FunctionName("FunctionBus")]
        public void Run([ServiceBusTrigger("items", "bus", Connection = "SERVICE_BUS")] string mySbMsg)
        {
            _logger.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
        }

        //[FunctionName("FunctionBus")]
        //public async Task RunsAsync([ServiceBusTrigger("items", "bus", Connection = "SERVICE_BUS")]
        //    ServiceBusReceivedMessage message,
        //    ServiceBusMessageActions messageActions
        //)
        //{

        //    try
        //    {

        //        // int i = int.Parse("1a");


        //        await messageActions.CompleteMessageAsync(message);
        //    }
        //    catch (Exception ex)
        //    {

        //        await messageActions.DeadLetterMessageAsync(message, ex.InnerException.Message, ex.InnerException.StackTrace);
        //    } 
        //}
    } 
}
