using System;
using System.ComponentModel;
using System.Text;

namespace Jarvis.Data.DataModels
{
    public class TransactionDataModel : BaseDataModel
    {
        public override string CommonId => TransactionName;

        public override string ShortDescriptor
        {
            get
            {
                var description = new StringBuilder();
                description.AppendLine( $"Data: {Date.ToShortDateString()}" );
                description.AppendLine( $"Serviço: {TransactionName}" );
                description.AppendLine( $"Cobrado: {Amount} €" );
                if ( !string.IsNullOrWhiteSpace( Notes ) )
                {
                    description.AppendLine( $"Notas: {Amount} €" );
                }

                return description.ToString();
            }
        }

        private FiscalEntityDataModel requestor;
        public virtual FiscalEntityDataModel Requestor
        {
            get => requestor;
            set => SetProperty(ref requestor , value);
        }

        private int requestorId;
        public int RequestorId
        {
            get => requestorId;
            set => SetProperty( ref requestorId , value);
        }

        private string transactionName;
        public string TransactionName
        {
            get => transactionName;
            set => SetProperty ( ref transactionName , value );
        }

        private decimal amount;
        public decimal Amount
        {
            get => amount;
            set => SetProperty( ref amount, value );
        }

        public string AmountAsString => Amount.ToString( "C2" );

        private TransactionType transactionType;
        public TransactionType TransactionType
        {
            get => transactionType;
            set => SetProperty( ref transactionType , value);
        }

        private DateTime date = DateTime.Today;
        public DateTime Date
        {
            get => date;
            set => SetProperty( ref date , value );
        }

        private string notes;
        public string Notes
        {
            get => notes;
            set => SetProperty( ref notes , value );
        }

    }

    public enum TransactionType
    {
        [Description("Desconhecido")]
        Unknown,
        [Description( "Cobrança" )]
        Charge,
        [Description( "Pagamento" )]
        Payment
    }
}
