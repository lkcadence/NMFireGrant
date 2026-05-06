using System;

namespace NMSFM.ViewModels
{
	public class DetailedFile
	{
		public Guid RecordId { get; set; }
		public Guid? AddressId { get; set; }
		public int selectedIndex { get; set; }
		public int[] FileCount { get; set; }
		public Data.File NewFile { get; set; }		
	}
}