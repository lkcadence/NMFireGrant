namespace NMSFM.Data
{
	using System;
	using System.ComponentModel.DataAnnotations;

	public partial class nm_FYDistributionInvoice
	{
		[Key]
		public Guid? FYInvoiceId { get; set; }
		public string InvoiceNo { get; set; }
		public short Year { get; set; }
		public DateTime InvoiceDate { get; set; }
		public DateTime? DateSent { get; set; }
		public short Quarter { get; set; }
		public bool Finalize { get; set; }
		public decimal InvoiceAmount { get; set; }
		public Guid AddressId { get; set; }
		public Guid AddressTypeId { get; set; }
		public Guid PartyId { get; set; }	
		public Guid RoleTypeId { get; set; }
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
		public string Governor { get; set; }  //Header Info Agency UDFs
		public string CabinetSec { get; set; }  //Header Info Agency UDFs
		public string DeputyCabinetSec { get; set; }  //Header Info Agency UDFs
		public string FireMarshalName { get; set; }  //Header Info Agency UDFs
		public string ChiefofStaffTitle { get; set; }  //Header Info Agency UDFs
		public string ChiefofStaffName { get; set; }  //Header Info Agency UDFs

	}
}
