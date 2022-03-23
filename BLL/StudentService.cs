using BOL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class StudentService : BaseService<Student>
    {
        public StudentService(StudentRepository repo)
           : base(repo)
        {
        }

    }
}
