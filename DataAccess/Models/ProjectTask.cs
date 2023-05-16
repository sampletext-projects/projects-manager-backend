namespace DataAccess.Models;

public class ProjectTask
{
    public Guid Id { get; set; }
    
    public string Title { get; set; }
    
    public string? Description { get; set; }

    public TaskStatus Status { get; set; }
    
    public Guid ProjectId { get; set; }
    
    public virtual Project Project { get; set; }
    
    public Guid CreatorId { get; set; }
    
    public virtual AppUser Creator { get; set; }

    public virtual ICollection<TaskComment>? Comments { get; set; }
}