using BOL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class CertifiedRecruitmentService : BaseService<CertifiedRecruitment>
    {
        public CertifiedRecruitmentService(CertifiedRecruitmentRepository repo)
            : base(repo)
        {
        }
    }
}
