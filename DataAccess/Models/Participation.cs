namespace DataAccess.Models;

public class Participation
{
    public DateTime GrantedAt { get; set; }
    
    public ParticipationRole Role { get; set; }

    public Guid UserId { get; set; }

    public virtual AppUser User { get; set; }

    public Guid ProjectId { get; set; }

    public virtual Project Project { get; set; }
}