namespace NMSFM.Data
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	[Serializable]
	public partial class User
	{
		public Guid UserId { get; set; }

		[StringLength(50)]
		public string Login { get; set; }

		[StringLength(500)]
		public string Password { get; set; }

		[StringLength(256)]
		public string Email { get; set; }

		[StringLength(500)]
		public string ConnectionString { get; set; }

		public DateTime DateUpdated { get; set; }

		public DateTime DateInserted { get; set; }

		public Guid? CodepalId { get; set; }

		public bool? IsWebAdmin { get; set; }

		public Guid? LastSessionId { get; set; }

		public bool? Inactive { get; set; }

		public DateTime? LastLoginDate { get; set; }

		[StringLength(100)]
		public string DatabaseName { get; set; }

		[StringLength(150)]
		public string Organization { get; set; }

		public Guid? AgencyId { get; set; }

		public bool AHJUser { get; set; }

		public bool ClientUser { get; set; }

		public bool ThirdPartyUser { get; set; }

		public bool PublicUser { get; set; }

		public bool Readonly { get; set; }

		public bool CPTK { get; set; }

		public bool NMFPF { get; set; }

		public bool NMFGA { get; set; }

		public bool NMFGC { get; set; }
		public Guid? ForgotPasswordToken { get; set; }
	}
}
