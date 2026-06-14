using Bridge.Server.Entities;

namespace Bridge.Server.Repositories;

public interface ITodoRepository
{
    Task<IEnumerable<Todo>> GetAllAsync();
    Task<Todo?> GetByIdAsync(int id);
    Task<Todo> MakeAsync(Todo todo);
    Task UpdateAsync(Todo todo);
    Task DeleteAsync(int id);
}
