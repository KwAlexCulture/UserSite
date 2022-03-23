using BOL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ImageService : BaseService<Image>
    {
        public ImageService(ImageRepository repo)
           : base(repo)
        {
        }

    }
}
