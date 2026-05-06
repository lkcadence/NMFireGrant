using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NMSFM.ViewModels
{
	public class PermitAddress
	{
		public Guid AddressId { get; set; }		
		public string AddressType { get; set; }
		public string FullAddress { get; set; }
		public string AddressCode { get; set; }		
		public string AddressNumber { get; set; }
		public string Direction { get; set; }
		public string Address { get; set; }
		public string SubAddress { get; set; }
		public string City { get; set; }
		public string Suffix { get; set; }
		public string StateAbbr { get; set; }
		public string Zip { get; set; }
		public string TaxParcel { get; set; }
		public string Map { get; set; }
		public string Lot { get; set; }
		public string Block { get; set; }
		public string OccupancyType { get; set; }
		public string PropertyUseType { get; set; }
		public Guid OccupancyTypeId { get; set; }
		public Guid PropertyUseTypeId { get; set; }
	}
}