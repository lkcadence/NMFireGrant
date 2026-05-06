namespace NMSFM.Data
{
	using System;
	using System.ComponentModel.DataAnnotations;

	public partial class nm_FYDetailedApplication
	{
		public Guid? FYApplicationsId { get; set; }
		public long? FYApplicationsIdentity { get; set; }
		[Key]
		public Guid AddressId { get; set; }
		public Guid AddressTypeId { get; set; }
		public Guid PartyID { get; set; }
		public short? Year { get; set; }
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
		public short? ISOClass { get; set; }
		public int? MainStationCount { get; set; }
		public int? AdminBldgCount { get; set; }
		public int? SubStationCount { get; set; }
		public string ISOContendSig { get; set; }  //FD Input
		public decimal? FPFBalance { get; set; }  //FD Input
		public decimal? FPFRollOverAmount { get; set; }  //FD Input
		public string FPFRollOverDescription { get; set; }  //FD Input	
		public Guid? NFIRSContact1Id { get; set; }
		public Guid? NFIRSContact2Id { get; set; }
		public string AppDaySubmitted { get; set; } //FD Input
		public string AppMonthSubmitted { get; set; } //FD Input
		public string GovOffElectronicSig { get; set; } //FD Input
		public string ChiefElectronicSig { get; set; } //FD Input
		public string Dist1Comm { get; set; }  //Header Info Agency UDFs
		public string Dist2Comm { get; set; }  //Header Info Agency UDFs
		public string Dist3Comm { get; set; }  //Header Info Agency UDFs
		public string Dist4Comm { get; set; }  //Header Info Agency UDFs
		public string Dist5Comm { get; set; }  //Header Info Agency UDFs
		public string Governor { get; set; }  //Header Info Agency UDFs
		public string CabinetSec { get; set; }  //Header Info Agency UDFs
		public string DeputyCabinetSec { get; set; }  //Header Info Agency UDFs
		public string FireMarshalName { get; set; }  //Header Info Agency UDFs
		public string ChiefofStaffTitle { get; set; }  //Header Info Agency UDFs
		public string ChiefofStaffName { get; set; }  //Header Info Agency UDFs
		public string ApplicationDueDate { get; set; }  //Header Info Agency UDFs
		public string FPFRollOverSubmittalDueDate { get; set; }  ///Agency UDFs
		[System.ComponentModel.DefaultValue(false)]
		public bool? Complete { get; set; }
		public Guid? CompletedBy { get; set; }
		public string AppType { get; set; }
		public bool? Approved { get; set; }
		public DateTime? DateApproved { get; set; }
		public Guid? ApprovedBy { get; set; }



	}
}
