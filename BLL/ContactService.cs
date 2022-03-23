using BOL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ContactService : BaseService<Contact>
    {
        public ContactService(ContactRepository repo)
           : base(repo)
        {
        }

    }
}
