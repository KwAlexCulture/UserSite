using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BOL
{
    public class Image : AuditableEntity
    {
        [Key]
        public Guid ImageId { get; set; }
        public string ImageType { get; set; }
        public string ImagePath { get; set; }
        public string ImageTitle { get; set; }
        public byte[] ImageData { get; set; }
        public bool StatusCode { get; set; }
    }
}
