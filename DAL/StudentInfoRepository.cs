using BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class StudentInfoRepository : BaseRepository<StudentInfo>
    {
        public StudentInfoRepository(EntitiesDbContext db)
            : base(db)
        { }
    }
}
