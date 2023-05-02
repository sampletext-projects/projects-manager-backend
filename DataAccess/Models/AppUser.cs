namespace DataAccess.Models;

public class AppUser
{
    public Guid Id { get; set; }

    public string Username { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public virtual ICollection<Project> CreatedProjects { get; set; }
}