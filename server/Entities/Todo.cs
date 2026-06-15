namespace Bridge.Server.Entities;

public class Todo
{
  public int Id { get; set; }
  public string Title { get; set; } = string.Empty;
  public bool IsCompleted { get; set; }
  public DateTime MadeAt { get; set; } = DateTime.UtcNow;
}
