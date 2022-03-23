using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL
{
    public class Student : AuditableEntity
    {
        [Key]
        public Guid StudentId { get; set; }
        public string FullName { get; set; }
        public string NationalIDNo { get; set; }
        public int RegistrationNo { get; set; }

        //public int RegNo { get; set; }
        public string CollegeName { get; set; }
        public string PassportNo { get; set; }
        public string EducationLevel { get; set; }
        public string CellularNoKuwait { get; set; }
        public string CellularNoEgypt { get; set; }
        public string Email { get; set; }
        public Guid? UniversityId { get; set; }
        public Guid? CollegeId { get; set; }
        public Guid? ImageId { get; set; }
        public Guid? FileId { get; set; }
        public string RequestStatus { get; set; }
        public bool StatusCode { get; set; }
    }
}
