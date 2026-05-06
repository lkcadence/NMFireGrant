using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace NMSFM.ViewModels
{
    public class AttachedProjects
    {
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectNumber { get; set; }
        public string ProjectType { get; set; }
        public bool Complete { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}