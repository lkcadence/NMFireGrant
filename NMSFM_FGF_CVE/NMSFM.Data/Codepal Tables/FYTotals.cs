using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NMSFM.Data
{
	public partial class FYTotal
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[DefaultValue("newid()")]
		[Key]
		public Guid FYTotalsId { get; set; }

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long FYTotalsIdentity { get; set; }

		[DefaultValue("YEAR(getdate())")]
		public short Year { get; set; }

		[DefaultValue("0")]
		public decimal? FYTotalAvailableFunding { get; set; }

		[DefaultValue("0")]
		public decimal? FYTotalDistribution { get; set; }

		[DefaultValue("0")]
		public decimal? FYTotalDistToDept { get; set; }
	}
}
