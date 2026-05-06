using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMSFM.ViewModels
{
    public class DetailedFGApplication
    {
		public Guid? ApplicationId { get; set; }
		public short? FiscalYear { get; set; }
		public string ApplicationNumber { get; set; }
		public DateTime? DateSubmitted { get; set; }
		public short? AppStatus { get; set; }
		public string ApplicationStatus { get; set; }
		public DateTime? LastStatusChange { get; set; }
		public bool InstructionsSubmitted { get; set; }
		public Guid AddressId { get; set; }
		public Guid AddressTypeId { get; set; }
		public Guid PartyID { get; set; }
		public Guid RoleTypeId { get; set; }
		public string PartyName { get; set; }
		public string AddressCode { get; set; }
		public string AddressNumber { get; set; }
		public string Direction { get; set; }
		public string Address { get; set; }
		public string SubAddress { get; set; }
		public string City { get; set; }
		public string County { get; set; }
		public string StateAbbr { get; set; }
		public string Zip { get; set; }
		public bool Inactive { get; set; }
		
	}
}
