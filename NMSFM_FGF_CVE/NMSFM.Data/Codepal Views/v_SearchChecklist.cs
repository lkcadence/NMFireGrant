
namespace NMSFM.Data
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;
	public partial class v_SearchChecklist
	{
		[StringLength(50)]
		public string CheckListName { get; set; }

		public short? SeqNum { get; set; }
		[Key]
		[Column(Order = 1)]
		[StringLength(1000)]
		public string CheckItem { get; set; }

		public short? CheckListOrder { get; set; }

		[StringLength(3000)]
		public string TextValue { get; set; }

		public short? BooleanValue { get; set; }

		[StringLength(2000)]
		public string ResolutionText { get; set; }

		public DateTime? Corrected { get; set; }
		[Key]
		[Column(Order = 0)]
		public Guid InspectionlId { get; set; }

		public Guid InspectionDetailId { get; set; }

		[StringLength(100)]
		public string FailValue { get; set; }
	}
}
