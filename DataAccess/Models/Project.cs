namespace DataAccess.Models;

public class Project
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }
    
    public Guid CreatorId { get; set; }
    
    public virtual AppUser Creator { get; set; }
}