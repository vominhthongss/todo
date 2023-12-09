using Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Template
{
    public abstract class TRepository<TEntity, TContext> : IRepository<TEntity>
        where TEntity : class, IEntity
        where TContext : DbContext
    {
        private readonly TContext context;
        public TRepository(TContext context)
        {
            this.context = context;
        }
        public virtual async Task<TEntity?> Add(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
            var n = await context.SaveChangesAsync();
            return (n != 0) ? entity : null;
        }

        public virtual async Task<TEntity?> Delete(string id)
        {
            var entity = await context.Set<TEntity>().FindAsync(id);
            if (entity == null)
            {
                return entity;
            }

            context.Set<TEntity>().Remove(entity);
            await context.SaveChangesAsync();

            return entity;
        }

        public IQueryable<TEntity> Get(string id)
        {
            return context.Set<TEntity>().Where(e => e.Id == id);
        }

        public IQueryable<TEntity> GetAll()
        {
            return context.Set<TEntity>();
        }

        public virtual async Task<int> Update(TEntity entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            var n = await context.SaveChangesAsync();
            return n;
        }

        // return found entity
        public virtual async Task<TEntity?> Update(TEntity input, string id)
        {
            var found = await context.Set<TEntity>().FindAsync(id);
            if (found != null)
            {
                var properties = typeof(TEntity).GetProperties();
                foreach (var property in properties)
                {
                    var inputValue = property.GetValue(input);
                    var foundValue = property.GetValue(found);
                    property.SetValue(found, inputValue ?? foundValue);
                }

                context.Entry(found).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }

            return found;
        }

        public virtual async Task<TEntity?> FindAsync(string id) => await context.Set<TEntity>().FindAsync(id);
        public virtual async Task<TEntity> Delete(TEntity entity)
        {
            context.Set<TEntity>().Remove(entity);
            await context.SaveChangesAsync();
            return entity;

        }
    }
}
