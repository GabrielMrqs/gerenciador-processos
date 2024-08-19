using System.Linq.Expressions;

namespace GerenciadorProcessos.Domain.Repositorios
{
    public interface IBaseRepository<T> where T : class
    {
        abstract Task<T?> GetByIdAsync(Guid id);
        abstract Task<IEnumerable<T>> GetAllAsync();
        abstract Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        abstract Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        abstract Task AddAsync(T entity);
        abstract void Attach(T entity);
        abstract Task SaveChangesAsync();
        abstract void Update(T entity);
        abstract void Remove(T entity);
    }
}
