using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Jarvis.DataAcess.Contract;

namespace Jarvis.DataAccess.Repositories
{
    internal class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected DbContext Context;

        public Repository( DbContext context )
        {
            Context = context;
        }

        public virtual TEntity Get( int id )
        {
            return Context.Set<TEntity>().Find( id );
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return Context.Set<TEntity>().ToList();
        }

        public virtual IEnumerable<TEntity> Find( Expression<Func<TEntity , bool>> predicate )
        {
            return Context.Set<TEntity>().Where( predicate ).ToList();
        }

        public virtual void Add( TEntity entity )
        {
            Context.Set<TEntity>().Add( entity );
        }

        public virtual void AddRange( IEnumerable<TEntity> entities )
        {
            Context.Set<TEntity>().AddRange( entities );
        }

        public virtual void Remove( TEntity entity )
        {
            Context.Set<TEntity>().Remove( entity );
        }

        public virtual void RemoveRange( IEnumerable<TEntity> entities )
        {
            Context.Set<TEntity>().RemoveRange( entities );
        }
    }
}
