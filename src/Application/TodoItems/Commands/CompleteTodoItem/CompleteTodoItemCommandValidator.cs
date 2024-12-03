namespace ToDoApp.Application.TodoItems.Commands.CompleteTodoItem;

public class CompleteTodoItemCommandValidator : AbstractValidator<CompleteTodoItemCommand>
{
    public CompleteTodoItemCommandValidator()
    {
        RuleFor(command => command.Ids)
            .NotNull().WithMessage("Ids cannot be null.")
            .NotEmpty().WithMessage("At least one ID must be provided.");
    }
}
