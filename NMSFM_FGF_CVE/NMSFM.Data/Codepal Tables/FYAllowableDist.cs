using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NMSFM.Data
{ 
	public partial class FYAllowableDist
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[DefaultValue("newid()")]
		[Key]
		public Guid FYAllowableDistId { get; set; }

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
		public long FYAllowableDistIdentity { get; set; }
		public short Year { get; set; }
		public string CalculationPer { get; set; }

		[DefaultValue("0")]
		public decimal? FYAuditedRevenue { get; set; }

		[DefaultValue("0")]
		public decimal? PERA { get; set; }

		[DefaultValue("0")]
		public decimal? FYAllowedDistribution { get; set; }

		[DefaultValue("0")]
		public decimal? FYActualDistribution { get; set; }

		[DefaultValue("0")]
		public decimal? FYStatuteDistribution { get; set; }

		[DefaultValue("0")]
		public decimal? NMFAFYPayment { get; set; }

		[DefaultValue("0")]		
		public decimal? FYDistributionFactor { get; set; }

		[DefaultValue("0")]
		public decimal? FYDistToDept { get; set; }

		[DefaultValue(false)]
		public bool FYDistCalcAccepted { get; set; }
	}
}
