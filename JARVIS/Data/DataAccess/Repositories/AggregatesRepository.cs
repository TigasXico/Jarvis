
using System;
using System.Linq;

using Jarvis.Data.Contract.Repositories;
using Jarvis.Data.DataAccess.Database;
using Jarvis.Data.DataModels;

namespace Jarvis.Data.DataAccess.Repositories
{
    internal class AggregatesRepository : Repository<AggregateDataModel>, IAggregatesRepository
    {
        public AggregatesRepository( JarvisContext context ) : base(context)
        {

        }

        public AggregateDataModel GetByName( string aggregateName )
        {
            return dbSet.Where( a => a.Name.Equals( aggregateName , StringComparison.InvariantCultureIgnoreCase ) ).SingleOrDefault();
        }
    }
}
