using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NMSFM.ViewModels
{
    public class AttachedInventoryItems
    {
        public Guid InvItemId { get; set; }
        public string InventoryItem { get; set; }
        public string Manufacturer { get; set; }
        public string ModelNumber { get; set; }
        public string PartNumber { get; set; }
    }
}