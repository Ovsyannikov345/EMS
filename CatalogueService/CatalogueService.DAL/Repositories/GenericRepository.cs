﻿using CatalogueService.DAL.Data;
using CatalogueService.DAL.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CatalogueService.DAL.Repositories
{
    public class GenericRepository<TEntity>(EstateDbContext context) : IGenericRepository<TEntity> where TEntity : class, new()
    {
        public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await context.Set<TEntity>().FindAsync([id], cancellationToken: cancellationToken);
        }

        public async Task<TEntity?> GetByFilterAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await context.Set<TEntity>().FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await context.Set<TEntity>().AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await context.Set<TEntity>().AddAsync(entity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public async Task<TEntity?> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await context.Set<TEntity>().FindAsync([id], cancellationToken);

            if (entity is not null)
            {
                context.Set<TEntity>().Remove(entity);
                await context.SaveChangesAsync(cancellationToken);
            }

            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            context.Update(entity);
            await context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
