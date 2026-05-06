using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NMSFM.Data;
using System.Web.Mvc;

namespace NMSFM.ViewModels
{
	[Serializable]
	public class UserList
	{
		public User Admin { get; set; }

		[StringLength(50)]
		[Required(ErrorMessage = "Please enter a username.")]
		public string WebLogin { get; set; }
		[StringLength(500)]
		[Required(ErrorMessage = "Please enter a password.")]
		public string WebPassword { get; set; }

		[Required(ErrorMessage = "Please select a Codepal user.")]
		public Guid WebUserId { get; set; }
		public List<SelectListItem> InspectorList { get; set; }
		public List<SelectListItem> PartyList { get; set; }
		public List<ExistingUser> ExistingUserList { get; set; }
	}
}