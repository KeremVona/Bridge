using Bridge.Server.Entities;
using Bridge.Server.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Bridge.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    private readonly ITodoRepository _repository;

    public TodoController(ITodoRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Todo>>> GetTodos()
    {
        var todos = await _repository.GetAllAsync();
        return Ok(todos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Todo>> GetTodo(int id)
    {
        var todo = await _repository.GetByIdAsync(id);
        if (todo == null) return NotFound();
        return Ok(todo);
    }

    [HttpPost]
    public async Task<ActionResult<Todo>> CreateTodo(Todo todo)
    {
        var created = await _repository.MakeAsync(todo);
        return CreatedAtAction(nameof(GetTodo), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTodo(int id, Todo todo)
    {
        if (id != todo.Id) return BadRequest();
        await _repository.UpdateAsync(todo);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodo(int id)
    {
        await _repository.DeleteAsync(id);
        return NoContent();
    }
}
