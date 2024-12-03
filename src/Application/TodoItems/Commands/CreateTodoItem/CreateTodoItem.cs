using System.ComponentModel.DataAnnotations;
using System.Reflection;
using ToDoApp.Application.Common.Interfaces;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.TodoItems.Commands.CreateTodoItem;

public record CreateTodoItemCommand : IRequest<int>
{
    public int ListId { get; set; }
    [Required(ErrorMessage = "Title is required")]
    public string? Title { get; set; }

    public CreateTodoItemCommand() { }

    public CreateTodoItemCommand(int listId, string? title)
    {
        ListId = listId;
        Title = title;
    }
}

public class CreateTodoItemCommandHandler : IRequestHandler<CreateTodoItemCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateTodoItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var newTodoItem = new TodoItem
        {
            ListId = request.ListId,
            Title = request.Title,
            Done = false
        };

        _context.TodoItems.Add(newTodoItem);
        await _context.SaveChangesAsync(cancellationToken);

        return newTodoItem.Id;
    }
}
