using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace NMSFM.ViewModels
{
	public class AttachedSignature
	{        
		public Guid SignatureId { get; set; }
		public string PrintedName { get; set; }
		public int Sequence { get; set; }
		public Guid SignatureTypeId { get; set; }       
	}
}