using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMSFM.Data;
using NMSFM.ViewModels;

namespace NMSFM.Services.Models
{
	public class CheckItemModel
	{
		public Guid ActivityId { get; set; }
		public Guid CheckListId { get; set; }
		public string CheckListName { get; set; }
		public int CheckListOrder { get; set; }
		public Guid? CheckListTypeId { get; set; }
		public Guid CheckItemId { get; set; }
		public Guid CheckItemTypeId { get; set; }
		public string CheckItem { get; set; }
		public int CheckItemOrder { get; set; }
		public Guid CheckItemValueId { get; set; }
		public string TextValue { get; set; }
		public byte? BooleanValue { get; set; }
		public string ResolutionText { get; set; }
		public DateTime? Corrected { get; set; }
		public DateTime? CorrectedInspectionId { get; set; }
		public Guid? InspectionDetailId { get; set; }
		public string InfoLine { get; set; }
		public string DefaultValue { get; set; }
		public string FailValue { get; set; }
		public bool Required { get; set; }
		public bool FailsCheckList { get; set; }
		public bool HideNA { get; set; }
		public bool HideNO { get; set; }
		public bool StaticList { get; set; }
		public bool HideAddRef { get; set; }
		public bool DefaultLastValues { get; set; }		
		public PrevCheckItem LastValues { get; set; }
		public List<SelectListItem> Resolutions { get; set; }
		public List<bool> CheckBoxValues { get; set; }
		public List<SelectListItem> Violations { get; set; }
	}
}
