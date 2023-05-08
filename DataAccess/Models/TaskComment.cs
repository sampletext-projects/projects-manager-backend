namespace DataAccess.Models;

public class TaskComment
{
    public Guid Id { get; set; }

    public string Content { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid TaskId { get; set; }

    public virtual ProjectTask Task { get; set; }

    public Guid AuthorId { get; set; }

    public virtual AppUser Author { get; set; }
}