using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMSFM.ViewModels
{
    public partial class SearchGrantApps
    {
		public Guid? FireGranAppId { get; set; }
		//From Address
		public Guid AddressId { get; set; }
		public string DepartmentName { get; set; }
		public short Year { get; set; }
		public string County { get; set; }
		//From ApplicationGeneralInfo table
		public string ConfirmationNumber { get; set; }
		public DateTime DateSubmitted { get; set; }
		public string ApplicationStatus { get; set; }
		public DateTime LastUpdateDate { get; set; }
	}
}
