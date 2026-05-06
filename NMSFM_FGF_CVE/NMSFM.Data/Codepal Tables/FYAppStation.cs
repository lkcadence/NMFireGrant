using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NMSFM.Data
{
	public partial class FYAppStation
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[DefaultValue("newid()")]
		[Key]
		public Guid FYAppStationsId { get; set; }
		
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long FYAppStationsIdentity { get; set; }

		[DefaultValue("YEAR(getdate())")] 
		public short Year { get; set; }
		public Guid DepartmentAddressId { get; set; }
		public Guid AddressTypeId { get; set; }
		public Guid AddressId { get; set; }
		public string AddressCode { get; set; }
		public string AddressNumber { get; set; }
		public string Direction { get; set; }
		public string Address { get; set; }
		public string Suffix { get; set; }
		public string SubAddress { get; set; }
		public string City { get; set; }
		public string StateAbbr { get; set; }
		public string Zip { get; set; }
		public string AddDesc { get; set; }

	}
}
