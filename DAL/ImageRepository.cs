using BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ImageRepository : BaseRepository<Image>
    {
        public ImageRepository(EntitiesDbContext db)
            : base(db)
        { }
    }
}
