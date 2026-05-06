using System;

namespace NMSFM.ViewModels
{
    public class DetailedAddressFile
    {
        public Guid AddressId { get; set; }
        public int selectedIndex { get; set; }
        public int[] FileCount { get; set; }
        public NMSFM.Data.File NewFile { get; set; }
    }
}