using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Cms;
using System.Text.Json;

namespace ToDoApp.API.Controllers
{
    [Route("api/test-polly")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<TestController> _logger;

        public TestController(IHttpClientFactory httpClientFactory, ILogger<TestController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var client = _httpClientFactory.CreateClient("ResilientClient");
            // var response = await client.GetAsync("https://jsonplaceholder.typicode.com/posts/1");
            // var response = await client.GetAsync("https://jsonplacexholder.typicode.com/postsx/1");


            // Add a custom header to identify cached responses
            //var request = new HttpRequestMessage(HttpMethod.Get, "https://fakestoreapi.com/products");  
            // var response = await client.SendAsync(request);
            var response = await client.GetAsync("https://fakestoreapi.com/products");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var products = JsonSerializer.Deserialize<List<Product>>(content);
                var fromCache = response.Headers.Contains("X-Polly-Cache");

                foreach (var product in products)
                {
                    product.origen = fromCache ? "Cache" : "Source";
                }

                _logger.LogInformation("Request successful. Returning products.");
                return Ok(products);
            }

            _logger.LogError("Request failed with status code: {StatusCode}", response.StatusCode);
            return StatusCode((int)response.StatusCode, response.ReasonPhrase);

        }
    }
    public class Rating
    {
        public double rate { get; set; }
        public int count { get; set; }
    }
    internal class Product
    {
        public int id { get; set; }
        public string title { get; set; }
        public double price { get; set; }
        public string description { get; set; }
        public string category { get; set; }
        public string image { get; set; }
        public Rating rating { get; set; }
        public string origen { get; set; }
    }
}
