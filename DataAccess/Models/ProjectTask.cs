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
    
    public AppUser Creator { get; set; }
}