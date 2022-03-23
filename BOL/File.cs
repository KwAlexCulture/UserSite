using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL
{
    public class File : AuditableEntity
    {
        [Key]
        public Guid FileId { get; set; }
        public string FileType { get; set; }
        public string FilePath { get; set; }
        public string FileTitle { get; set; }
        public byte[] FileData { get; set; }
        public bool StatusCode { get; set; }
    }
}
