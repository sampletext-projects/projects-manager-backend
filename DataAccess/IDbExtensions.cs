using DataAccess.EntityConfigurations;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public interface IDbExtensions
{
    Task<bool> CanEdit(Guid userId, Guid projectId, CancellationToken cancellationToken);
    Task<bool> CanView(Guid userId, Guid projectId, CancellationToken cancellationToken);
}

public class DbExtensions : IDbExtensions
{
    private readonly ProjectsContext _context;

    public DbExtensions(ProjectsContext context)
    {
        _context = context;
    }

    public async Task<bool> CanEdit(Guid userId, Guid projectId, CancellationToken cancellationToken)
    {
        return await _context.Set<Participation>()
            .AnyAsync(x => x.UserId == userId && x.ProjectId == projectId && x.Role == ParticipationRole.Admin || x.Role == ParticipationRole.Creator, cancellationToken: cancellationToken);
    }

    public async Task<bool> CanView(Guid userId, Guid projectId, CancellationToken cancellationToken)
    {
        return await _context.Set<Participation>()
            .AnyAsync(x => x.UserId == userId && x.ProjectId == projectId, cancellationToken: cancellationToken);
    }
}