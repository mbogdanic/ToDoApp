using Microsoft.AspNetCore.Mvc;
using ToDoApp.Application.TodoItems.Commands.CreateTodoItem;
using ToDoApp.Application.Common.Exceptions;
using ToDoApp.Application.TodoItems.Commands.DeleteTodoItem;
using ToDoApp.Application.TodoItems.Commands.CompleteTodoItem;

namespace ToDoApp.Web.Controllers;

public class ToDoItemController : Controller
{
    private readonly ISender _sender;

    public ToDoItemController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTodoItemCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var id = await _sender.Send(command, cancellationToken);
            return Redirect("/");
        }
        catch (ValidationException ex)
        {
            TempData["Errors"] = string.Join("\n", ex.Errors.SelectMany(e => e.Value));
        }
        catch (Exception)
        {
            TempData["Errors"] = "An error occurred while creating the todo item.";
        }

        return Redirect("/");
    }

    [HttpPost]
    public async Task<IActionResult> Delete([FromForm] List<int> id, CancellationToken cancellationToken)
    {
        if (id == null || !id.Any())
        {
            TempData["Errors"] = "No items selected to delete";
            return Redirect("/");
        }

        try
        {
            var command = new DeleteTodoItemCommand();
            command.Ids.AddRange(id);
            await _sender.Send(command, cancellationToken);
        }
        catch (ValidationException ex)
        {
            TempData["Errors"] = string.Join("\n", ex.Errors.SelectMany(e => e.Value));
        }

        return Redirect("/");
    }

    [HttpPost]
    public async Task<IActionResult> Complete([FromForm] List<int> id, CancellationToken cancellationToken)
    {
        if (id == null || !id.Any())
        {
            TempData["Errors"] = "No items selected to complete";
            return Redirect("/");
        }

        try
        {
            var command = new CompleteTodoItemCommand();
            command.Ids.AddRange(id);
            await _sender.Send(command, cancellationToken);
        }
        catch (ValidationException ex)
        {
            TempData["Errors"] = string.Join("\n", ex.Errors.SelectMany(e => e.Value));
        }

        return Redirect("/");
    }
}
