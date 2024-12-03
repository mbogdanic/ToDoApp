using System.ComponentModel.DataAnnotations;
using ToDoApp.Application.Common.Interfaces;

namespace ToDoApp.Application.TodoItems.Commands.CompleteTodoItem;

public record CompleteTodoItemCommand : IRequest<int>
{
    public List<int> Ids { get; private set; }

    public CompleteTodoItemCommand()
    {
        Ids = new List<int>();
    }
}

public class CompleteTodoItemCommandHandler : IRequestHandler<CompleteTodoItemCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IValidator<CompleteTodoItemCommand> _validator;

    public CompleteTodoItemCommandHandler(IApplicationDbContext context, IValidator<CompleteTodoItemCommand> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<int> Handle(CompleteTodoItemCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new FluentValidation.ValidationException(validationResult.Errors);
        }

        var items = await _context.TodoItems
            .Where(x => request.Ids.Contains(x.Id))
            .ToListAsync(cancellationToken);

        foreach (var item in items)
        {
            item.Done = true;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return items.Count;
    }
}
