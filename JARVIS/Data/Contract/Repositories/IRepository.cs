using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Jarvis.Data.Contract.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        #region Get item from set

        IEnumerable<TEntity> Get( Expression<Func<TEntity , bool>> where = null , Func<IQueryable<TEntity> , IOrderedQueryable<TEntity>> orderBy = null );

        IEnumerable<TEntity> GetAll( Func<IQueryable<TEntity> , IOrderedQueryable<TEntity>> orderBy = null );

        TEntity GetByID( object id );

        #endregion

        #region Add entity to set

        void Add( TEntity entity );

        #endregion

        #region Update the entity on the set

        void Update( TEntity entityToUpdate );

        #endregion

        #region Remove the entity from the set

        void Remove( object id );

        void RemoveEntity( TEntity entity );

        void RemoveEntities( IEnumerable<TEntity> entity );

        #endregion
    }
}
