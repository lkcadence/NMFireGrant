using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMSFM.ViewModels
{
	public class FeeTypeViewModel
	{
		public Guid FeeTypeId { get; set; }
		public string FeeType1 { get; set; }
		public bool Rate { get; set; }
		public bool RatedRange { get; set; }
		public bool TotalPercent { get; set; }
		public bool Penalty { get; set; }
		public bool? Contract { get; set; }
		public string RateFee { get; set; }
		public Guid? InvItemId { get; set; }
	}
}
