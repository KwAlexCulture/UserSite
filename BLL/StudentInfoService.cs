using BOL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class StudentInfoService : BaseService<StudentInfo>
    {
        public StudentInfoService(StudentInfoRepository repo)
           : base(repo)
        {
        }

    }
}
