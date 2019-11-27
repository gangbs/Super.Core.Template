﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Super.Core.Infrastruct.EF
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        protected readonly DbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(DbContext context)
        {
            this._context = context;
            this._dbSet = _context.Set<TEntity>();
        }

        public IQueryable<TEntity> Table
        {
            get
            {
                return this._dbSet.AsNoTracking();
            }
        }

        #region 查询

        public TEntity Get(params object[] keys)
        {
            TEntity obj = _dbSet.Find(keys);

            if (obj != null)
            {
                _context.Entry<TEntity>(obj).State = EntityState.Detached;
            }

            return obj;
        }
        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            return this._dbSet.Where(filter).AsNoTracking().FirstOrDefault();
        }
        public bool TryGet(Expression<Func<TEntity, bool>> filter, out TEntity entity)
        {
            entity = this.Get(filter);
            return entity != null ? true : false;
        }
        public IQueryable<TEntity> GetAllIncluding<TProperty>(Expression<Func<TEntity, TProperty>> propertySelectors)
        {
            return this._dbSet.Include<TEntity, TProperty>(propertySelectors);
        }
        public IQueryable<TEntity> GetAll()
        {
            return this._dbSet.AsNoTracking();
        }
        public IQueryable<TEntity> GetList(Expression<Func<TEntity, bool>> filter)
        {
            return this._dbSet.Where(filter).AsNoTracking();
        }
        public IQueryable<TEntity> GetList(string sql, params object[] parameters)
        {
            return this._dbSet.FromSql(sql, parameters).AsNoTracking();
        }
        public IQueryable<TEntity> GetPaging<TOrder>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TOrder>> orderFiled, int pageSize, int pageNum, out int count, bool isAsc = true)
        {
            count = _dbSet.Count(filter);
            IQueryable<TEntity> lstReturn;

            if (isAsc)
            {
                lstReturn = _dbSet.Where(filter).OrderBy(orderFiled).Skip(pageSize * (pageNum - 1)).Take(pageSize).AsNoTracking();
            }
            else
            {
                lstReturn = _dbSet.Where(filter).OrderByDescending(orderFiled).Skip(pageSize * (pageNum - 1)).Take(pageSize).AsNoTracking();
            }
            return lstReturn;
        }
        public List<TReturn> GetList<TReturn>(string sql, params object[] parameters) where TReturn : new()
        {
            var cmd = this._context.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = sql;
            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }
            cmd.CommandType = CommandType.Text;
            var reader = cmd.ExecuteReader();

            var lst = new List<TReturn>();
            var pros = typeof(TReturn).GetProperties(System.Reflection.BindingFlags.Public);
            while (reader.Read())
            {
                TReturn entity = new TReturn();
                foreach (var p in pros)
                {
                    p.SetValue(entity, reader[p.Name]);
                }
                lst.Add(entity);
            }
            return lst;
        }

        #endregion

        #region 增加

        public SaveResult Insert(TEntity entity, bool isSaveChange = true)
        {
            ////第一种方法
            //_dbSet.Attach(entity);
            //_context.Entry<TEntity>(entity).State = EntityState.Added;

            //第二种方法
            _dbSet.Add(entity); //EntityState.Detached

            return isSaveChange ? this.Save() : new SaveResult();
        }
        public SaveResult InsertMany(IEnumerable<TEntity> lst, bool isSaveChange = true)
        {
            _dbSet.AddRange(lst);

            return isSaveChange ? this.Save() : new SaveResult();
        }


        #endregion

        #region 编辑

        public SaveResult Update(TEntity entity, bool isSaveChange = true)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;

            return isSaveChange ? this.Save() : new SaveResult();
        }

        public SaveResult UpdateProperty(Expression<Func<TEntity, bool>> filter, Action<TEntity> change, bool isSaveChange = true)
        {
            var lstEntity = this._dbSet.Where(filter);
            if (lstEntity == null || lstEntity.Count() < 1)
                return new SaveResult { Status = SaveStatus.NonExist, Rows = 0, Message = "The entity is not exist!" };

            foreach (var entity in lstEntity)
            {
                change(entity);
            }
            return isSaveChange ? this.Save() : new SaveResult();
        }

        #endregion

        #region 删除

        public SaveResult Delete(TEntity entity, bool isSaveChange = true)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Deleted;
            return isSaveChange ? this.Save() : new SaveResult();
        }
        public SaveResult Delete(Expression<Func<TEntity, bool>> filter, bool isSaveChange = true)
        {
            var lst = this._dbSet.Where(filter);
            if (lst == null || lst.Count() < 1)
                return new SaveResult { Status = SaveStatus.NonExist, Rows = 0, Message = "The entity is not exist!" };
            _dbSet.RemoveRange(lst);
            return isSaveChange ? this.Save() : new SaveResult();
        }

        #endregion

        #region 保存变更

        protected SaveResult Save()
        {
            SaveResult r;
            try
            {
                int count = _context.SaveChanges();
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

        #endregion

        #region 其它

        public SaveResult ExcuteSqlCommand(string sql, object[] parameters)
        {
            SaveResult r;
            try
            {
                int count = this._context.Database.ExecuteSqlCommand(sql, parameters);

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

        public bool IsExist(Expression<Func<TEntity, bool>> filter)
        {
            //return this._dbSet.Any(filter);//mysql中有问题
            return this.Count(filter) > 0;
        }

        public int Count(Expression<Func<TEntity, bool>> filter)
        {
            return this._dbSet.Count(filter);
        }

        #endregion
    }    

    //public class Repository<TEntity, TPrimaryKey> : Repository<TEntity>, IRepository<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    //{
    //    public Repository(DbContext context) : base(context)
    //    {
    //    }

    //    public TEntity Get(TPrimaryKey key)
    //    {
    //        TEntity obj = _dbSet.Find(key);

    //        if (obj != null)
    //        {
    //            _context.Entry<TEntity>(obj).State = EntityState.Detached;
    //        }

    //        return obj;
    //    }
    //    public SaveResult Update(TPrimaryKey key, Action<TEntity> change, bool isSaveChange = true)
    //    {
    //        var entity = _dbSet.Find(key);
    //        if (entity == null) return new SaveResult { Status = SaveStatus.NonExist, Rows = 0, Message = "The entity is not exist!" };
    //        change(entity);
    //        return this.Update(entity, isSaveChange);
    //    }
    //    public SaveResult Delete(TPrimaryKey key, bool isSaveChange = true)
    //    {
    //        TEntity entity = this._dbSet.Find(key);
    //        if (entity == null) return new SaveResult { Status = SaveStatus.NonExist, Rows = 0, Message = "The entity is not exist!" };
    //        return this.Delete(entity, isSaveChange);
    //    }
    //}    
}
