using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;
using WebApi.Data.Context;
using WebApi.Data.Interfaces;

namespace WebApi.Data.Repositories;

public abstract class Repository<TEntity>(ApplicationDbContext context) : IRepository<TEntity> where TEntity : class
{
    private readonly ApplicationDbContext _context = context;
    private readonly DbSet<TEntity> _table = context.Set<TEntity>();
    private IDbContextTransaction _transaction = null!;

    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        if (entity == null) return null!; ;

        try
        {
            await _table.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error creating {nameof(TEntity)} entity :: {ex.Message}");
            return null!;
        }
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(bool orderByDecending = false, Expression<Func<TEntity, object>>? sortBy = null, Expression<Func<TEntity, bool>>? filterBy = null, params Expression<Func<TEntity, object>>[] joins)
    {
        IQueryable<TEntity> query = _table;
        if (filterBy != null)
            query = query.Where(filterBy);

        if (joins != null && joins.Length != 0)
            foreach (var join in joins)
                query = query.Include(join);

        if (sortBy != null)
            query = orderByDecending
                ? query.OrderByDescending(sortBy)
                : query.OrderBy(sortBy);

        var entityList = await query.ToListAsync();
        return entityList;

    }
    public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> findBy, params Expression<Func<TEntity, object>>[] joins)
    {
        if (findBy == null) return null!;

        IQueryable<TEntity> query = _table;

        if (joins != null && joins.Length != 0)
            foreach (var join in joins)
                query = query.Include(join);

        var entity = await query.FirstOrDefaultAsync(findBy);

        return entity ?? null!;
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity)
    {
        if (entity is null) return null!;
        try
        {
            _table.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error Updating {nameof(TEntity)} entity: {ex.Message}");
            return null!;
        }
    }

    public virtual async Task<bool> DeleteAsync(TEntity entity)
    {
        if (entity == null) return false;
        try
        {

            _table.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error Deleting {nameof(TEntity)} entity :: {ex.Message}");
            return false;
        }
    }

    // Transactions
    public virtual async Task BeginTransactionAsync()
    {
        _transaction ??= await _context.Database.BeginTransactionAsync();
    }
    public virtual async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null!;
        }
    }

    public virtual async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null!;
        }
    }
}
