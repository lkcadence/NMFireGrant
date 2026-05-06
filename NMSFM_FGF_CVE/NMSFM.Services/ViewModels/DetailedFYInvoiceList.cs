using System;
using System.Collections.Generic;
using NMSFM.Data;
using NMSFM.ViewModels;

namespace NMSFM.ViewModels
{
	public partial class DetailedFYInvoiceList
	{

		public short Year { get; set; }	
		public List<DetailedFYInvoice> InvoiceList { get; set; }		
	}

	

}
