using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NMSFM.ViewModels
{
	public class DetailedAddressParty
	{
		public Guid? AddressPartyId { get; set; }
		public Guid? PartyId { get; set; }
		public Guid? AddressId { get; set; }
		public string PartyName { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string PhoneExt { get; set; }
		public string Fax { get; set; }
		public string FaxExt { get; set; }
		public string Cell { get; set; }
		public string CellExt { get; set; }
		public string Pager { get; set; }
		public string PagerExt { get; set; }
		public Guid? RoleTypeId { get; set; }
		public bool FromWeb { get; set; }
	}
}