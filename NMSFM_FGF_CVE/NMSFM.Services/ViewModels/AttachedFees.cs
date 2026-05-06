using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NMSFM.ViewModels
{
    public class AttachedFees
    {
        public Guid FeeId { get; set; }
        public Guid? FeeTypeId { get; set; }
        public string FeeType { get; set; }
        public DateTime FeeDate { get; set; }
        public decimal? FeeAmt { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal? PaymentAmt { get; set; }
        public decimal? BalanceDue { get; set; }
		public decimal? FeeBase { get; set; }
		public decimal? Units { get; set; }
	}
}