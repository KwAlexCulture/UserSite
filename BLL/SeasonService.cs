using BOL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class SeasonService : BaseService<Season>
    {
        public SeasonService(SeasonRepository repo)
            : base(repo)
        {
        }
    }
}
