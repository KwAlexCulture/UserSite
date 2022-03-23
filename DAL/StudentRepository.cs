using BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class StudentRepository : BaseRepository<Student>
    {
        public StudentRepository(EntitiesDbContext db)
            : base(db)
        { }
    }
}
