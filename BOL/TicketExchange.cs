using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL
{
    public class TicketExchange : AuditableEntity
    {
       [Key]
       public Guid TicketExchangeId { get; set; }
       public string FullName { get; set; }
       public string NationalIDNo { get; set; }
       public Guid CollegeId  { get; set; }
       public Guid UniversityId  { get; set; }
       public string EducationLevel { get; set; }
       public Guid? ImageId { get; set; }
       public string RequestStatus { get; set; }
       public bool StatusCode { get; set; }
    }
}
