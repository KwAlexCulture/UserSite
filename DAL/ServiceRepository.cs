using BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ContactRepository : BaseRepository<Contact>
    {
        public ContactRepository(EntitiesDbContext db)
            : base(db)
        { }
    }
}
