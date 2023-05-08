namespace DataAccess.Models;

public class ProjectComment
{
    public Guid Id { get; set; }

    public string Content { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid ProjectId { get; set; }

    public virtual Project Project { get; set; }

    public Guid AuthorId { get; set; }

    public virtual AppUser Author { get; set; }
}