using BOL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class StudentViewService : BaseService<StudentView>
    {
        public StudentViewService(StudentViewRepository repo)
            : base(repo)
        {
        }
    }
}
