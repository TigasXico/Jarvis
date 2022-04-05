using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Jarvis.Data.DataModels
{
    public class ContactDataModel : BaseDataModel
    {
        public override string CommonId => ContactValue;

        public override string ShortDescriptor => ToString();

        private ContactType contactType;
        public ContactType ContactType
        {
            get => contactType;
            set => SetProperty( ref contactType , value );
        }

        [NotMapped]
        public string ContactTypeDescription
        {
            get
            {
                switch ( ContactType )
                {
                    case ContactType.PhoneNumber:
                        return "Telemóvel/Telefone";
                    case ContactType.Email:
                        return "Email";
                    default:
                        return string.Empty;
                }
            }
        }

        private string contactValue;
        public string ContactValue
        {
            get => contactValue;
            set => SetProperty( ref contactValue , value );
        }

        private int contactHolderId;

        public int ContactHolderId
        {
            get => contactHolderId;
            set => SetProperty( ref contactHolderId , value );
        }

        private FiscalEntityDataModel contactHolder;

        public FiscalEntityDataModel ContactHolder
        {
            get => contactHolder;
            set => SetProperty( ref contactHolder , value );
        }

        public override string ToString()
        {
            var description = new StringBuilder();
            description.AppendLine( $"Tipo de contacto: {ContactTypeDescription}" );
            description.Append( $"Contacto: {ContactValue}" );
            return description.ToString();
        }
    }

    public enum ContactType
    {
        PhoneNumber,
        Email
    }
}
