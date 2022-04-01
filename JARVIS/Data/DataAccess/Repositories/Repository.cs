using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Jarvis.DataAcess.Contract;
using Jarvis.Services;

namespace Jarvis.DataAccess.Repositories
{
    internal class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        internal DbContext context;
        internal DbSet<TEntity> dbSet;


        public Repository( DbContext context )
        {
            this.context = context;
            dbSet = context.Set<TEntity>();
        }

        #region Get item from set

        public IEnumerable<TEntity> Get( Expression<Func<TEntity , bool>> where = null , Func<IQueryable<TEntity> , IOrderedQueryable<TEntity>> orderBy = null )
        {
            try
            {
                IQueryable<TEntity> query = dbSet;

                if ( where != null )
                {
                    query = query.Where( where );
                }


                if ( orderBy != null )
                {
                    return orderBy( query ).ToList();
                }
                else
                {
                    return query.ToList();
                }

            }
            catch ( Exception ex )
            {
                WindowService.ShowException( ex );
                return null;
            }

        }

        public IEnumerable<TEntity> GetAll( Func<IQueryable<TEntity> , IOrderedQueryable<TEntity>> orderBy = null )
        {
            return Get();
        }

        public TEntity GetByID( object id )
        {
            return dbSet.Find( id );
        }

        #endregion

        #region Add entity to set

        public void Add( TEntity entity )
        {
            dbSet.Add( entity );
        }

        #endregion

        #region Update the entity on the set

        public virtual void Update( TEntity entityToUpdate )
        {
            dbSet.Attach( entityToUpdate );
            context.Entry( entityToUpdate ).State = EntityState.Modified;
        }

        #endregion

        #region Remove the entity from the set

        public void Remove( object id )
        {
            TEntity entityToDelete = dbSet.Find( id );
            Remove( entityToDelete );
        }

        public void RemoveEntity( TEntity entity )
        {
            if ( context.Entry( entity ).State == EntityState.Detached )
            {
                dbSet.Attach( entity );
            }
            dbSet.Remove( entity );
        }

        public void RemoveEntities( IEnumerable<TEntity> entities )
        {
            foreach ( TEntity entity in entities )
            {
                if ( context.Entry( entity ).State == EntityState.Detached )
                {
                    dbSet.Attach( entity );
                }
            }
            
            dbSet.RemoveRange( entities );
        }

        #endregion
    }
}
