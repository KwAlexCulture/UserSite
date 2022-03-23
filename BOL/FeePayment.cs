using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL
{
    public class FeePayment : AuditableEntity
    {
       [Key]
       public Guid FeePaymentId { get; set; }
       public string FullName { get; set; }
       public string NationalIDNo { get; set; }
       public Guid CollegeId  { get; set; }
       public Guid UniversityId  { get; set; }
       public string EducationLevel { get; set; }
       public string IsHavingSiblingsSameCollege { get; set; }
       public string RequestStatus { get; set; }
       public bool StatusCode { get; set; }
    }
}
