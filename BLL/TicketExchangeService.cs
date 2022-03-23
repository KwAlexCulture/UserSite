using BOL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class TicketExchangeService : BaseService<TicketExchange>
    {
        public TicketExchangeService(TicketExchangeRepository repo)
            : base(repo)
        {
        }
    }
}
