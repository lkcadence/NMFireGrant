using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMSFM.Services.Models
{
    public class AuditFieldModel
    {
        public string ControlName { get; set; }
        public string FieldDesc { get; set; }
        public Guid? OldId { get; set; }
        public string OldValue { get; set; }
        public Guid? NewId { get; set; }
        public string NewValue { get; set; }
    }
}
