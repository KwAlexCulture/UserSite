using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BOL
{
    public class Child : AuditableEntity
    {
        [Key]
        public Guid ChildId { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public DateTime DOB { get; set; }
        public string WhatsAppNo { get; set; }
        public string ContactNo { get; set; }
        public string Classroom { get; set; }
        public int TShirtSize { get; set; }
        public Guid? ImageId { get; set; }
        public string Notes { get; set; }
        public Image QRCode { get; set; }
        public bool StatusCode { get; set; }
    }
}
