using System;
using System.Collections.Generic;

namespace NMSFM.ViewModels
{
	public partial class DetailedApplication
	{

		public Guid? FYApplicationsId { get; set; }
		public long FYApplicationsIdentity { get; set; }
		public Guid AddressId { get; set; }
		public short Year { get; set; }
		public string PartyName { get; set; }
		public string AddressCode { get; set; }
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
		public short ISOClass { get; set; }  //From UDFs
		public int MainStationCount { get; set; } //From UDFs
		public int AdminBldgCount { get; set; } //From UDFs
		public int SubStationCount { get; set; } //From UDFs
		public decimal TotalFundDistribution { get; set; } //Calculated


		public string ISOContendSig { get; set; }  //FD Input
		public decimal? FPFBalance { get; set; }  //FD Input
		public decimal? FPFRollOverAmount { get; set; }  //FD Input
		public string FPFRollOverDescription { get; set; }  //FD Input	
		public string AppDaySubmitted { get; set; } //FD Input
		public string AppMonthSubmitted { get; set; } //FD Input
		public string GovOffElectronicSig { get; set; } //FD Input
		public string ChiefElectronicSig { get; set; } //FD Input

		

		public string Dist1Comm { get; set; }  //Header Info Agency UDFs
		public string Dist2Comm { get; set; }  //Header Info Agency UDFs
		public string Dist3Comm { get; set; }  //Header Info Agency UDFs
		public string Dist4Comm { get; set; }  //Header Info Agency UDFs
		public string Dist5Comm { get; set; }  //Header Info Agency UDFs
		public string FireMarshalName { get; set; }  //Header Info Agency UDFs
		public string ChiefofStaffTitle { get; set; }  //Header Info Agency UDFs
		public string ChiefofStaffName { get; set; }  //Header Info Agency UDFs
		public string ApplicationDueDate { get; set; }  //Header Info Agency UDFs
		public string FPFRollOverSubmittalDueDate { get; set; }  ///Agency UDFs

		public string Governor { get; set; }  //Header Info Agency UDFs
		public string CabinetSec { get; set; }  //Header Info Agency UDFs
		public string DeputyCabinetSec { get; set; }  //Header Info Agency UDFs


		public DetailedAddressParty NIFRSContact1 { get; set; }
		public DetailedAddressParty NIFRSContact2 { get; set; }

		public List<StationViewModel>MainStations { get; set; }
		public List<StationViewModel> AdminStations { get; set; }
		public List<StationViewModel> SubStations { get; set; }


		public bool Complete { get; set; }
		public Guid? CompletedBy { get; set; }
		public bool Approved { get; set; }
		public DateTime? DateApproved { get; set; }
		public Guid? ApprovedBy { get; set; }
	}
}
