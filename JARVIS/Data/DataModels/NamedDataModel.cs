namespace Jarvis.Data.DataModels
{
    public abstract class NamedDataModel : BaseDataModel
    {
        private string name;
        public string Name
        {
            get => name;
            set => SetProperty( ref name , value );
        }
    }
}
