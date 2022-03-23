using BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CertifiedRecruitmentRepository : BaseRepository<CertifiedRecruitment>
    {
        public CertifiedRecruitmentRepository(EntitiesDbContext db)
            : base(db)
        { }
    }
}
