﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Super.Core.Infrastruct.EF
{    
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        IQueryable<TEntity> Table { get; }

        #region 查询

        TEntity Get(params object[] keys);
        TEntity Get(Expression<Func<TEntity, bool>> fliter);
        bool TryGet(Expression<Func<TEntity, bool>> fliter, out TEntity entity);
        IQueryable<TEntity> GetAllIncluding<TProperty>(Expression<Func<TEntity, TProperty>> propertySelectors);
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetList(Expression<Func<TEntity, bool>> filter);
        IQueryable<TEntity> GetList(string sql, params object[] parameters);
        IQueryable<TEntity> GetPaging<TOrder>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TOrder>> orderFiled, int pageSize, int pageNum, out int count, bool isAsc = true);

        List<TReturn> GetList<TReturn>(string sql, params object[] parameters)
            where TReturn : new();

        #endregion

        #region 增加

        SaveResult Insert(TEntity entity, bool isSaveChange = true);

        SaveResult InsertMany(IEnumerable<TEntity> lst, bool isSaveChange = true);

        #endregion

        #region 编辑       

        SaveResult Update(TEntity entity, bool isSaveChange = true);

        SaveResult UpdateProperty(Expression<Func<TEntity, bool>> filter, Action<TEntity> change, bool isSaveChange = true);

        #endregion

        #region 删除

        SaveResult Delete(TEntity Entity, bool isSaveChange = true);

        SaveResult Delete(Expression<Func<TEntity, bool>> filter, bool isSaveChange = true);

        #endregion

        #region 其它

        SaveResult ExcuteSqlCommand(string sql, object[] parameters);

        bool IsExist(Expression<Func<TEntity, bool>> filter);

        int Count(Expression<Func<TEntity, bool>> filter);

        #endregion
    }

    //public interface IRepository<TEntity, TPrimaryKey> : IRepository<TEntity> where TEntity : class, IEntity<TPrimaryKey>
    //{
    //    TEntity Get(TPrimaryKey key);
    //    SaveResult Update(TPrimaryKey key, Action<TEntity> change, bool isSaveChange = true);
    //    SaveResult Delete(TPrimaryKey key, bool isSaveChange = true);
    //}    
}
