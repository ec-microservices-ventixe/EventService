﻿using System.Linq.Expressions;

namespace WebApi.Data.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{

    public Task<TEntity> CreateAsync(TEntity entity);

    public Task<IEnumerable<TEntity>> GetAllAsync(bool orderByDecending = false, Expression<Func<TEntity, object>>? sortBy = null, Expression<Func<TEntity, bool>>? filterBy = null, params Expression<Func<TEntity, object>>[] joins);

    public Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> findBy, params Expression<Func<TEntity, object>>[] joins);

    public Task<TEntity> UpdateAsync(TEntity entity);

    public Task<bool> DeleteAsync(TEntity entity);

    public Task BeginTransactionAsync();

    public Task CommitTransactionAsync();

    public Task RollbackTransactionAsync();
}
