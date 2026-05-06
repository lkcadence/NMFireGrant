using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NMSFM.Data;
using NMSFM.ViewModels;

namespace NMSFM.ViewModels
{
	public partial class DetailedFYTotalDist
	{

		public short Year { get; set; }
		public short Quarter { get; set; }
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime InvoiceDate { get; set; }
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime? SentDate { get; set; }
		public bool Finalized { get; set; }
		public List<nm_FYTotalDistribution> TotalDists { get; set; }		
	}

	

}
