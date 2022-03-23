using BOL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class AnnualAllowanceExchangeService : BaseService<AnnualAllowancesExchange>
    {
        public AnnualAllowanceExchangeService(AnnualAllowanceExchangeRepository repo)
            : base(repo)
        {
        }
    }
}
