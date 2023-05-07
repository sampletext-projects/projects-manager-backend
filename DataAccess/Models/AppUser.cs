namespace DataAccess.Models;

public class AppUser
{
    public Guid Id { get; set; }

    public string Username { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public virtual ICollection<Participation> Participations { get; set; }
    public virtual ICollection<Project> Projects { get; set; }
}