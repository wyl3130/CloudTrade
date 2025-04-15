using SqlSugar;
using System.Linq.Expressions;

namespace CloudTrade.SqlSugarCore
{
    public class BaseService : IBaseService
    {
        private readonly ISqlSugarClient db;
        public BaseService(ISqlSugarClient db)
        {
            this.db = db;
        }
        public bool Insert<T>(T t) where T : class, new()
        {
            try
            {
                if (t == null) throw new Exception("T is Null");
                return db.Insertable<T>(t).ExecuteCommand() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool Insert<T>(IEnumerable<T> tList) where T : class, new()
        {
            try
            {
                if (tList == null) throw new Exception("tList Is Null");
                return db.Insertable<T>(tList).ExecuteCommand() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool Delete<T>(T t) where T : class, new()
        {
            try
            {
                if (t == null)
                {
                    throw new Exception("T Is Null");
                }
                return db.Deleteable<T>(t).ExecuteCommandHasChange();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete<T>(Guid Id) where T : class, new()
        {
            try
            {
                T t = this.Find<T>(Id);
                if (t == null)
                {
                    throw new Exception("T is Null");
                }
                return db.Deleteable<T>(t).ExecuteCommandHasChange();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool Delete<T>(IEnumerable<T> tList) where T : class, new()
        {
            try
            {
                if (tList == null || !tList.Any())
                {
                    throw new Exception("T List Is Null or Empty");
                }
                return db.Deleteable<T>(tList).ExecuteCommandHasChange();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update<T>(T t) where T : class, new()
        {
            try
            {
                if (t == null)
                {
                    throw new Exception("T Is Null");
                }
                return db.Updateable<T>(t).ExecuteCommandHasChange();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update<T>(IEnumerable<T> tList) where T : class, new()
        {
            try
            {
                return db.Updateable<T>(tList).ExecuteCommandHasChange();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public T Find<T>(Guid Id) where T : class
        {
            try
            {
                return db.Queryable<T>().InSingle(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<T> Queryable<T>() where T : class
        {
            try
            {
                return db.Queryable<T>().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<T> Queryable<T>(Expression<Func<T, bool>> fun) where T : class
        {
            try
            {
                return db.Queryable<T>().Where(fun).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public Task<IEnumerable<T>> GetAllIncludingAsync<T>(params Expression<Func<T, object>>[] includeProperties) where T : class
        //{
        //    throw new NotImplementedException();
        //}


        //public async Task<T> existData<T>(string name) where T : class
        //{

        //}



        public async Task<bool> InsertAsync<T>(T t) where T : class, new()
        {
            try
            {
                if (t == null) throw new Exception("T is Null");
                return await db.Insertable<T>(t).ExecuteCommandAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> InsertAsync<T>(IEnumerable<T> tList) where T : class, new()
        {
            try
            {
                if (tList == null) throw new Exception("tList Is Null");
                return await db.Insertable<T>(tList.ToList()).ExecuteCommandAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<bool> DeleteAsync<T>(T t) where T : class, new()
        {
            try
            {
                if (t == null)
                {
                    throw new Exception("T Is Null");
                }
                return await db.Deleteable<T>(t).ExecuteCommandHasChangeAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteAsync<T>(Guid Id) where T : class, new()
        {
            try
            {
                T t = await this.FindAsync<T>(Id);
                if (t == null)
                {
                    throw new Exception("T is Null");
                }
                return await db.Deleteable<T>(t).ExecuteCommandHasChangeAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> DeleteAsync<T>(IEnumerable<T> tList) where T : class, new()
        {
            try
            {
                if (tList == null || !tList.Any())
                {
                    throw new Exception("T List Is Null or Empty");
                }
                return await db.Deleteable<T>(tList).ExecuteCommandHasChangeAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateAsync<T>(T t) where T : class, new()
        {
            try
            {
                if (t == null)
                {
                    throw new Exception("T Is Null");
                }
                return await db.Updateable<T>(t).ExecuteCommandHasChangeAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateAsync<T>(IEnumerable<T> tList) where T : class, new()
        {
            try
            {
                return await db.Updateable<T>(tList).ExecuteCommandHasChangeAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public async Task<T> FindAsync<T>(Guid Id) where T : class
        {
            try
            {
                return await db.Queryable<T>().InSingleAsync(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<T>> QueryableAsync<T>() where T : class
        {
            try
            {
                return await db.Queryable<T>().ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<T>> QueryableAsync<T>(Expression<Func<T, bool>> fun) where T : class
        {
            try
            {
                return await db.Queryable<T>().Where(fun).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }

}
