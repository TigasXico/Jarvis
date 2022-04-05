using System.Collections.ObjectModel;
using System.Text;

namespace Jarvis.Data.DataModels
{
    public class AggregateDataModel : NamedDataModel
    {
        public override string CommonId => $"{ID} - {Name}";

        public override string ShortDescriptor
        {
            get
            {
                if ( Members?.Count > 0 )
                {

                    var description = new StringBuilder();

                    description.AppendLine( "Aggregado:" );

                    foreach ( var member in Members )
                    {
                        description.AppendLine( member.Name );
                    }

                    return description.ToString();
                }
                else
                {
                    return "Agregado vazio ...";
                }
            }
        }

        private ObservableCollection<ClientDataModel> members;

        public virtual ObservableCollection<ClientDataModel> Members
        {
            get => members;
            set => SetProperty( ref members , value );
        }
    }
}
