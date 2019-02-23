using System.Collections.Generic;
using System.Data.Entity;
using Koowoo.Domain;
using System.Data.Entity.Infrastructure;
using System;

namespace Koowoo.Data
{
    /// <summary>
    /// 数据上下文接口
    /// </summary>
    public interface IDbContext: IDisposable
    {
        IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity;


        /// <summary>
        /// 得到跟踪实体的当前跟踪状态
        /// </summary>
        /// <returns></returns>
        IEnumerable<DbEntityEntry> CurrentEntries();

        int SaveChanges();

        /// <summary>
        /// 执行存储过程，并返回对象列表
        /// </summary>
        /// <typeparam name="TEntity">The type of the T entity.</typeparam>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>IList{``0}.</returns>
        //IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters)
        //    where TEntity : BaseEntity, new();
        /// <summary>
        /// 查询Sql语句
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters);

        /// <summary>
        /// 执行sql 是否启用事务
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="doNotEnsureTransaction"></param>
        /// <param name="timeout"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null,
            params object[] parameters);
    }
}
