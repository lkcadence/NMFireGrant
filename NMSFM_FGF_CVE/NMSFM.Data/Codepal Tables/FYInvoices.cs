using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NMSFM.Data
{
    public partial class FYInvoices
    {
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[DefaultValue("newid()")]
		[Key]
		public Guid FYInvoiceId { get; set; }

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long FYInvoiceIdentity { get; set; }
		public string InvoiceNo { get; set; }

		[DefaultValue("YEAR(getdate())")]
		public short Year { get; set; }
		public Guid AddressId { get; set; }
		public DateTime InvoiceDate { get; set; }
		public short Quarter { get; set; }
		public string PartyName { get; set; }
		public DateTime? DateSent { get; set; }
		public decimal InvoiceAmount { get; set; }
		public bool Finalize { get; set; }
	}
}
