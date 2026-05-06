using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NMSFM.Data
{
	public partial class v_UserDefValues
	{
		public Guid? UserDefCategoryId { get; set; }

		public Guid? UserDefFieldId { get; set; }

		public string FieldDesc { get; set; }

		public int? SeqNum { get; set; }

		public bool? FieldEncrypted { get; set; }

		public bool? StaticCombo { get; set; }
		[Key]
		[Column(Order = 0)]
		public Guid? UserDefValueId { get; set; }

		public string UserDefValue { get; set; }

		public Guid? RecordId { get; set; }

		public Guid? UserDefTypeId { get; set; }

		public string UserDefType { get; set; }

		public bool? AllModuleTypes { get; set; }

		public string Category { get; set; }

		public Guid? ModuleId { get; set; }

		public Guid? ModuleTypeId { get; set; }

		public int? CategorySeqNum { get; set; }

		public Guid? AgencyId { get; set; }

		public string AllAgency { get; set; }

		public bool? WebViewable { get; set; }

	}
}
