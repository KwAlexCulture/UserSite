using BOL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL
{
    public class AuditableEntity : IAuditableEntity
    {
        public Nullable<Guid> CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Nullable<Guid> ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        //[ForeignKey("CreatedBy")]
        //public virtual SecurityUser Creator { get; set; }
        //[ForeignKey("ModifiedBy")]
        //public virtual SecurityUser Modifier { get; set; }
    }
}
