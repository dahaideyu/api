using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Koowoo.Data.Interface
{
    public interface IRepository<T> where T : Domain.BaseEntity
    {
        Task<T> GetEntityAsync(Expression<Func<T, bool>> where);
        /// <summary>
        /// Gets the by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>`0.</returns>
        T GetById(object id);
        T GetEntity(Expression<Func<T, bool>> where);
        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Insert(T entity);
        /// <summary>
        /// Inserts the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void Insert(IEnumerable<T> entities);
        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Update(T entity);

        void Update(List<T> entitys);
        // bool AddOrUpdate(params T[] entities);

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Delete(T entity);

        void Delete(Expression<Func<T, bool>> where);
        /// <summary>
        /// Gets the table.
        /// </summary>
        /// <value>The table.</value>
        IQueryable<T> Table { get; }
        /// <summary>
        /// Gets the tables no tracking.
        /// </summary>
        /// <value>The tables no tracking.</value>
        IQueryable<T> TableNoTracking { get; }
    }
}