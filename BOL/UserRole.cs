using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL
{
    public class UserRole : AuditableEntity
    {
        [Key]
        public Guid UserRoleId { get; set; }
        public string UserRoleTitle { get; set; }
        public bool StatusCode { get; set; }
    }
}
