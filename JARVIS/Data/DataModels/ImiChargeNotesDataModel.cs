using System;

namespace Jarvis.Data.DataModels
{
    public enum ImiChargeNoteStatus
    {
        PendingEmition = 1,
        Emited = 2,
        UnknownState3 = 3,
        Payed = 4,
        UnknownState5 = 5,
        UnknownState6 = 6,
        Canceled = 7,
        PayedOnOverdue = 8
    }

    public class ImiChargeNotesDataModel : BaseDataModel
    {
        public override string CommonId => throw new NotImplementedException();

        public override string ShortDescriptor => throw new NotImplementedException();

        private int payingEntityId;
        public int PayingEntityId
        {
            get => payingEntityId;
            set => SetProperty( ref payingEntityId , value );
        }

        private FiscalEntityDataModel payingEntity;
        public FiscalEntityDataModel PayingEntity
        {
            get => payingEntity;
            set => SetProperty(ref payingEntity , value);
        }

        private string year;
        public string Year
        {
            get => year;
            set => SetProperty (ref year , value );
        }


        private string chargeNoteNumber;
        public string ChargeNoteNumber
        {
            get => chargeNoteNumber;
            set => SetProperty(ref chargeNoteNumber , value );
        }

        private decimal paymentValue;
        public decimal PaymentValue
        {
            get => paymentValue;
            set => SetProperty (ref paymentValue , value);
        }

        private DateTime limitDate;
        public DateTime LimitDate
        {
            get => limitDate;
            set => SetProperty(ref limitDate , value);
        }

        private ImiChargeNoteStatus status;
        public ImiChargeNoteStatus Status
        {
            get => status;
            set => SetProperty( ref status , value );
        }

        private string paymentReference;
        public string PaymentReference
        {
            get => paymentReference;
            set => SetProperty( ref paymentReference , value );
        }

        private int numberOfBuildings;
        public int NumberOfBuildings
        {
            get => numberOfBuildings;
            set => SetProperty( ref numberOfBuildings , value );
        }
    }
}
