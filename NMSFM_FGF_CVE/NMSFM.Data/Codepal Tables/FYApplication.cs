using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;

namespace NMSFM.Data
{
	public partial class FYApplication
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[DefaultValue("newid()")]
		[Key]
		public Guid FYApplicationsId { get; set; }

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long FYApplicationsIdentity { get; set; }

		[DefaultValue("YEAR(getdate())")]
		public short Year { get; set; }
		public Guid AddressId { get; set; }
		public string ISOContendSig { get; set; }
		public decimal? FPFBalance { get; set; }
		public decimal? FPFRollOverAmount { get; set; }
		public string FPFRollOverDescription { get; set; }
		public Guid NFIRSContact1Id { get; set; }
		public Guid NFIRSContact2Id { get; set; }
		public string AppDaySubmitted { get; set; }
		public string AppMonthSubmitted { get; set; }
		public string GovOffElectronicSig { get; set; }
		public string ChiefElectronicSig { get; set; }
		public bool Complete { get; set; }
		public Guid? CompletedBy { get; set; }

		public short ISOClass { get; set; }
		public int MainStationCount { get; set; }
		public int AdminBldgCount { get; set; }
		public int SubStationCount { get; set; }
		public bool Approved { get; set; }
		public DateTime? DateApproved { get; set; }
		public Guid? ApprovedBy { get; set; }

	}
}
