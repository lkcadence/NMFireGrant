using NMSFM.Services.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foolproof;

namespace NMSFM.ViewModels
{
	public class DetailedDefaultFee
	{
		public Guid DefaultFeeId { get; set; }
		public Guid RecordId { get; set; }
		public Guid FeeTypeId { get; set; }
		public bool ForReInspection { get; set; }
		[StringLength(2)]
		public string ReInspectionLetter { get; set; }
		public bool ReInspForward { get; set; }
		[StringLength(2)]
		public string EndReInspectionLetter { get; set; }
		[StringLength(100)]
		public string FeeAmount { get; set; }		
		public DateTime DateUpdated { get; set; }
		public DateTime DateInserted { get; set; }
		public Guid? FeeSchedId { get; set; }
		public bool? IsUpdated { get; set; }
		public bool? IsNew { get; set; }
		public bool? IsDeleted { get; set; }
		public Guid DefaultInvoiceTypeId { get; set; }
		public bool Rate { get; set; }
		public bool RatedRange { get; set; }
		public bool TotalPercent { get; set; }
		public bool Penatly { get; set; }
		public bool Contract { get; set; }		
	}
}
