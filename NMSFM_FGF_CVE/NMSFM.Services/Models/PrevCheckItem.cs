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
	public class PrevCheckItem
	{
		public string TextValue { get; set; }
		public byte? BooleanValue { get; set; }
		public string ResolutionText { get; set; }
	}
}
