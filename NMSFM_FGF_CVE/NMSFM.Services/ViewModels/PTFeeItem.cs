using NMSFM.Services.Models;
using System;
using System.Collections.Generic;

namespace NMSFM.ViewModels
{
	public class PTFeeItem
	{
		public Guid FeesPTId { get; set; }
		public Guid BaseFeeId { get; set; }
		public Guid FeeId { get; set; }
		public bool Select { get; set; }
		public string FeeType { get; set; }
		public decimal? FeeAmt { get; set; }
	}
}
