using System;
using System.Collections.Generic;
using NMSFM.Data;
using NMSFM.ViewModels;

namespace NMSFM.ViewModels
{
	public partial class DetailedFYTotalDistCalc
	{

		public short Year { get; set; }		
		public List<nm_FYTotalDistributionCalc> TotalDists { get; set; }
		
	}

	

}
