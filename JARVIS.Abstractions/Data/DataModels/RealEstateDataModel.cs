using System.Collections.Generic;
using System.Text;

namespace Jarvis.DataModels
{
    public class RealEstateDataModel : BaseDataModel
    {
        public override string CommonId => FullArticle;

        public override string ShortDescriptor => ToString();

        private FiscalEntityDataModel owner;
        /// <summary>
        /// A reference to the owner of the Vehiecle
        /// </summary>
        public FiscalEntityDataModel Owner
        {
            get => owner;
            set => SetProperty( ref owner , value );
        }

        private int ownerId;
        /// <summary>
        /// A reference to the owner of the Vehiecle
        /// </summary>
        public int OwnerId
        {
            get => ownerId;
            set => SetProperty( ref ownerId , value );
        }

        private string location;
        public string Location
        {
            get => location;
            set => SetProperty( ref location , value );
        }

        private string fullArticle;
        public string FullArticle
        {
            get => fullArticle;
            set
            {
                SetProperty( ref fullArticle , value );
                string[] splittedArticle = fullArticle.Split( '-' );
                if ( splittedArticle.Length == 3 )
                {
                    Type = splittedArticle[0];
                    Article = splittedArticle[1];
                    Fraction = splittedArticle[2];
                }
            }
        }

        private string type;
        public string Type
        {
            get => type;
            private set => SetProperty( ref type , value );
        }

        private string article;
        public string Article
        {
            get => article;
            private set => SetProperty( ref article , value );
        }

        private string fraction;
        public string Fraction
        {
            get => fraction;
            private set => SetProperty( ref fraction , value );
        }

        private string part;
        public string Part
        {
            get => part;
            set => SetProperty( ref part , value );
        }

        private int? matrixYear;
        public int? MatrixYear
        {
            get => matrixYear;
            set => SetProperty( ref matrixYear , value );
        }

        private string initialValue;
        public string InitialValue
        {
            get => initialValue;
            set => SetProperty( ref initialValue , value );
        }

        private string currentValue;
        public string CurrentValue
        {
            get => currentValue;
            set => SetProperty( ref currentValue , value );
        }

        private List<TagDataModel> tags;
        /// <summary>
        /// The tags of this Fiscal Entity
        /// </summary>
        public List<TagDataModel> Tags
        {
            get => tags;
            set => SetProperty( ref tags , value );
        }

        public override string ToString()
        {
            StringBuilder description = new StringBuilder();
            description.AppendLine( $"Artigo completo: {FullArticle}" );
            description.AppendLine( $"Localização: {Location}" );
            description.AppendLine( $"Parte: {Part}" );
            description.AppendLine( $"Ano de matriz: {MatrixYear}" );
            description.AppendLine( $"Valor inicial: {InitialValue}" );
            description.Append( $"Valor atual: {CurrentValue}" );
            return description.ToString();
        }
    }
}
