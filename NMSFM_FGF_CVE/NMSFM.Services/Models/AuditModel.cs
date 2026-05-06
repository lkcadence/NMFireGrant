using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMSFM.Services.Models
{
    public class AuditModel
    {
        public string TableName { get; set; }
        public Guid RecordId { get; set; }
        public string AuditAction { get; set; }
        public string Description { get; set; }
    }
}
