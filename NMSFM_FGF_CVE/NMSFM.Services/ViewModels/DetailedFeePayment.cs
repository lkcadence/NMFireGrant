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
	public class DetailedFeePayment
	{
		public Guid FeeId { get; set; }
		public Guid FeePaymentId { get; set; }		
		public Guid? PaymentUserId { get; set; }
		public string InspectorName { get; set; }
		[Required(ErrorMessage = "Amount is Required")]
		[LessThanOrEqualTo("FeeBalance",DependentPropertyDisplayName = "Fee Balance", ErrorMessage = "Must not be greater than balance")]
		public decimal? PaymentAmt { get; set; }
		[Required(ErrorMessage = "Payment Date is Required")]		
		public DateTime? PaymentDate { get; set; }
		[Required(ErrorMessage = "Payment Type is Required")]
		public string PaymentType { get; set; }
		public string ReceivedFrom { get; set; }
		public string RefNum { get; set; }
		public string Comment { get; set; }
		public bool Void { get; set; }
		public Guid AddressId { get; set; }
		public decimal FeeBalance { get; set; }
	}	
}
