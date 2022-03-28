using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BOL
{
    public class CertifiedRecruitment : AuditableEntity
    {
        [Key]
        public Guid CertifiedRecruitmentId { get; set; }
        public string FullName { get; set; }
        public string NationalIDNo { get; set; }
        public string State { get; set; }
        public string DOB { get; set; }
        public string PlaceOfBirth { get; set; }
        public string KuwaitAddress { get; set; }
        public string HostCountryAddress { get; set; }
        public string EducationStage { get; set; }
        public string ExpetctedCertificate { get; set; }
        public string IsStillEducating { get; set; }
        public string EducationLevel { get; set; }
        public string EducationDiscontinuityReason { get; set; }
        public string RequestStatus { get; set; }
        public string RefuseReason { get; set; }
        public string CellularNoEgypt { get; set; }
        public bool StatusCode { get; set; }
    }
}
