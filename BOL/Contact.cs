using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL
{
    public class Contact : AuditableEntity
    {
        [Key]
        public Guid ContactId { get; set; }
        public string ContactFullName { get; set; }
        public int ContactNationalIDNo { get; set; }
        public int ContactPhoneNo { get; set; }
        public string ContactEmail { get; set; }
        public string ContactMessageHeader { get; set; }
        public string ContactMessageBody { get; set; }
        public bool StatusCode { get; set; }
    }
}
