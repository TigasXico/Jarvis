using Jarvis.Data.DataModels;
using Jarvis.DataAccess.Database;
using Jarvis.DataAcess.Contract;

namespace Jarvis.DataAccess.Repositories
{
    internal class ContactsRepository : Repository<ContactDataModel>, IContactsRepository
    {
        public ContactsRepository( JarvisContext context ) : base( context )
        {
        }
    }
}
