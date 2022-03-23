using BOL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ChildService : BaseService<Child>
    {
        public ChildService(ChildRepository repo)
            : base(repo)
        {
        }
    }
}
