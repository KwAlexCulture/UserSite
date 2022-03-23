using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL
{
    public class SameCollegeSebling : AuditableEntity
    {
       [Key]
       public Guid SameCollegeSeblingId { get; set; }
       public Guid FeePaymentId { get; set; }
       public string SeblingFullName { get; set; }
       public int SeblingNationalIDNo { get; set; }
       public bool StatusCode { get; set; }
    }
}
