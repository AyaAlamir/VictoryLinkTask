using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

namespace VictoryLinkTask.Repository.Interfaces.Common
{
    public interface IRepository<T> where T : class
    {

        #region add
        void CreateAsyn(T entity);
        void CreateListAsyn(List<T> entityList);
        #endregion

        #region update
        void Update(T entity);
        void UpdateList(List<T> entityList);
        #endregion

        #region Get
        Task<List<T>> GetAllAsync();
        Task<bool> GetAnyAsync(Expression<Func<T, bool>> filter = null);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter, string includeProperties = "");
        Task<List<T>> GetWhereAsync(Expression<Func<T, bool>> filter = null);
        Task<List<T>> GetWhereAsync(Expression<Func<T, bool>> filter = null, string includeProperties = ""); 
        #endregion
    }
}