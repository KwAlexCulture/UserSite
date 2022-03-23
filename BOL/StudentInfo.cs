using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL
{
	public class StudentInfo : AuditableEntity
	{
		[Key]
		public Guid StudentInfoId { get; set; }
		public string StudentInfoFullName { get; set; }
		public string StudentInfoNationalIdNo { get; set; }
		public string StudentPassportNo { get; set; }
		public string StudentInfoNationality { get; set; }
		public Guid? ImageId { get; set; }
		public string StudentInfoEgCellNo { get; set; }
		public string StudentInfoKwCellNo { get; set; }
		public string StudentInfoUrgentEgCellNo { get; set; }
		public string StudentInfoEmail { get; set; }
		public string StudentInfoEgAddress { get; set; }
		public Guid UniversityId { get; set; }
		public Guid CollegeId { get; set; }
		public string EducationLevel { get; set; }
		public string EducationCategory { get; set; }
		public bool StatusCode { get; set; }
		public Guid SeasonId { get; set; }
	}
}
