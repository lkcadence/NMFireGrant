using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NMSFM.Data
{
	public partial class FYTotalDist
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[DefaultValue("newid()")]
		[Key]
		public Guid FYTotalDistId { get; set; }
		
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long FYTotalDistIdentity { get; set; }

		[DefaultValue("YEAR(getdate())")] 
		public short Year { get; set; }
		public Guid AddressId { get; set; }
		public short ISOClass { get; set; }

		[DefaultValue("0")] 
		public int MainStationCount { get; set; }

		[DefaultValue("0")] 
		public int AdminBldgCount { get; set; }

		[DefaultValue("0")] 
		public int SubStationCount { get; set; }

		[DefaultValue("0")] 
		public decimal FireFundDist { get; set; }

		[DefaultValue("0")] 
		public decimal NMFAAmount { get; set; }

		[DefaultValue("0")] 
		public decimal FYTotalDistribution { get; set; }

		[DefaultValue("0")]
		public decimal MainCalcTotalRnd { get; set; }

		[DefaultValue("0")]
		public decimal AdminCalcTotalRnd { get; set; }
		[DefaultValue("0")]
		public decimal MainAdmCalcTotalRnd { get; set; }

		[DefaultValue("0")]
		public decimal SubCalcTotalRnd { get; set; }		
	}
}
