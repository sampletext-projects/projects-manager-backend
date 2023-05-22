using DataAccess.EntityConfigurations;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public interface IDbExtensions
{
    Task<bool> CanEditProject(Guid userId, Guid projectId, CancellationToken cancellationToken);
    Task<bool> CanViewProject(Guid userId, Guid projectId, CancellationToken cancellationToken);
    Task<bool> CanEditTask(Guid userId, Guid taskId, CancellationToken cancellationToken);
    Task<bool> CanViewTask(Guid userId, Guid taskId, CancellationToken cancellationToken);
}

public class DbExtensions : IDbExtensions
{
    private readonly ProjectsContext _context;

    public DbExtensions(ProjectsContext context)
    {
        _context = context;
    }

    public async Task<bool> CanEditProject(Guid userId, Guid projectId, CancellationToken cancellationToken)
    {
        return await _context.Set<Participation>()
            .Where(x => x.ProjectId == projectId)
            .Where(x => x.Project.Participations != null && x.Project.Participations.Any(y => y.UserId == userId && (y.Role == ParticipationRole.Creator || y.Role == ParticipationRole.Admin)))
            .AnyAsync(cancellationToken: cancellationToken);
    }

    public async Task<bool> CanEditTask(Guid userId, Guid taskId, CancellationToken cancellationToken)
    {
        return await _context.Set<ProjectTask>()
            .Where(x => x.Id == taskId)
            .Where(x => x.Project.Participations != null && x.Project.Participations.Any(y => y.UserId == userId && (y.Role == ParticipationRole.Admin || y.Role == ParticipationRole.Creator)))
            .AnyAsync(cancellationToken: cancellationToken);
    }

    public async Task<bool> CanViewProject(Guid userId, Guid projectId, CancellationToken cancellationToken)
    {
        return await _context.Set<Participation>()
            .AnyAsync(x => x.UserId == userId && x.ProjectId == projectId, cancellationToken: cancellationToken);
    }

    public async Task<bool> CanViewTask(Guid userId, Guid taskId, CancellationToken cancellationToken)
    {
        return await _context.Set<Participation>()
            .Where(x => x.Project.Tasks != null && x.Project.Tasks.Any(y => y.Id == taskId))
            .AnyAsync(x => x.UserId == userId, cancellationToken: cancellationToken);
    }
}