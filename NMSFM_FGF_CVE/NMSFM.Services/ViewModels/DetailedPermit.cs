using NMSFM.Services.Models;
using System;
using System.Collections.Generic;
using AutoMapper;
using System.Linq;
using System.Web;

namespace NMSFM.ViewModels
{
	public class DetailedPermit
	{		
		public Guid PermitId { get; set; }
		public Guid? PermitTypeId { get; set; }
		public string PermitNumber { get; set; }
		public DateTime? BeginDate { get; set; }
		public DateTime? EndDate { get; set; }		
		public string Comment { get; set; }
		public Guid? AddressId { get; set; }
		public Guid? RecordId { get; set; }		
		public string PermitType { get; set; }
		public Guid? IssuedToPartyId { get; set; }
		//public Guid rowguid { get; set; }		
		public bool Complete { get; set; }
		public Guid? ApprovalStep { get; set; }
		public Guid? AgencyId { get; set; }		
		public string ExternalId { get; set; }
		public Guid? ContactId { get; set; }		
		public string PropConst { get; set; }
		public Guid? OwnerId { get; set; }
		public Guid? ContractorId { get; set; }
		public bool? FromWeb { get; set; }
		public Guid? PermitStatusId { get; set; }
		public Guid? ParentPermitId { get; set; }		
		public DateTime? SubmittalDate { get; set; }
		public Guid? IssuedToRoleTypeId { get; set; }
		public Guid? ContactRoleTypeId { get; set; }
		public Guid? IssuingOfficerId { get; set; }
		public Guid? ReportId { get; set; }
		public Guid? CertReportId { get; set; }
		public Guid? LandCertReportId { get; set; }
		public Guid? ALReportId { get; set; }
		public Guid? OccupancyTypeId { get; set; }
		public Guid? PropertyUseTypeId { get; set; }
		//public DateTime DateUpdated { get; set; }
		//public DateTime DateInserted { get; set; }
		public bool StopAlerts { get; set; }
		public Guid? ItemId { get; set; }

		public string Item { get; set; }
		public string LegalDesc { get; set; }		
		public bool? SignOffComplete { get; set; }		
		public PermitAddress AddressDisplay { get; set; }
		public List<UserDefinedValue> UserValues { get; set; }
		public IEnumerable<AttachedImages> AttachedImages { get; set; }
		public IEnumerable<AttachedImages> AttachedPdfs { get; set; }
		public IEnumerable<AttachedFees> AttachedFees { get; set; }
	}
}