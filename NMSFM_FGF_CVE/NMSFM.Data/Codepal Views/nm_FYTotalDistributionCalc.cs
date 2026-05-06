namespace NMSFM.Data
{
	using System;
	using System.ComponentModel.DataAnnotations;

	
	public partial class nm_FYTotalDistributionCalc

	{
		
		public short Year { get; set; }
		[Key]
		public Guid AddressId { get; set; }
		public bool Inactive { get; set; }
		public string AddressCode { get; set; }
		public string AddressNumber { get; set; }
		public string Direction { get; set; }
		public string Address { get; set; }
		public string SubAddress { get; set; }
		public string City { get; set; }
		public Guid CountyId { get; set; }
		public string County { get; set; }
		public short ISOClass { get; set; }
		public int MainStationCount { get; set; }
		public int AdminBldgCount { get; set; }
		public int SubStationCount { get; set; }
		public decimal NMFAAmount { get; set; }
		public decimal MainCalcAmountRnd { get; set; }
		public decimal SubCalcAmountRnd { get; set; }
		public decimal MainCalcTotalRnd { get; set; }
		public decimal AdminCalcTotalRnd { get; set; }
		public decimal MainAdmCalcTotalRnd { get; set; }
		public decimal SubCalcTotalRnd { get; set; }
		public decimal FireFundDist { get; set; }		
		public decimal FYTotalDistribution { get; set; }
	}
}
