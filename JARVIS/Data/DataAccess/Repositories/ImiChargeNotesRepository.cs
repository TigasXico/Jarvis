﻿
using Jarvis.Data.Contract.Repositories;
using Jarvis.Data.DataModels;
using Jarvis.DataAccess.Database;
using Jarvis.DataAccess.Repositories;

namespace Jarvis.Data.DataAccess.Repositories
{
    internal class ImiChargeNotesRepository : Repository<ImiChargeNotesDataModel>, IImiChargeNotesRepository
    {
        public ImiChargeNotesRepository( JarvisContext context ) : base( context )
        {

        }
    }
}
