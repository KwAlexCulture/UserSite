using BOL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ClinicalAllowanceService : BaseService<ClinicalAllowance>
    {
        public ClinicalAllowanceService(ClinicalAllowanceRepository repo)
            : base(repo)
        {
        }
    }
}
