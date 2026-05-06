using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NMSFM.Data
{
	public partial class FYAppData
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[DefaultValue("newid()")]
		[Key]
		public Guid FYAppDataId { get; set; }

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long FYAppDataIdentity { get; set; }

		[DefaultValue("YEAR(getdate())")]
		public short Year { get; set; }
		public string Dist1Comm { get; set; }
		public string Dist2Comm { get; set; }
		public string Dist3Comm { get; set; }
		public string Dist4Comm { get; set; }
		public string Dist5Comm { get; set; }
		public string Governor { get; set; }
		public string CabinetSec { get; set; }
		public string DeputyCabinetSec { get; set; }
		public string FireMarshalName { get; set; }
		public string ChiefofStaffTitle { get; set; }
		public string ChiefofStaffName { get; set; }
		public string ApplicationDueDate { get; set; }
		public string FPFRollOverSubmittalDueDate { get; set; }		


	}
}
