using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;
using ToDoApp.Domain.Interfaces;

namespace ToDoApp.Application.Queries.ToDoItem;

public class ToDoItemQueryHandler : IRequestHandler<ToDoItemQuery, List<Domain.Entities.ToDoItem>>
{
    private readonly IToDoRepository _toDoRepository;
    private readonly IDistributedCache _distributedCache;

    public ToDoItemQueryHandler(IToDoRepository toDoRepository, IDistributedCache distributedCache)
    {
        _toDoRepository = toDoRepository ?? throw new ArgumentNullException(nameof(toDoRepository));
        _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
    }

    public async Task<List<Domain.Entities.ToDoItem>> Handle(ToDoItemQuery request, CancellationToken cancellationToken)
    {


        var response = new List<Domain.Entities.ToDoItem>();
        var cacheKey = "TODO-key";
        var redisItems = await _distributedCache.GetAsync(cacheKey);

        if (redisItems != null)
        {
            response = JsonSerializer.Deserialize<List<Domain.Entities.ToDoItem>>(redisItems);

        }
        else
        {
            response = await _toDoRepository.GetAllAsync(); 
            var todojson = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response));
            var options = new DistributedCacheEntryOptions()
               .SetAbsoluteExpiration(DateTime.Now.AddHours(8))
               .SetSlidingExpiration(TimeSpan.FromMinutes(60));
            await _distributedCache.SetAsync(cacheKey, todojson, options); 
        }  
        return response;
    }
}