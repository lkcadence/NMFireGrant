using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace NMSFM.ViewModels
{
    public class AttachedNotes
    {
        public Guid NoteId { get; set; }
        public DateTime NoteDate { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public string ObjectRef { get; set; }
    }
}