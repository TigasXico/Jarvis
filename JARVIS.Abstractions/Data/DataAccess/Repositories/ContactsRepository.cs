using Jarvis.DataAccess.Database;
using Jarvis.DataAcess.Contract;
using Jarvis.DataModels;

namespace Jarvis.DataAccess.Repositories
{
    internal class ContactsRepository : Repository<ContactDataModel>, IContactsRepository
    {
        public JarvisContext CurrentContext => Context as JarvisContext;

        public ContactsRepository( JarvisContext context ) : base( context )
        {
        }
    }
}
