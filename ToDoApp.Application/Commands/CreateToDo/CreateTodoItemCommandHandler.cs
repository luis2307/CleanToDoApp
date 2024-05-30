using MediatR;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Interfaces;

namespace ToDoApp.Application.Commands.CreateToDo;

public class CreateToDoItemCommandHandler : IRequestHandler<CreateToDoItemCommand, int>
{
    private readonly IToDoRepository _toDoRepository;

    public CreateToDoItemCommandHandler(IToDoRepository toDoRepository)
    {
        _toDoRepository = toDoRepository;
    }

    public Task<int> Handle(
        CreateToDoItemCommand request, CancellationToken cancellationToken)
    {
        var item = new ToDoItem
        {
            Description = request.Description
        };

        return _toDoRepository.CreateAsync(item);
    }
}
