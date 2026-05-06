using System;
using System.Collections.Generic;
using NMSFM.Services.Models;
using System.ComponentModel.DataAnnotations;
using NMSFM.Data;

namespace NMSFM.ViewModels
{
	public class DetailedActivity
	{
		public Guid InspectionId { get; set; }

		public Guid? InspectionCauseId { get; set; }

		public Guid? RecordId { get; set; }

		public Guid? AddressId { get; set; }
		public string FullAddress { get; set; }

		public Guid? InspectorId { get; set; }

		public Guid? InspectedPartyId { get; set; }

		public DateTime? InspectionDate { get; set; }

		public DateTime? ReInspectionDate { get; set; }

		public Guid? InspectionTypeId { get; set; }

		[StringLength(50)]
		public string InspectionNumber { get; set; }

		public decimal? Hrs { get; set; }

		public bool Complete { get; set; }

		public Guid rowguid { get; set; }

		public Guid? AlternatePartyId { get; set; }

		public Guid? HasDefaultFeeId { get; set; }

		[StringLength(100)]
		public string ExternalId { get; set; }

		public Guid? ParentInspectionId { get; set; }

		public Guid? ItemId { get; set; }
		public string Item { get; set; }

		public Guid? ItemInspectionStatusId { get; set; }

		public Guid? ActivityTypeId { get; set; }

		public bool PrimaryParty { get; set; }

		public Guid? OccupancyTypeId { get; set; }

		public Guid? PropertyUseTypeId { get; set; }

		public Guid? ApprovalStep { get; set; }

		public DateTime? ScheduledDate { get; set; }

		public bool? LockActTime { get; set; }

		public Guid? RoutingSlipId { get; set; }

		[StringLength(5000)]
		public string ActivitySummary { get; set; }

		public DateTime? EndDate { get; set; }

		public DateTime? StartDate { get; set; }

		public bool? FromWeb { get; set; }

		public Guid? SecondaryInspectorId { get; set; }

		public Guid? AcGroupId { get; set; }
		public string GroupName { get; set; }

		public int NewViolations { get; set; }

		public int OldViolations { get; set; }

		public int CorrectedViolations { get; set; }

		public int ViolationCounts { get; set; }

		public Guid? InspectedPartyRoleTypeId { get; set; }

		public Guid? AlternatePartyRoleTypeId { get; set; }

		public bool dummyAgreement { get; set; }

		public Guid? InvNarrativeId { get; set; }

		public Guid? SecAddressId { get; set; }
		public string SecAddress { get; set; }

		public Guid? ReportId { get; set; }

		public Guid? DefReportId { get; set; }

		public Guid? SubDefReportId { get; set; }

		[StringLength(50)]
		public string ExternalValue { get; set; }

		public DateTime DateUpdated { get; set; }

		public DateTime DateInserted { get; set; }

		public int SubViolations { get; set; }

		public int OldSubViolations { get; set; }

		public bool FollowUp { get; set; }
		public string Comment { get; set; }
		public string ProjectNumber { get; set; }
		public List<CheckItemModel> CheckItems { get; set; }

		public List<UserDefinedValue> UserValues { get; set; }		

		public ActivitySetting DisplaySettings { get; set; }

	}
}