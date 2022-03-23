using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL.Interfaces
{
    public interface IAuditableEntity : IEntity
    {
        Nullable<Guid> CreatedBy { get; set; }
        DateTime CreatedOn { get; set; }
        Nullable<Guid> ModifiedBy { get; set; }
        DateTime ModifiedOn { get; set; }

    }
}
