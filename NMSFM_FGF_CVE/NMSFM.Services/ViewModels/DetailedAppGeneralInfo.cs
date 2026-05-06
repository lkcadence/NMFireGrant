using System;
using System.Collections.Generic;

namespace NMSFM.ViewModels
{
    public partial class DetailedAppGeneralInfo
    {
        public Guid? FireGranApptId { get; set; }
        public long FYApplicationsIdentity { get; set; }

		//From Address
        public Guid AddressId { get; set; }
        public short Year { get; set; }
        public string PartyName { get; set; }
		public string DepartmentName { get; set; }
		public string AddressNumber { get; set; }
		public string Direction { get; set; }
		public string Address { get; set; }
		public string Suffix { get; set; }
		public string SubAddress { get; set; }
		public string City { get; set; }
		public string County { get; set; }
		public string StateAbbr { get; set; }
		public string Zip { get; set; }
		public string DeptAddress { get; set; }

		//From ApplicationGeneralInfo table
		public string ConfirmationNumber { get; set; }
		public DateTime DateSubmitted { get; set; }
		public string ApplicationStatus { get; set; }
		public DateTime LastUpdateDate { get; set; }

	}
}
