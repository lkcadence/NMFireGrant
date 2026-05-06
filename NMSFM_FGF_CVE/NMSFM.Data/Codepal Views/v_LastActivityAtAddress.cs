namespace NMSFM.Data
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	public partial class v_LastActivityAtAddress
	{
		[Key]
		[Column(Order = 0)]
		public Guid? AddressId { get; set; }

		public DateTime? InspectionDate { get; set; }

		public Guid? ACAgencyId { get; set; }

		public Guid? ActivityTypeId { get; set; }

		public Guid? InspectionTypeId { get; set; }

		public Guid? StateId { get; set; }

		public Guid? ZipId { get; set; }

		public Guid? RegionId { get; set; }

		public Guid? CountyId { get; set; }

		public Guid? OccupancyTypeId { get; set; }

		public Guid? PropertyUseTypeId { get; set; }

		[StringLength(15)]
		public string Zip { get; set; }

		[StringLength(50)]
		public string City { get; set; }

		[StringLength(50)]
		public string SubAddress { get; set; }

		[StringLength(50)]
		public string Address { get; set; }

		[StringLength(50)]
		public string AddressNumber { get; set; }

		[StringLength(100)]
		public string InspectionType { get; set; }

		[StringLength(50)]
		public string ActivityType { get; set; }

		[StringLength(204)]
		public string FullAddress { get; set; }

		[StringLength(50)]
		public string AddressCode { get; set; }

		[StringLength(50)]
		public string Region { get; set; }

		[StringLength(50)]
		public string County { get; set; }

		[StringLength(50)]
		public string OccupancyType { get; set; }

		[StringLength(50)]
		public string PropertyUseType { get; set; }

		[StringLength(5)]
		public string StateAbbr { get; set; }

		[StringLength(50)]
		public string Direction { get; set; }

		[StringLength(15)]
		public string Suffix { get; set; }

		public Guid? AddressTypeId { get; set; }

		public bool? ActInactive { get; set; }

		public bool? TypeInactive { get; set; }

		public bool? Inactive { get; set; }
	}
}
