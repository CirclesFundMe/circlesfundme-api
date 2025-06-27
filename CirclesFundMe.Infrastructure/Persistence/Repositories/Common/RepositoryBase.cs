namespace CirclesFundMe.Infrastructure.Persistence.Repositories.Common
{
    public class RepositoryBase<TEntity>(DbSet<TEntity> entities) : IRepositoryBase<TEntity> where TEntity : BaseEntity
    {
        protected readonly DbSet<TEntity> _entities = entities;

        public async Task<EntityEntry<TEntity>> AddAsync(TEntity entity, CancellationToken cancellationToken) => await _entities.AddAsync(entity, cancellationToken);
        public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellation) => await _entities.AddRangeAsync(entities, cancellation);
        public EntityEntry<TEntity> Update(TEntity entity) => _entities.Update(entity);
        public void Delete(TEntity entity) => _entities.Remove(entity);
        public async Task<TEntity?> GetByPrimaryKey(object primaryKey, CancellationToken cancellationToken)
        {
            TEntity? entity = await _entities.FindAsync([primaryKey], cancellationToken);

            if (entity != null && !entity.IsDeleted)
            {
                _entities.Entry(entity).State = EntityState.Detached;
                return entity;
            }

            return null;
        }
        public async Task<TEntity?> GetByPrimaryKey(object primaryKey, CancellationToken cancellationToken, params Expression<Func<TEntity, object>>[] includes)
        {
            TEntity? entity = await _entities.FindAsync([primaryKey], cancellationToken);

            if (entity != null && !entity.IsDeleted)
            {
                foreach (Expression<Func<TEntity, object>> include in includes)
                {
                    if (include.Body is MemberExpression includePath)
                    {
                        NavigationEntry navigation = _entities.Entry(entity).Navigation(includePath.Member.Name);
                        await navigation.LoadAsync(cancellationToken);
                    }
                }

                _entities.Entry(entity).State = EntityState.Detached;
                return entity;
            }

            return null;
        }
        public async Task<TEntity?> GetOneAsync(Expression<Func<TEntity, bool>>[]? predicates = null, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _entities.AsNoTracking().Where(e => e.IsDeleted == false);

            if (predicates != null && predicates.Length > 0)
            {
                foreach (Expression<Func<TEntity, bool>> predicate in predicates)
                {
                    query = query.Where(predicate);
                }
            }

            if (includes != null && includes.Length > 0)
            {
                foreach (Expression<Func<TEntity, object>> include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<List<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>>[]? predicates = null, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _entities.AsNoTracking().Where(e => e.IsDeleted == false);

            if (predicates != null && predicates.Length > 0)
            {
                foreach (Expression<Func<TEntity, bool>> predicate in predicates)
                {
                    query = query.Where(predicate);
                }
            }

            if (includes != null && includes.Length > 0)
            {
                foreach (Expression<Func<TEntity, object>> include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.ToListAsync(cancellationToken);
        }
        public async Task<TEntity?> GetByPrimaryKeys(object[] primaryKeys, CancellationToken cancellationToken) => await _entities.FindAsync(primaryKeys, cancellationToken);
        public async Task<TEntity?> GetByPrimaryKeys(object[] primaryKeys, string[] primaryKeyNames, CancellationToken cancellationToken, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _entities.AsQueryable();

            foreach (Expression<Func<TEntity, object>> include in includes)
            {
                query = query.Include(include);
            }

            ParameterExpression parameter = Expression.Parameter(typeof(TEntity), "e");
            BinaryExpression body = primaryKeys
                .Select((key, index) => Expression.Equal(
                    Expression.Property(parameter, primaryKeyNames[index]),
                    Expression.Constant(key)
                ))
                .Aggregate(Expression.AndAlso);

            Expression<Func<TEntity, bool>> lambda = Expression.Lambda<Func<TEntity, bool>>(body, parameter);

            return await query.FirstOrDefaultAsync(lambda, cancellationToken);
        }
        public async Task<int> CountAsync(Expression<Func<TEntity, bool>>[] predicates, CancellationToken cancellationToken)
        {
            IQueryable<TEntity> query = _entities.AsNoTracking().Where(x => x.IsDeleted == false);

            foreach (Expression<Func<TEntity, bool>> predicate in predicates)
            {
                query = query.Where(predicate);
            }

            return await query.CountAsync(cancellationToken);
        }
        public async Task<bool> ExistAsync(Expression<Func<TEntity, bool>>[] predicates, CancellationToken cancellationToken)
        {
            IQueryable<TEntity> query = _entities.AsNoTracking().Where(x => x.IsDeleted == false);

            foreach (Expression<Func<TEntity, bool>> predicate in predicates)
            {
                query = query.Where(predicate);
            }

            return await query.AnyAsync(cancellationToken);
        }
    }
}
