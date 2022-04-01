using Jarvis.Data.Contract.Repositories;
using Jarvis.Data.DataModels;
using Jarvis.DataAccess.Database;

namespace Jarvis.DataAccess.Repositories
{
    internal class TransactionRepository : Repository<TransactionDataModel> , ITransactionRepository
    {
        public TransactionRepository( JarvisContext context ) : base( context )
        {

        }
    }
}