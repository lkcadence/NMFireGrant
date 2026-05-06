using System;
using System.Collections.Generic;
using NMSFM.Data;
using NMSFM.ViewModels;

namespace NMSFM.ViewModels
{
	public partial class DetailedFYAppList
	{
		
		public short Year { get; set; }		
		public List<nm_FYDetailedApplication> AppList { get; set; }		
	}

	

}
