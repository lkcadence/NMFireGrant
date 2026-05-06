using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NMSFM.Data
{
	public partial class FYTotalDistCalc
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[DefaultValue("newid()")]
		[Key]
		public Guid FYTotalDistCalcId { get; set; }

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
		public long FYTotalDistCalcIdentity { get; set; }
		public short Year { get; set; }
		public short ISOClass { get; set; }		
		public decimal MainGrowthAmount { get; set; }
		
		public decimal MainCalcAmount { get; set; }

		public decimal MainCalcAmountRnd { get; set; }

		public short MainCount { get; set; }

		public decimal MainStatuteTotal { get; set; }

		public decimal MainCalcTotal { get; set; }

		public decimal MainCalcTotalRnd { get; set; }

		public decimal SubGrowthAmount { get; set; }

		public decimal SubCalcAmount { get; set; }

		public decimal SubCalcAmountRnd { get; set; }

		public short SubCount { get; set; }

		public decimal SubStatuteTotal { get; set; }

		public decimal SubCalcTotal { get; set; }

		public decimal SubCalcTotalRnd { get; set; }

	}
}
