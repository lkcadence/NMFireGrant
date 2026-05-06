using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;

namespace NMSFM.Data
{
    public partial class FGApplications
    {
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[DefaultValue("newid()")]
		[Key]
		public Guid ApplicationId { get; set; }
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long FGApplicationIdentity { get; set; }
		[DefaultValue("YEAR(getdate())")]
		public short FiscalYear { get; set; }
		public Guid AddressId { get; set; }
		public string ApplicationNumber { get; set; }
		public DateTime DateStarted { get; set; }
		public DateTime? DateSubmitted { get; set; }
		public short AppStatus { get; set; }
		public DateTime LastStatusChange { get; set; }
		public bool InstructionsSubmitted { get; set; }
		public decimal GrantedAmount { get; set; }
		[DefaultValue(0.0000)]
		public decimal StipendAmount { get; set; }
		[DefaultValue(0.0000)]
		public string ApplicationNotes { get; set; }
		public Guid? SubmittedBy { get; set; }
		public Guid? ApprovedBy { get; set; }
		public DateTime? ApprovedDate { get; set; }
	}
}
