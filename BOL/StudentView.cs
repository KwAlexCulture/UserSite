using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BOL
{
    public class StudentView : AuditableEntity
    {
        [Key]
        public Guid StudentId { get; set; }
        public string FullName { get; set; }
        public string NationalIDNo { get; set; }
        public string EducationLevel { get; set; }
        public string PassportNo { get; set; }
        public Guid? CollegeId { get; set; }
        public Guid? UniversityId { get; set; }
        public Guid? FileId { get; set; }
        public int RegistrationNo { get; set; }
        public Guid? ImageId { get; set; }
        public Guid? StudentCollegeId { get; set; }
        public Guid? StudentUniversityId { get; set; }
        public string CollegeName { get; set; }
        public string UniversityName { get; set; }
        public string RequestStatus { get; set; }
        public bool StatusCode { get; set; }
    }
}
