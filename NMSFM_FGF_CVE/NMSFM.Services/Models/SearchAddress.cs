using System;

namespace NMSFM.ViewModels
{
	public class SearchAddress
	{
		public Guid AddressId { get; set; }
		public bool Inactive { get; set; }
		public string AddressType { get; set; }
		public Guid AddressTypeId { get; set; }
		public string AddressCode { get; set; }
		public string AddressNumber { get; set; }
		public string Direction { get; set; }
		public string Address { get; set; }
		public string SubAddress { get; set; }
		public string City { get; set; }
		public string Suffix { get; set; }
		public string StateAbbr { get; set; }
		public string Zip { get; set; }
		public string Comment { get; set; }
		public string Party { get; set; }
		public string Region { get; set; }
		public string County { get; set; }
		public string Occupancy { get; set; }
		public string Property { get; set; }
	}
}