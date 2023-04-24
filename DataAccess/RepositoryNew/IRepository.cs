namespace DataAccess.RepositoryNew;

public interface IRepository<T>
    where T : class
{
    IQueryable<T> GetAll();

    Task Add(IEnumerable<T> entities, CancellationToken cancellationToken);

    Task Add(T entity, CancellationToken cancellationToken);

    Task Update(IEnumerable<T> entities, CancellationToken cancellationToken);

    Task Update(T entity, CancellationToken cancellationToken);

    Task Delete(IEnumerable<T> entities, CancellationToken cancellationToken);

    Task Delete(T entity, CancellationToken cancellationToken);
    Task SaveChanges(CancellationToken cancellationToken);
}