using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMSFM.Data;

namespace NMSFM.Services.Models
{
	public class UserDefinedValue
	{
		public string Category { get; set; }
		public Guid CategoryId { get; set; }
		public string FieldDescription { get; set; }
		public string FieldValue { get; set; }
		public string FieldOldValue { get; set; }
		public List<bool> boolValue { get; set; }
		public Guid ValueId { get; set; }
		public Guid FieldId { get; set; }
		public Guid FieldType { get; set; }
		public int SequenceNumber { get; set; }
		public int FieldSequenceNumber { get; set; }
		public bool WebViewable { get; set; }
		public bool StaticCombo { get; set; }
		public bool Required { get; set; }
		public string DefaultValue { get; set; }
		public bool Persistent { get; set; }
		public string PersistentValue { get; set; }
		public Guid ResolutionId { get; set; }
		public List<Resolution> Resolutions { get; set; }
	}
}
