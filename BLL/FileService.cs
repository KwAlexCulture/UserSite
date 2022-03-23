using BOL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class FileService : BaseService<File>
    {
        public FileService(FileRepository repo)
           : base(repo)
        {
        }

    }
}
