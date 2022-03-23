using BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class FileRepository : BaseRepository<File>
    {
        public FileRepository(EntitiesDbContext db)
            : base(db)
        { }
    }
}
