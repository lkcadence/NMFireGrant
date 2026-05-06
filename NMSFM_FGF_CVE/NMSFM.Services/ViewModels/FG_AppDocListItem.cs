using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMSFM.Data;

namespace NMSFM.ViewModels
{
    [Serializable]
    public class FG_AppDocListItem
    {
        public Guid DocumentId { get; set; }
        public Guid ApplicationId { get; set; }
        public string DocumentType { get; set; }
        public string DocumentName { get; set; }
        public string DocType { get; set; }
    }
}
