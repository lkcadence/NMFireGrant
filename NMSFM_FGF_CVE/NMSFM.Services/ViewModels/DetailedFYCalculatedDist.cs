using System;
using System.Collections.Generic;
using NMSFM.Data;
using NMSFM.ViewModels;

namespace NMSFM.ViewModels
{
	public partial class DetailedFYCalculatedDist
	{

		public Guid FYAllowableDistId { get; set; }
		public long FYAllowableDistIdentity { get; set; }
		public short Year { get; set; }
		public decimal? FYAllowedDistribution { get; set; }
		public decimal? FYActualDistribution { get; set; }
		public decimal? FYStatuteDistribution { get; set; }
		public decimal? NMFAFYPayment { get; set; }
		public decimal? FYDistributionFactor { get; set; }
		public decimal? FYDistToDept { get; set; }
		public List<FYStatuteDist> StatuteDists { get; set; }
		public List<FYTotalDistCalc> TotalDistCalcs { get; set; }
		public decimal? MainCountTotal { get; set; }
		public decimal? MainStatuteCalc { get; set; }
		public decimal? MainStatuteTotal { get; set; }
		public decimal? MainDistTotal { get; set; }
		public decimal? MainDistTotalRnd { get; set; }
		public decimal? SubCountTotal { get; set; }
		public decimal? SubStatuteCalc { get; set; }
		public decimal? SubStatuteTotal { get; set; }
		public decimal? SubDistTotal { get; set; }
		public decimal? SubDistTotalRnd { get; set; }
		public bool Final { get; set; }
		public bool FYDistCalcAccepted { get; set; }
		public bool UnFinal { get; set; }



	}


}
