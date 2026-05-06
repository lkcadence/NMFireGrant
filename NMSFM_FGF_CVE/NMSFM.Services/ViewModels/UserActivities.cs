using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NMSFM.ViewModels
{
	public class UserActivities
	{
		public Guid InspectorId { get; set; }
		public string InspectorName { get; set; }
		public bool isAHJ { get; set; }
		public IEnumerable<AttachedActivities> AttachedActivities { get; set; }		
	}
}