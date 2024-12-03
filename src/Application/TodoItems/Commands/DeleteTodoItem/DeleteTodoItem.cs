using ToDoApp.Application.Common.Interfaces;

namespace ToDoApp.Application.TodoItems.Commands.DeleteTodoItem;

public record DeleteTodoItemCommand : IRequest<int>
{
    public List<int> Ids { get; private set; }

    public DeleteTodoItemCommand()
    {
        Ids = new List<int>();
    }
}

public class DeleteTodoItemCommandHandler : IRequestHandler<DeleteTodoItemCommand, int>
{
    private readonly IApplicationDbContext _context;

    public DeleteTodoItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }


    public async Task<int> Handle(DeleteTodoItemCommand request, CancellationToken cancellationToken)
    {
        if (request.Ids == null || !request.Ids.Any())
        {
            throw new ArgumentException("No IDs provided.");
        }

        var items = await _context.TodoItems
            .Where(x => request.Ids.Contains(x.Id))
            .ToListAsync(cancellationToken);

        if (!items.Any())
        {
            throw new KeyNotFoundException("No items found to delete.");
        }

        _context.TodoItems.RemoveRange(items);
        await _context.SaveChangesAsync(cancellationToken);

        return items.Count;
    }
}
