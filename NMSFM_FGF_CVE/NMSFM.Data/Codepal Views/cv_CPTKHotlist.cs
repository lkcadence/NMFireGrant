using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace NMSFM.Data
{

	public partial class cv_CPTKHotlist
	{
		[Key]
		public Guid AddressId { get; set; }

		[StringLength(220)]
		public string FullAddress { get; set; }

		[StringLength(50)]
		public string City { get; set; }

		[StringLength(5)]
		public string StateAbbr { get; set; }

		[StringLength(15)]
		public string Zip { get; set; }

		public DateTime DateAdded { get; set; }
	}
}
