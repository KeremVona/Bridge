using Bridge.Server.Data;
using Bridge.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bridge.Server.Repositories;

public class TodoRepository : ITodoRepository
{
    private readonly AppDbContext _context;

    public TodoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Todo>> GetAllAsync()
    {
        return await _context.Todos.ToListAsync();
    }

    public async Task<Todo?> GetByIdAsync(int id)
    {
        return await _context.Todos.FindAsync(id);
    }

    public async Task<Todo> MakeAsync(Todo todo)
    {
        _context.Todos.Add(todo);
        await _context.SaveChangesAsync();
        return todo;
    }

    public async Task UpdateAsync(Todo todo)
    {
        _context.Entry(todo).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var todo = await _context.Todos.FindAsync(id);
        if (todo != null)
        {
            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();
        }
    }
}
