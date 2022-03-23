using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL
{
    public class Season : AuditableEntity
    {
			public Guid SeasonId { get; set; }
			public string StartYear { get; set; }
			public string EndYear { get; set; }
			public bool IsCurrentYear { get; set; }
			public string SeasonNotes { get; set; }
			public bool StatusCode { get; set; }
	}
}
