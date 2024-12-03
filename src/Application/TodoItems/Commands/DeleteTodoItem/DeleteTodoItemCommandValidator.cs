namespace ToDoApp.Application.TodoItems.Commands.DeleteTodoItem;

public class DeleteTodoItemCommandValidator : AbstractValidator<DeleteTodoItemCommand>
{
    public DeleteTodoItemCommandValidator()
    {
        RuleFor(command => command.Ids)
            .NotNull().WithMessage("Ids cannot be null.")
            .NotEmpty().WithMessage("At least one ID must be provided.");
    }
}
