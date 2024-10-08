﻿
using System;
using System.Linq;

using Jarvis.Data.Contract.Repositories;
using Jarvis.Data.DataModels;
using Jarvis.DataAccess.Database;
using Jarvis.DataAccess.Repositories;

namespace Jarvis.Data.DataAccess.Repositories
{
    internal class CustomerGroupRepository : Repository<CustomerGroupDataModel>, ICustomerGroupRepository
    {
        public JarvisContext CurrentContext => context as JarvisContext;

        public CustomerGroupRepository( JarvisContext context ) : base( context )
        {
        }

        public CustomerGroupDataModel GetByName( string customerGroupName )
        {
            return Get( cg => cg.Name.Equals( customerGroupName , StringComparison.InvariantCultureIgnoreCase ) ).FirstOrDefault();
        }
    }
}
