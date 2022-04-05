using Jarvis.Data.Contract.Repositories;
using Jarvis.Data.DataAccess.Database;
using Jarvis.Data.DataModels;

namespace Jarvis.Data.DataAccess.Repositories
{
    internal class TransactionRepository : Repository<TransactionDataModel> , ITransactionRepository
    {
        public TransactionRepository( JarvisContext context ) : base( context )
        {

        }
    }
}