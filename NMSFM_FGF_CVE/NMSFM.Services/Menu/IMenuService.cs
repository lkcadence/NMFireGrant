using System.Collections.Generic;
using System.Threading.Tasks;
using NMSFM.Data;
using System;
using System.Web.Mvc;
using NMSFM.Services.Models;
using NMSFM.ViewModels;
	 
namespace NMSFM.Services.Menu
{
		public interface IMenuService
	{
		Task<string> GetMainMenu(Guid? AgencyId);
		Task<string> GetITRSMenu(Guid? AgencyId);
		Task<string> GetPreplanViewerMenu(Guid? AgencyId);
		Task<string> GetPreplanEditorMenu(Guid? AgencyId);
		Task<string> GetCodepalFormsMenu(Guid? AgencyId);
		Task<string> GetCodepalMenu(Guid? AgencyId);		
	}
}
