using Microsoft.EntityFrameworkCore;

namespace DataAccess.RepositoryNew;

public class RepositoryBase<T> : IRepository<T>
    where T : class
{
    private readonly ProjectsContext _context;

    public RepositoryBase(ProjectsContext context)
    {
        _context = context;
    }

    private DbSet<T> GetSet()
    {
        return _context.Set<T>();
    }


    public IQueryable<T> GetAll()
    {
        return GetSet();
    }

    public async Task Add(IEnumerable<T> entities, CancellationToken cancellationToken)
    {
        var dbSet = GetSet();

        dbSet.AddRange(entities);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task Add(T entity, CancellationToken cancellationToken)
    {
        var dbSet = GetSet();

        dbSet.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task Update(IEnumerable<T> entities, CancellationToken cancellationToken)
    {
        var dbSet = GetSet();

        dbSet.UpdateRange(entities);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task Update(T entity, CancellationToken cancellationToken)
    {
        var dbSet = GetSet();

        dbSet.Update(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task Delete(IEnumerable<T> entities, CancellationToken cancellationToken)
    {
        var dbSet = GetSet();

        dbSet.RemoveRange(entities);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task Delete(T entity, CancellationToken cancellationToken)
    {
        var dbSet = GetSet();

        dbSet.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChanges(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}