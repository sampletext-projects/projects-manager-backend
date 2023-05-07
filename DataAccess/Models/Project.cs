namespace DataAccess.Models;

public class Project
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string? Description { get; set; }

    public ProjectStyle Style { get; set; }

    public Visibility Visibility { get; set; }

    public virtual ICollection<ProjectTask> Tasks { get; set; }

    public virtual ICollection<Participation> Participations { get; set; }

    public virtual ICollection<AppUser> Users { get; set; }
}