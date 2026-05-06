using System;
using System.Collections.Generic;
using NMSFM.Data;

namespace NMSFM.ViewModels
{
    public partial class DetailedFYInvoice
    {
		public Guid? FYInvoiceId { get; set; }
		public long FYInvoiceIdentity { get; set; }
		public string InvoiceNo { get; set; }
		public short Year { get; set; }
		public DateTime InvoiceDate { get; set; }
		public DateTime DateSent { get; set; }
		public short Quarter { get; set; }
		public string QuarterString { get; set; }
		public bool Finalize { get; set; }
		public string PartyName { get; set; }
		public Guid AddressId { get; set; }
		public string AddressCode { get; set; }
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
		

		public decimal TotalFundDistribution { get; set; } //From Distribution FYTotalDists
		public decimal NMFAAmount { get; set; } //From Distribution FYTotalDists
		public decimal TotalAfterNFMA { get; set; } //calculated
		public decimal FirstAllotment { get; set; } //calculated
		public decimal SecondAllotment { get; set; } //calculated

		public string FireMarshalName { get; set; }  //Header Info Agency UDFs

		public string Governor { get; set; }  //Header Info Agency UDFs
		public string CabinetSec { get; set; }  //Header Info Agency UDFs
		public string DeputyCabinetSec { get; set; }  //Header Info Agency UDFs

		public List<DetailedFYInvoice> countyDepts { get; set; }

		public v_Addresses2 trAddress { get; set; }
	}
}
