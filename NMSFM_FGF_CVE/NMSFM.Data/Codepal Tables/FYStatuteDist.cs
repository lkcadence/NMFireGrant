using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NMSFM.Data
{
	public partial class FYStatuteDist
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[DefaultValue("newid()")]
		[Key]		
		public Guid FYStatuteDistId { get; set; }

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long FYStatuteDistIdentity { get; set; }

		public short Year { get; set; }
		public short ISOClass { get; set; }
		
		public decimal MSBaseAmount { get; set; }
		
		public decimal SSBaseAmount { get; set; }


	}
}
