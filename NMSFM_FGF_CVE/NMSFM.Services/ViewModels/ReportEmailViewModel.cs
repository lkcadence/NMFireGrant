using System;

namespace NMSFM.ViewModels
{
    public class ReportEmailViewModel
    {
        public string FromEmail { get; set; }

        public string ToEmail { get; set; }

        public string CCEmail { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }
    }
}