using System;
using AutoMapper;

namespace NMSFM.ViewModels
{
	public partial class StationViewModel
	{		
		public Guid? FYAppStationsId { get; set; }
		public Guid AddressId { get; set; }		
		public string AddressCode { get; set; }
		public string AddressNumber { get; set; }
		public string Direction { get; set; }
		public string Address { get; set; }
		public string Suffix { get; set; }
		public string SubAddress { get; set; }
		public string City { get; set; }
		public string StateAbbr { get; set; }
		public string Zip { get; set; }
		public string AddDesc { get; set; }

	}
}
