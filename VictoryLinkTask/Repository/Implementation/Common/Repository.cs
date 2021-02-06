using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using VictoryLinkTask.Core;
using VictoryLinkTask.Repository.Interfaces.Common;

namespace VictoryLinkTask.Repository.Implementation.Common
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly Promotions_Entities AppDbContext;

        public Repository(Promotions_Entities appDbContext)
        {
            AppDbContext = appDbContext;
        }

        #region Get Methods
        public async Task<List<T>> GetAllAsync()
        {
            return await AppDbContext.Set<T>().AsNoTracking().ToListAsync();
        }
        public async Task<bool> GetAnyAsync(Expression<Func<T, bool>> filter = null)
        {
            return await AppDbContext.Set<T>().AnyAsync(filter);
        }
        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = AppDbContext.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter, string includeProperties = "")
        {
            IQueryable<T> query = AppDbContext.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return await query.FirstOrDefaultAsync();
        }
        public async Task<List<T>> GetWhereAsync(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = AppDbContext.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task<List<T>> GetWhereAsync(Expression<Func<T, bool>> filter = null, string includeProperties = "")
        {
            IQueryable<T> query = AppDbContext.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return await query.ToListAsync();
        }
        #endregion

        #region Create 
        public void CreateAsyn(T entity)
        {
            AppDbContext.Set<T>().Add(entity);
        }
        public void CreateListAsyn(List<T> entityList)
        {
            AppDbContext.Set<T>().AddRange(entityList);
        }
        #endregion

        #region Update
        public void Update(T entity)
        {
            AppDbContext.Entry(entity).State = EntityState.Modified;
        }
        public void UpdateList(List<T> entityList)
        {
            AppDbContext.Entry(entityList).State = EntityState.Modified;
        }
        #endregion
    }
}