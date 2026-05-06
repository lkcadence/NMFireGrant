using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMSFM.ViewModels
{	
	/// <summary>
	/// This view model can be used for different types of combos.
	/// </summary>
	public class ComboViewModel
	{
		public Guid? ValueId { get; set; }
		public string Value { get; set; }
		public string Text { get; set; }

	}
}
