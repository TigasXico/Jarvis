using System;
using System.Linq;

using Jarvis.Data.DataModels;
using Jarvis.Services;

namespace Jarvis.Controllers.ModelControllers
{
    public class TransactionController : BaseDataModelController<TransactionDataModel>
    {
        public TransactionController() : base(new TransactionDataModel())
        {
            Model.Date = DateTime.Now;
        }

        public TransactionController( TransactionDataModel transactionModel ) : base( transactionModel )
        {
            Model = transactionModel;
        }

        public override bool PersistChanges()
        {
            try
            {
                UnitOfWork.Complete();
                return true;
            }
            catch ( Exception ex )
            {
                WindowService.ShowException( ex );
                return false;
            }
        }

        public override bool DeleteEntity()
        {
            try
            {
                UnitOfWork.Transactions.RemoveEntity(Model);
                return true;
            }
            catch ( Exception ex )
            {
                WindowService.ShowException( ex );
                return false;
            }
        }

        public static void UpdateCurrentBalanceOfEntity( FiscalEntityDataModel fiscalEntity )
        {
            var charged = fiscalEntity.Transactions.Where( t => t.TransactionType == TransactionType.Charge  ).Sum( t => t.Amount );
            var payed   = fiscalEntity.Transactions.Where( t => t.TransactionType == TransactionType.Payment ).Sum( t => t.Amount );

            var currentBalance = (charged - payed);

            fiscalEntity.CurrentBalance = currentBalance;
        }
    }
}
