using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL
{
    public class College : AuditableEntity
    {
        [Key]
       public Guid CollegeId { get; set; }
       public string CollegeName { get; set; }
       public bool FeePaymentEntity { get; set; }
       public bool ClinicalAllowanceEntity { get; set; }
       public bool TicketExchangeEntity { get; set; }
       public bool AnnualAllowancesExchangeEntity { get; set; }
       public bool StatusCode { get; set; }
    }
}
