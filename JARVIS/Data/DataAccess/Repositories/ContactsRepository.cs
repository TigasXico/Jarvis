using Jarvis.Data.Contract.Repositories;
using Jarvis.Data.DataAccess.Database;
using Jarvis.Data.DataModels;

namespace Jarvis.Data.DataAccess.Repositories
{
    internal class ContactsRepository : Repository<ContactDataModel>, IContactsRepository
    {
        public ContactsRepository( JarvisContext context ) : base( context )
        {
        }
    }
}
