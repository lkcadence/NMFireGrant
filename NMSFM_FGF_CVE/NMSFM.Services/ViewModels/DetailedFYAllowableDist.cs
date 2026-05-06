using System;
using System.Collections.Generic;
using NMSFM.Data;
using NMSFM.ViewModels;

namespace NMSFM.ViewModels
{
	public partial class DetailedFYAllowableDist
	{

		public Guid FYAllowableDistId { get; set; }
		public long FYAllowableDistIdentity { get; set; }
		public short Year { get; set; }
		public string CalculationPer { get; set; }
		public decimal? Prev2FYRevenue { get; set; }
		public decimal? Prev3FYRevenue { get; set; }
		public decimal? RevDifference { get; set; }
		public decimal? PERA { get; set; }
		public decimal? FYIncrease { get; set; }
		public decimal? PrevFYActualDistribution { get; set; }
		public decimal? FYAllowableDistribution { get; set; }
		public decimal? NMFAFYPayment { get; set; }
		public decimal? FYDistributionFactor { get; set; }
		public List<FYStatuteDist> StatuteDists { get; set; }
		public List<FYTotalDistCalc> TotalDistCalcs { get; set; }
		public List<DistYear> Years { get; set; }
		public bool isStats { get; set; }
		public bool yearComplete { get; set; }
	}

	public class DistYear
	{
		public Guid FYAllowableDistId { get; set; }
		public string Year { get; set; }
		
	}

}
