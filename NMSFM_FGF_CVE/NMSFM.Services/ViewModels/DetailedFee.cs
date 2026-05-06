using NMSFM.Services.Models;
using System;
using System.Collections.Generic;

namespace NMSFM.ViewModels
{
	public class DetailedFee
	{
		public bool? ActivityComplete { get; set; }
		public Guid AddressId { get; set; }
		public decimal? BalanceDue { get; set; }
		public string Code { get; set; }
		public string Comment { get; set; }
		public bool? Contract { get; set; }
		public string Description { get; set; }
		public string ExternalId { get; set; }
		public Guid FAgencyId { get; set; }
		public decimal? FeeAmt { get; set; }
		public string FeeBarcode { get; set; }
		public decimal? FeeBase { get; set; }
		public DateTime? FeeDate { get; set; }
		public string FeeDesc { get; set; }
		public Guid FeeId { get; set; }
		public int FeeStatus { get; set; }
		public string FeeType { get; set; }
		public Guid FeeTypeId { get; set; }
		public string FeeUOM { get; set; }
		public decimal? Hrs { get; set; }
		public Guid InspectedPartyId { get; set; }
		public DateTime? InspectionDate { get; set; }
		public Guid InspectionId { get; set; }
		public string InspectionNumber { get; set; }
		public string InventoryItem { get; set; }
		public string InvItemBarcode { get; set; }
		public Guid InvItemId { get; set; }
		public Guid InvoiceId { get; set; }
		public string InvoiceNumber { get; set; }
		public bool? IsDefault { get; set; }
		public bool? IsSub { get; set; }
		public Guid IssuedToPartyId { get; set; }
		public DateTime? OriginalFeeDate { get; set; }
		public Guid PAgencyId { get; set; }
		public Guid ParentInspectionId { get; set; }
		public Guid PartyID { get; set; }
		public string PartyName { get; set; }
		public decimal? PaymentAmt { get; set; }
		public DateTime? PaymentDate { get; set; }
		public Guid PaymentUserId { get; set; }
		public bool? Penalty { get; set; }
		public bool? PermitComplete { get; set; }
		public Guid PermitId { get; set; }
		public string PermitNumber { get; set; }
		public string PermitType { get; set; }
		public bool? ProjectComplete { get; set; }
		public Guid ProjectId { get; set; }
		public string ProjectNumber { get; set; }
		public bool? Rate { get; set; }
		public bool? RatedRange { get; set; }
		public DateTime? ReCalcDate { get; set; }
		public Guid RecordId { get; set; }
		public string RefNum { get; set; }
		public Guid ResponsiblePartyId { get; set; }
		public string RespParty { get; set; }
		public bool? TotalPercent { get; set; }
		public decimal? Units { get; set; }
		public List<UserDefinedValue> UserValues { get; set; }




	}
}
