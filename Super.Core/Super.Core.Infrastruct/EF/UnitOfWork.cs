using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Super.Core.Infrastruct.EF
{    
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        protected readonly DbContext _dbContext;
        private readonly Dictionary<string, Object> _dicRepository = new Dictionary<string, object>();

        public UnitOfWork(DbContext dbContext)
        {
            this._dbContext = dbContext;
        }        

        public virtual TRepository GetRepository<TRepository, TEntity>()
            where TEntity : class, IEntity
            where TRepository : class, IRepository<TEntity>
        {
            string typeName = typeof(TEntity).Name;
            object repository;
            if (!this._dicRepository.TryGetValue(typeName, out repository))
            {                
                this._dicRepository[typeName] = new Repository<TEntity>(this._dbContext);
            }
            return (TRepository)this._dicRepository[typeName];
        }


        public SaveResult SaveChanges()
        {
            SaveResult r;
            try
            {
                int count = _dbContext.SaveChanges();
                if (count >= 1)
                {
                    r = new SaveResult { Status = SaveStatus.Success, Rows = count };
                }
                else
                {
                    r = new SaveResult { Status = SaveStatus.NoImpact, Rows = count };
                }
            }
            catch (Exception exp)
            {
                r = new SaveResult { Status = SaveStatus.Error, Message = exp.Message };
            }
            return r;
        }

        #region Dispose模式释放资源

        //表示对象是否已被清除
        private bool disposed = false;

        //参数表示该方法是由IDisposable.Dispose()方法调用的，还是由析构函数调用的
        //非托管资源由IDisposable.Dispose()方法处理，析构函数不作处理
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed) return;

            if (disposing)
            {//释放托管资源

            }
            //释放非托管资源
            this._dbContext.Dispose();
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            //该方法告诉垃圾回收器该对象已经不再需要调用其析构函数了
            GC.SuppressFinalize(this);
        }

        //析构函数
        ~UnitOfWork()
        {
            Dispose(false);
        }

        #endregion
    }
}
