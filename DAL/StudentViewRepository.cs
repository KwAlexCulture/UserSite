using BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class StudentViewRepository : BaseRepository<StudentView>
    {
        public StudentViewRepository(EntitiesDbContext db)
            : base(db)
        { }
    }
}
