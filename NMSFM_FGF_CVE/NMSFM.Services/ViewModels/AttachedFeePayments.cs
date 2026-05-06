using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMSFM.ViewModels
{
	public class AttachedFeePayments
	{
		public Guid FeeId { get; set; }
		public Guid FeePaymentId { get; set; }
		public DateTime? PaymentDate { get; set; }
		public decimal PaymentAmt { get; set; }		
		public string PaymentType { get; set; }		
		public string RefNum { get; set; }
		public string ReceivedFrom { get; set; }
		public string InspectorName { get; set; }
		public bool? Void { get; set; }
	}
}
