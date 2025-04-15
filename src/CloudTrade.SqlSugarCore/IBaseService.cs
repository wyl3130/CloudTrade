using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.SqlSugarCore
{
    public interface IBaseService
    {
        bool Insert<T>(T t) where T : class, new();
        bool Insert<T>(IEnumerable<T> tList) where T : class, new();
        bool Delete<T>(T t) where T : class, new();
        bool Delete<T>(IEnumerable<T> tList) where T : class, new();
        bool Delete<T>(Guid Id) where T : class, new();
        bool Update<T>(T t) where T : class, new();
        bool Update<T>(IEnumerable<T> tList) where T : class, new();
        T Find<T>(Guid Id) where T : class;
        IEnumerable<T> Queryable<T>(Expression<Func<T, bool>> fun) where T : class;
        IEnumerable<T> Queryable<T>() where T : class;



        Task<bool> InsertAsync<T>(T t) where T : class, new();
        Task<bool> InsertAsync<T>(IEnumerable<T> tList) where T : class, new();
        Task<bool> DeleteAsync<T>(T t) where T : class, new();
        Task<bool> DeleteAsync<T>(IEnumerable<T> tList) where T : class, new();
        Task<bool> DeleteAsync<T>(Guid Id) where T : class, new();
        Task<bool> UpdateAsync<T>(T t) where T : class, new();
        Task<bool> UpdateAsync<T>(IEnumerable<T> tList) where T : class, new();
        Task<T> FindAsync<T>(Guid Id) where T : class;
        Task<IEnumerable<T>> QueryableAsync<T>(Expression<Func<T, bool>> fun) where T : class;
        Task<IEnumerable<T>> QueryableAsync<T>() where T : class;



        //Task<IEnumerable<T>> GetAllIncludingAsync<T>(params Expression<Func<T, object>>[] includeProperties) where T : class;
    }
}
