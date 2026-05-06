using NMSFM.Services.Models;
using NMSFM.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NMSFM.ViewModels
{
	public class AttachAddressParty
	{
		public Guid AddressId { get; set; }
		public Guid AddressPartyId { get; set; }
		public Guid PartyId { get; set; }		
		public string PartyName { get; set; }
		[EmailAddress]
		public string Email { get; set; }
		public List<Phone> PhoneList { get; set; }
		public string RoleType { get; set; }
		public Guid? RoleTypeId { get; set; }
		public string Comment { get; set; }
		public string Salutation { get; set; }
		public string FirstName { get; set; }
		public string MiddleInitial { get; set; }
		public string LastName { get; set; }
		public string Suffix { get; set; }
		public string PartyType {get; set; }
		public IEnumerable<SearchParty> SearchParties { get; set; }
	}
}