using NMSFM.Data;
using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using NMSFM.Services.Models;
using NMSFM.Services.Audit;
using log4net;
using NMSFM.Services.Logging;
using NMSFM.Services.CPSystem;
using System.Security.Cryptography;
using NMSFM.ViewModels;
using AutoMapper;
using System.Web;
using System.Configuration;

namespace NMSFM.Services.Menu
{
	public class MenuService : IMenuService
	{
		private ICodepalWebModel cwmContext;
		private ILogging logger;
		public MenuService(ICodepalWebModel codepalWebModel, ILogging codepalLogger)
		{
			cwmContext = codepalWebModel;
			logger = codepalLogger;
		}

		public async Task<string> GetMainMenu(Guid? AgencyId)
		{
			string result;
			//ConfigurationManager.AppSettings["RootDomain"]
			//HttpContext.Current.Session["CodepalUserId"]
			//ConfigurationManager.AppSettings["RootDomain"];
			try
			{				
				result = "<li><a id='preplanViewer' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/PreplanViewer/PVAddress/Search'CPTKPV><i class='glyphicon glyphicon-eye-open'></i>Preplan Viewer</a></li>" + Environment.NewLine +
						 "<li CPLIHL><a id='addressHotList' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/PreplanViewer/PVAddress/Hotlist'CPTKHL><i class='glyphicon glyphicon-eye-open'></i>Address Hotlist</a></li>" + Environment.NewLine +
						 "<li CPLIPE><a id='preplanEditor' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/PreplanEditor/PEAddress/Search?alternateView=asf'CPTKPE><i class='glyphicon glyphicon-open-file'></i>Preplan Editor</a></li>" + Environment.NewLine +
						 "<li CPLIIT><a id='ITRS' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/ITRS/ITRSActivity/ActivityList?userId=" + HttpContext.Current.Session["CodepalUserId"] + "'CPTKITRS><i class='glyphicon glyphicon-duplicate'></i>I.T.R.S.</a></li>" + Environment.NewLine +
						 "<li CPLICF><a id='permitApps' href='#permitsSubmenu' data-toggle='collapse' aria-expanded='false'CPTKCF><i class='glyphicon glyphicon-file'></i>Applications</a>" + Environment.NewLine +
						 "<ul id='permitsSubmenu' class='collapse list-unstyled'>" + Environment.NewLine;
				//"<li><a id='myPR' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/CodepalForms/CFPermit/LoadQuicklinkPermitSearch?PermitTypeId=91d0b06c-4970-450f-a266-7684274cad6c&agencyId=9808204f-d941-451e-b121-02c8a0d7e7fa' class='btn bg-primary'><i class='glyphicon glyphicon-file'></i>Firework Stand</a></li><li><a id='myInv' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/CodepalForms/CFPermit/LoadQuicklinkPermitSearch?PermitTypeId=74822e11-3cdb-4298-a591-930c1a367f3a&agencyId=9808204f-d941-451e-b121-02c8a0d7e7fa' class='btn bg-primary'><i class='glyphicon glyphicon-file'></i>Fireworks Show</a></li>" 
				result += GetCFPermitSublist(AgencyId);
				result += "</ul></li> " + Environment.NewLine;

				result += "<li CPLICP><a id='activityReq' href='#CodepalSubMenu' data-toggle='collapse' aria-expanded='false'CPTKCP><i class='glyphicon glyphicon-copy'></i>Codepal</a>" + Environment.NewLine +
						  "<ul id='CodepalSubMenu' class='collapse list-unstyled'>" + Environment.NewLine +
						  "<li><a id='addSearch' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/Codepal/CPAddress/Search?alternateView=asf' class='btn bg-primary'><i class='glyphicon glyphicon-map-marker'></i>Address Search</a></li>" + Environment.NewLine +
						  "<li><a id='myAct' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/Codepal/CPActivity/ActivityList?userId=" + HttpContext.Current.Session["CodepalUserId"] + "' class='btn bg-primary'><i class='glyphicon glyphicon-copy'></i>My Activity List</a></li>" + Environment.NewLine;

				result += GetCPActivitySublist(AgencyId);

				result += "</ul></li>" + Environment.NewLine;

				result = await DoPurchased(result, AgencyId);
			}
			catch (Exception)
			{
				//HandleError(ex, Name, "GetITRSMenu");
				result = "";
			}
			return result;
		}

		public async Task<string> GetITRSMenu(Guid? AgencyId)
		{
			string result;

			try
			{
				result = "<li><a id='preplanViewer' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/PreplanViewer/PVAddress/Search'CPTKPV><i class='glyphicon glyphicon-eye-open'></i>Preplan Viewer</a></li>" + Environment.NewLine + Environment.NewLine +
						 "<li CPLIHL><a id='addressHotList' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/PreplanViewer/PVAddress/Hotlist'CPTKHL><i class='glyphicon glyphicon-eye-open'></i>Address Hotlist</a></li>" + Environment.NewLine + Environment.NewLine +
						 "<li CPLIPE><a id='preplanEditor' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/PreplanEditor/PEAddress/Search?alternateView=asf'CPTKPE><i class='glyphicon glyphicon-open-file'></i>Preplan Editor</a></li>" + Environment.NewLine + Environment.NewLine +
						 "<li CPLIIT><a id='ITRS'href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/ITRS/ITRSActivity/ActivityList?userId=" + HttpContext.Current.Session["CodepalUserId"] + "' style='background-color:dodgerblue'CPTKITRS><i class='glyphicon glyphicon-duplicate'></i>I.T.R.S.</a></li>" + Environment.NewLine + Environment.NewLine +
						 "<li CPLICF><a id='permitApps' href='#permitsSubmenu' data-toggle='collapse' aria-expanded='false'CPTKCF><i class='glyphicon glyphicon-file'></i>Applications</a>" + Environment.NewLine +
						 "<ul id='permitsSubmenu' class='collapse list-unstyled'>" + Environment.NewLine;
				//<li><a id='myPR' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/CodepalForms/CFPermit/LoadQuicklinkPermitSearch?PermitTypeId=91d0b06c-4970-450f-a266-7684274cad6c&agencyId=9808204f-d941-451e-b121-02c8a0d7e7fa' class='btn bg-primary'><i class='glyphicon glyphicon-file'></i>Firework Stand</a></li><li><a id='myInv' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/CodepalForms/CFPermit/LoadQuicklinkPermitSearch?PermitTypeId=74822e11-3cdb-4298-a591-930c1a367f3a&agencyId=9808204f-d941-451e-b121-02c8a0d7e7fa' class='btn bg-primary'><i class='glyphicon glyphicon-file'></i>Fireworks Show</a></li></ul></li>" +
				result += GetCFPermitSublist(AgencyId);
				result += "</ul></li> " + Environment.NewLine;
				result += "<li CPLICP><a id='activityReq' href='#CodepalSubMenu' data-toggle='collapse' aria-expanded='false'CPTKCP><i class='glyphicon glyphicon-copy'></i>Codepal</a>" + Environment.NewLine +
						  "<ul id='CodepalSubMenu' class='collapse list-unstyled'>" + Environment.NewLine +
						  "<li><a id='addSearch' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/Codepal/CPAddress/Search?alternateView=asf' class='btn bg-primary'><i class='glyphicon glyphicon-map-marker'></i>Address Search</a></li>" + Environment.NewLine +
						  "<li><a id='myAct' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/Codepal/CPActivity/ActivityList?userId=" + HttpContext.Current.Session["CodepalUserId"] + "' class='btn bg-primary'><i class='glyphicon glyphicon-copy'></i>My Activity List</a></li>" + Environment.NewLine;
				//<li><a id='myCVI' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/Codepal/CPActivity/AddressForActivityRecord?ActivityTypeId=6dea84c2-f24a-460d-8d03-c89a0a69f988&InspectionTypeId=1a4575e3-a81c-417d-8614-77fc15ec92a4' class='btn bg-primary'><i class='glyphicon glyphicon-copy'></i>Company Inspection</a></li><li><a id='myPR' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/Codepal/CPActivity/AddressForActivityRecord?ActivityTypeId=5933809e-4517-4ef7-b21d-37141304b6e4&InspectionTypeId=e8194533-8b57-472f-aacf-fffef3f59808' class='btn bg-primary'><i class='glyphicon glyphicon-copy'></i>Plan Review</a></li><li><a id='myInv' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/Codepal/CPActivity/AddressForActivityRecord?ActivityTypeId=82b589a9-f5d8-4a1c-9728-cba1dcd466ab&InspectionTypeId=d34abd74-d54f-4acd-b268-a56d4f2845e4' class='btn bg-primary'><i class='glyphicon glyphicon-copy'></i>Investigation</a></li></ul></li>";
				result += GetCPActivitySublist(AgencyId);

				result += "</ul></li>" + Environment.NewLine;

				result = await DoPurchased(result, AgencyId);
			}
			catch (Exception)
			{
				//HandleError(ex, Name, "GetITRSMenu");
				result = "";
			}

			return result;
		}

		public async Task<string> GetPreplanViewerMenu(Guid? AgencyId)
		{
			string result;

			try
			{
				result = "<li><a id='preplanViewer' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/PreplanViewer/PVAddress/Search' style='background-color:dodgerblue'CPTKPV><i class='glyphicon glyphicon-eye-open'></i>Preplan Viewer</a></li>" + Environment.NewLine +
						 "<li CPLIHL><a id='addressHotList' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/PreplanViewer/PVAddress/Hotlist'CPTKHL><i class='glyphicon glyphicon-eye-open'></i>Address Hotlist</a></li>" + Environment.NewLine +
						 "<li CPLIPE><a id='preplanEditor' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/PreplanEditor/PEAddress/Search?alternateView=asf'CPTKPE><i class='glyphicon glyphicon-open-file'></i>Preplan Editor</a></li>" + Environment.NewLine +
						 "<li CPLIIT><a id='ITRS' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/ITRS/ITRSActivity/ActivityList?userId=" + HttpContext.Current.Session["CodepalUserId"] + "'CPTKITRS><i class='glyphicon glyphicon-duplicate'></i>I.T.R.S.</a></li>" + Environment.NewLine +
						 "<li CPLICF><a id='permitApps' href='#permitsSubmenu' data-toggle='collapse' aria-expanded='false'CPTKCF><i class='glyphicon glyphicon-file'></i>Applications</a><ul id='permitsSubmenu' class='collapse list-unstyled'>" + Environment.NewLine;

				//<li><a id='myPR' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/CodepalForms/CFPermit/LoadQuicklinkPermitSearch?PermitTypeId=91d0b06c-4970-450f-a266-7684274cad6c&agencyId=9808204f-d941-451e-b121-02c8a0d7e7fa' class='btn bg-primary'><i class='glyphicon glyphicon-file'></i>Firework Stand</a></li><li><a id='myInv' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/CodepalForms/CFPermit/LoadQuicklinkPermitSearch?PermitTypeId=74822e11-3cdb-4298-a591-930c1a367f3a&agencyId=9808204f-d941-451e-b121-02c8a0d7e7fa' class='btn bg-primary'><i class='glyphicon glyphicon-file'></i>Fireworks Show</a></li></ul></li>" +
				result += GetCFPermitSublist(AgencyId);
				result += "</ul></li> " + Environment.NewLine;

				result += "<li CPLICP><a id='activityReq' href='#CodepalSubMenu' data-toggle='collapse' aria-expanded='false'CPTKCP><i class='glyphicon glyphicon-copy'></i>Codepal</a>" + Environment.NewLine +
						  "<ul id='CodepalSubMenu' class='collapse list-unstyled'>" + Environment.NewLine +
						  "<li><a id='addSearch' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/Codepal/CPAddress/Search?alternateView=asf' class='btn bg-primary'><i class='glyphicon glyphicon-map-marker'></i>Address Search</a></li>" + Environment.NewLine +
						  "<li><a id='myAct' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/Codepal/CPActivity/ActivityList?userId=" + HttpContext.Current.Session["CodepalUserId"] + "' class='btn bg-primary'><i class='glyphicon glyphicon-copy'></i>My Activity List</a></li>" + Environment.NewLine;
				//<li><a id='myCVI' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/Codepal/CPActivity/AddressForActivityRecord?ActivityTypeId=6dea84c2-f24a-460d-8d03-c89a0a69f988&InspectionTypeId=1a4575e3-a81c-417d-8614-77fc15ec92a4' class='btn bg-primary'><i class='glyphicon glyphicon-copy'></i>Company Inspection</a></li><li><a id='myPR' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/Codepal/CPActivity/AddressForActivityRecord?ActivityTypeId=5933809e-4517-4ef7-b21d-37141304b6e4&InspectionTypeId=e8194533-8b57-472f-aacf-fffef3f59808' class='btn bg-primary'><i class='glyphicon glyphicon-copy'></i>Plan Review</a></li><li><a id='myInv' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/Codepal/CPActivity/AddressForActivityRecord?ActivityTypeId=82b589a9-f5d8-4a1c-9728-cba1dcd466ab&InspectionTypeId=d34abd74-d54f-4acd-b268-a56d4f2845e4' class='btn bg-primary'><i class='glyphicon glyphicon-copy'></i>Investigation</a></li></ul></li>";
				result += GetCPActivitySublist(AgencyId);

				result += "</ul></li>" + Environment.NewLine;

				result = await DoPurchased(result, AgencyId);
			}
			catch (Exception)
			{
				//HandleError(ex, Name, "GetITRSMenu");
				result = "";
			}
			return result;
		}

		public async Task<string> GetPreplanEditorMenu(Guid? AgencyId)
		{
			string result;

			try
			{
				result = "<li><a id='preplanViewer' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/PreplanViewer/PVAddress/Search'CPTKPV><i class='glyphicon glyphicon-eye-open'></i>Preplan Viewer</a></li>" + Environment.NewLine +
						 "<li CPLIHL><a id='addressHotList' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/PreplanViewer/PVAddress/Hotlist'CPTKHL><i class='glyphicon glyphicon-eye-open'></i>Address Hotlist</a></li>" + Environment.NewLine +
						 "<li CPLIPE><a id='preplanEditor' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/PreplanEditor/PEAddress/Search?alternateView=asf' style='background-color:dodgerblue'CPTKPE><i class='glyphicon glyphicon-open-file'></i>Preplan Editor</a></li>" + Environment.NewLine +
						 "<li CPLIIT><a id='ITRS' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/ITRS/ITRSActivity/ActivityList?userId=" + HttpContext.Current.Session["CodepalUserId"] + "'CPTKITRS><i class='glyphicon glyphicon-duplicate'></i>I.T.R.S.</a></li>" + Environment.NewLine +
						 "<li CPLICF><a id='permitApps' href='#permitsSubmenu' data-toggle='collapse' aria-expanded='false'CPTKCF><i class='glyphicon glyphicon-file'></i>Applications</a><ul id='permitsSubmenu' class='collapse list-unstyled'>" + Environment.NewLine;

				//<li><a id='myPR' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/CodepalForms/CFPermit/LoadQuicklinkPermitSearch?PermitTypeId=91d0b06c-4970-450f-a266-7684274cad6c&agencyId=9808204f-d941-451e-b121-02c8a0d7e7fa' class='btn bg-primary'><i class='glyphicon glyphicon-file'></i>Firework Stand</a></li><li><a id='myInv' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/CodepalForms/CFPermit/LoadQuicklinkPermitSearch?PermitTypeId=74822e11-3cdb-4298-a591-930c1a367f3a&agencyId=9808204f-d941-451e-b121-02c8a0d7e7fa' class='btn bg-primary'><i class='glyphicon glyphicon-file'></i>Fireworks Show</a></li></ul></li>" +
				result += GetCFPermitSublist(AgencyId);
				result += "</ul></li> " + Environment.NewLine;
				result += "<li CPLICP><a id='activityReq' href='#CodepalSubMenu' data-toggle='collapse' aria-expanded='false'CPTKCP><i class='glyphicon glyphicon-copy'></i>Codepal</a>" + Environment.NewLine +
						  "<ul id='CodepalSubMenu' class='collapse list-unstyled'>" + Environment.NewLine +
						  "<li><a id='addSearch' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/Codepal/CPAddress/Search?alternateView=asf' class='btn bg-primary'><i class='glyphicon glyphicon-map-marker'></i>Address Search</a></li>" + Environment.NewLine +
						  "<li><a id='myAct' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/Codepal/CPActivity/ActivityList?userId=" + HttpContext.Current.Session["CodepalUserId"] + "' class='btn bg-primary'><i class='glyphicon glyphicon-copy'></i>My Activity List</a></li>" + Environment.NewLine;
				//<li><a id='myCVI' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/Codepal/CPActivity/AddressForActivityRecord?ActivityTypeId=6dea84c2-f24a-460d-8d03-c89a0a69f988&InspectionTypeId=1a4575e3-a81c-417d-8614-77fc15ec92a4' class='btn bg-primary'><i class='glyphicon glyphicon-copy'></i>Company Inspection</a></li><li><a id='myPR' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/Codepal/CPActivity/AddressForActivityRecord?ActivityTypeId=5933809e-4517-4ef7-b21d-37141304b6e4&InspectionTypeId=e8194533-8b57-472f-aacf-fffef3f59808' class='btn bg-primary'><i class='glyphicon glyphicon-copy'></i>Plan Review</a></li><li><a id='myInv' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/Codepal/CPActivity/AddressForActivityRecord?ActivityTypeId=82b589a9-f5d8-4a1c-9728-cba1dcd466ab&InspectionTypeId=d34abd74-d54f-4acd-b268-a56d4f2845e4' class='btn bg-primary'><i class='glyphicon glyphicon-copy'></i>Investigation</a></li></ul></li>";
				result += GetCPActivitySublist(AgencyId);

				result += "</ul></li>";

				result = await DoPurchased(result, AgencyId);
			}
			catch (Exception)
			{
				//HandleError(ex, Name, "GetITRSMenu");
				result = "";
			}
			return result;
		}

		public async Task<string> GetCodepalFormsMenu(Guid? AgencyId)
		{
			string result;

			try
			{
				result = "<li><a id='preplanViewer' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/PreplanViewer/PVAddress/Search'CPTKPV><i class='glyphicon glyphicon-eye-open'></i>Preplan Viewer</a></li>" + Environment.NewLine +
						 "<li CPLIHL><a id='addressHotList' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/PreplanViewer/PVAddress/Hotlist'CPTKHL><i class='glyphicon glyphicon-eye-open'></i>Address Hotlist</a></li>" + Environment.NewLine +
						 "<li CPLIPE><a id='preplanEditor' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/PreplanEditor/PEAddress/Search?alternateView=asf'CPTKPE><i class='glyphicon glyphicon-open-file'></i>Preplan Editor</a></li>" + Environment.NewLine +
						 "<li CPLIIT><a id='ITRS' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/ITRS/ITRSActivity/ActivityList?userId=" + HttpContext.Current.Session["CodepalUserId"] + "'CPTKITRS><i class='glyphicon glyphicon-duplicate'></i>I.T.R.S.</a></li>" + Environment.NewLine +
						 "<li CPLICF><a id='permitApps' href='#permitsSubmenu' data-toggle='collapse' aria-expanded='false' style='background-color:dodgerblue 'CPTKCF><i class='glyphicon glyphicon-file'></i>Applications</a><ul id='permitsSubmenu' class='collapse list-unstyled'>" + Environment.NewLine;

				//<li><a id='myPR' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/CodepalForms/CFPermit/LoadQuicklinkPermitSearch?PermitTypeId=91d0b06c-4970-450f-a266-7684274cad6c&agencyId=9808204f-d941-451e-b121-02c8a0d7e7fa' class='btn bg-primary'><i class='glyphicon glyphicon-file'></i>Firework Stand</a></li><li><a id='myInv' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/CodepalForms/CFPermit/LoadQuicklinkPermitSearch?PermitTypeId=74822e11-3cdb-4298-a591-930c1a367f3a&agencyId=9808204f-d941-451e-b121-02c8a0d7e7fa' class='btn bg-primary'><i class='glyphicon glyphicon-file'></i>Fireworks Show</a></li></ul></li>" +
				result += GetCFPermitSublist(AgencyId);
				result += "</ul></li> " + Environment.NewLine;
				result += "<li CPLICP><a id='activityReq' href='#CodepalSubMenu' data-toggle='collapse' aria-expanded='false'CPTKCP><i class='glyphicon glyphicon-copy'></i>Codepal</a>" + Environment.NewLine +
						  "<ul id='CodepalSubMenu' class='collapse list-unstyled'>" + Environment.NewLine +
						  "<li><a id='addSearch' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/Codepal/CPAddress/Search?alternateView=asf' class='btn bg-primary'><i class='glyphicon glyphicon-map-marker'></i>Address Search</a></li>" + Environment.NewLine +
						  "<li><a id='myAct' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/Codepal/CPActivity/ActivityList?userId=" + HttpContext.Current.Session["CodepalUserId"] + "' class='btn bg-primary'><i class='glyphicon glyphicon-copy'></i>My Activity List</a></li>" + Environment.NewLine;
				//<li><a id='myCVI' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/Codepal/CPActivity/AddressForActivityRecord?ActivityTypeId=6dea84c2-f24a-460d-8d03-c89a0a69f988&InspectionTypeId=1a4575e3-a81c-417d-8614-77fc15ec92a4' class='btn bg-primary'><i class='glyphicon glyphicon-copy'></i>Company Inspection</a></li><li><a id='myPR' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/Codepal/CPActivity/AddressForActivityRecord?ActivityTypeId=5933809e-4517-4ef7-b21d-37141304b6e4&InspectionTypeId=e8194533-8b57-472f-aacf-fffef3f59808' class='btn bg-primary'><i class='glyphicon glyphicon-copy'></i>Plan Review</a></li><li><a id='myInv' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/Codepal/CPActivity/AddressForActivityRecord?ActivityTypeId=82b589a9-f5d8-4a1c-9728-cba1dcd466ab&InspectionTypeId=d34abd74-d54f-4acd-b268-a56d4f2845e4' class='btn bg-primary'><i class='glyphicon glyphicon-copy'></i>Investigation</a></li></ul></li>"; ;
				result += GetCPActivitySublist(AgencyId);

				result += "</ul></li>" + Environment.NewLine;

				result = await DoPurchased(result, AgencyId);
			}
			catch (Exception)
			{
				//HandleError(ex, Name, "GetITRSMenu");
				result = "";
			}
			return result;
		}

		public async Task<string> GetCodepalMenu(Guid? AgencyId)
		{
			string result;

			try
			{
				result = "<li><a id='preplanViewer' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/PreplanViewer/PVAddress/Search'CPTKPV><i class='glyphicon glyphicon-eye-open'></i>Preplan Viewer</a></li>" + Environment.NewLine +
						 "<li CPLIHL><a id='addressHotList' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/PreplanViewer/PVAddress/Hotlist'CPTKHL><i class='glyphicon glyphicon-eye-open'></i>Address Hotlist</a></li>" + Environment.NewLine +
						 "<li CPLIPE><a id='preplanEditor' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/PreplanEditor/PEAddress/Search?alternateView=asf'CPTKPE><i class='glyphicon glyphicon-open-file'></i>Preplan Editor</a></li>" + Environment.NewLine +
						 "<li CPLIIT><a id='ITRS' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/ITRS/ITRSActivity/ActivityList?userId=" + HttpContext.Current.Session["CodepalUserId"] + "'CPTKITRS><i class='glyphicon glyphicon-duplicate'></i>I.T.R.S.</a></li>" + Environment.NewLine +
						 "<li CPLICF><a id='permitApps' href='#permitsSubmenu' data-toggle='collapse' aria-expanded='false'CPTKCF><i class='glyphicon glyphicon-file'></i>Applications</a><ul id='permitsSubmenu' class='collapse list-unstyled'>" + Environment.NewLine;

				//<li><a id='myPR' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/CodepalForms/CFPermit/LoadQuicklinkPermitSearch?PermitTypeId=91d0b06c-4970-450f-a266-7684274cad6c&agencyId=9808204f-d941-451e-b121-02c8a0d7e7fa' class='btn bg-primary'><i class='glyphicon glyphicon-file'></i>Firework Stand</a></li><li><a id='myInv' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/CodepalForms/CFPermit/LoadQuicklinkPermitSearch?PermitTypeId=74822e11-3cdb-4298-a591-930c1a367f3a&agencyId=9808204f-d941-451e-b121-02c8a0d7e7fa' class='btn bg-primary'><i class='glyphicon glyphicon-file'></i>Fireworks Show</a></li></ul></li>" +
				result += GetCFPermitSublist(AgencyId);
				result += "</ul></li> " + Environment.NewLine;
				result += "<li CPLICP><a id='activityReq' href='#CodepalSubMenu' data-toggle='collapse' aria-expanded='false' style='background-color:dodgerblue'CPTKCP><i class='glyphicon glyphicon-copy'></i>Codepal</a>" + Environment.NewLine +
						  "<ul id='CodepalSubMenu' class='list-unstyled'>" + Environment.NewLine +
						  "<li><a class='btn bg-primary' href='/Codepal/CPAddress?alternateView=asf'>Address Search</a></li>" + Environment.NewLine +
						  "<li><a class='btn bg-primary' href='/Codepal/CPActivity/ActivityList?userId=" + HttpContext.Current.Session["CodepalUserId"] + "'>My Activity List</a></li>" + Environment.NewLine;
				//<li><a class='btn bg-primary' href='/Codepal/CPActivity/AddressForActivityRecord?ActivityTypeId=6dea84c2-f24a-460d-8d03-c89a0a69f988&amp;InspectionTypeId=1a4575e3-a81c-417d-8614-77fc15ec92a4'>Company Inspection</a></li><li><a class='btn bg-primary' href='/Codepal/CPActivity/AddressForActivityRecord?ActivityTypeId=5933809e-4517-4ef7-b21d-37141304b6e4&amp;InspectionTypeId=e8194533-8b57-472f-aacf-fffef3f59808'>Plan Review</a></li><li><a class='btn bg-primary' href='/Codepal/CPActivity/AddressForActivityRecord?ActivityTypeId=82b589a9-f5d8-4a1c-9728-cba1dcd466ab&amp;InspectionTypeId=d34abd74-d54f-4acd-b268-a56d4f2845e4'>Investigation</a></li></ul></li>";
				result += GetCPActivitySublist(AgencyId);

				result += "</ul></li>" + Environment.NewLine;

				result = await DoPurchased(result, AgencyId);
			}
			catch (Exception)
			{
				//HandleError(ex, Name, "GetITRSMenu");
				result = "";
			}
			return result;
		}

		private async Task<string> DoPurchased(string MenuText, Guid? AgencyId)
		{
			ISystemService settings = new SystemService(new CodepalWebModel(HttpContext.Current.Session["userConnection"].ToString()), new Logging.Logging());
			string result;
			string tmpMenuText = MenuText;
			string curSetting;
			bool hideDisabled = false;
			string strHide = "";
			try
			{
				strHide = (await settings.GetCodepalSetting("hideDisabledWeb", AgencyId, null));
				if (strHide != "")
					hideDisabled = Convert.ToBoolean(Convert.ToInt32(strHide));

				//WEL - 8.8.8 - 08/30/2019 - Uncommnet when/if we start charging for PreplanViewer
				//curSetting = await settings.GetCodepalSetting("CPPVPurchased", AgencyId, null);
				//if (curSetting == "1")
				//{
				tmpMenuText = tmpMenuText.Replace("CPTKPV", "");
				//}
				//else
				//{
				//	tmpMenuText = tmpMenuText.Replace("CPTKPV", " class='not-active'");
				//}

				curSetting = await settings.GetCodepalSetting("CPHLPurchased", AgencyId, null);
				if (curSetting == "1")
				{
					tmpMenuText = tmpMenuText.Replace("CPTKHL", "");
					tmpMenuText = tmpMenuText.Replace(" CPLIHL", "");
				}
				else
				{
					if (hideDisabled)
					{
						tmpMenuText = tmpMenuText.Replace(" CPLIHL", " style='display:none;'");
					}
					else
					{
						tmpMenuText = tmpMenuText.Replace(" CPLIHL", "");
					}
					tmpMenuText = tmpMenuText.Replace("CPTKHL", " class='not-active'");
				}

				curSetting = await settings.GetCodepalSetting("CPPEPurchased", AgencyId, null);
				if (curSetting == "1")
				{
					tmpMenuText = tmpMenuText.Replace("CPTKPE", "");
					tmpMenuText = tmpMenuText.Replace(" CPLIPE", "");
				}
				else
				{
					if (hideDisabled)
					{
						tmpMenuText = tmpMenuText.Replace(" CPLIPE", " style='display:none;'");
					}
					else
					{
						tmpMenuText = tmpMenuText.Replace(" CPLIPE", "");
					}
					tmpMenuText = tmpMenuText.Replace("CPTKPE", " class='not-active'");
				}

				//WEL - NMSFM - 11/11/2019 - Commneted out to hide the ITRS Menu. Uncooment to begin showing it again.
				//curSetting = await settings.GetCodepalSetting("CPITRSPurchased", AgencyId, null);
				//if (curSetting == "1")
				//{
				//	tmpMenuText = tmpMenuText.Replace("CPTKITRS", "");
				//}
				//else
				//{
				//	if (hideDisabled)
				//	{
				tmpMenuText = tmpMenuText.Replace(" CPLIIT", " style='display:none;'");
				//	}
				//	else
				//	{
				//		tmpMenuText = tmpMenuText.Replace("CPLIIT", "");
				//	}
					tmpMenuText = tmpMenuText.Replace("CPTKITRS", " class='not-active'");
				//}

				curSetting = await settings.GetCodepalSetting("CPCFPurchased", AgencyId, null);
				if (curSetting == "1")
				{
					tmpMenuText = tmpMenuText.Replace("CPTKCF", "");
					tmpMenuText = tmpMenuText.Replace(" CPLICF", "");
				}
				else
				{
					if (hideDisabled)
					{
						tmpMenuText = tmpMenuText.Replace(" CPLICF", " style='display:none;'");
					}
					else
					{
						tmpMenuText = tmpMenuText.Replace(" CPLICF", "");
					}
					tmpMenuText = tmpMenuText.Replace("CPTKCF", " class='not-active'");
				}

				curSetting = await settings.GetCodepalSetting("CPCPPurchased", AgencyId, null);
				if (curSetting == "1")
				{
					tmpMenuText = tmpMenuText.Replace("CPTKCP", "");
					tmpMenuText = tmpMenuText.Replace(" CPLICP", "");
				}
				else
				{
					if (hideDisabled)
					{
						tmpMenuText = tmpMenuText.Replace(" CPLICP", " style='display:none;'");
					}
					else
					{
						tmpMenuText = tmpMenuText.Replace(" CPLICP", "");
					}
					tmpMenuText = tmpMenuText.Replace("CPTKCP", " class='not-active'");
				}

				result = tmpMenuText;
			}
			catch (Exception)
			{
				//HandleError(ex, Name, "DoPurchased");
				result = "";
			}
			return result;
		}

		private string GetCPActivitySublist(Guid? AgencyId)
		{
			string result = "";
			string[] actTypes;
			Int16 count = 0;

			var tmpActTypes = cwmContext.Settings.SingleOrDefault(s => s.PropertyField == "CPOCPQuickLinkActivityTypes" && s.AgencyId == AgencyId);
			if (tmpActTypes != null)
			{
				if (tmpActTypes != null && tmpActTypes.ValueField != "")
				{
					actTypes = tmpActTypes.ValueField.Split(',');

					foreach (string oItem in actTypes)
					{
						var actGuid = Guid.Parse(oItem);
						//Get a record that contains the ActivityCategoryID, ActivityTypeId, and ActivityType to put in the menu.
						var actType = cwmContext.InspectionTypes.FirstOrDefault(it => it.InspectionTypeId == actGuid);
						if (actType != null)
						{
							count += 1;
							result += "<li><a id='cpat" + count + "' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/Codepal/CPActivity/AddressForActivityRecord?ActivityTypeId=" + actType.ActivityTypeId.ToString() + "&InspectionTypeId=" + actType.InspectionTypeId.ToString() + "' class='btn bg-primary'><i class='glyphicon glyphicon-copy'></i>" + actType.InspectionType1 + "</a></li>" + Environment.NewLine;

						}
					}
					//result += "<li><a id='myCVI' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/Codepal/CPActivity/AddressForActivityRecord?ActivityTypeId=6dea84c2-f24a-460d-8d03-c89a0a69f988&InspectionTypeId=1a4575e3-a81c-417d-8614-77fc15ec92a4' class='btn bg-primary'><i class='glyphicon glyphicon-copy'></i>Company Inspection</a></li>";
					//result += "<li><a id='myPR' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/Codepal/CPActivity/AddressForActivityRecord?ActivityTypeId=5933809e-4517-4ef7-b21d-37141304b6e4&InspectionTypeId=e8194533-8b57-472f-aacf-fffef3f59808' class='btn bg-primary'><i class='glyphicon glyphicon-copy'></i>Plan Review</a></li>";
					//result += "<li><a id='myInv' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/Codepal/CPActivity/AddressForActivityRecord?ActivityTypeId=82b589a9-f5d8-4a1c-9728-cba1dcd466ab&InspectionTypeId=d34abd74-d54f-4acd-b268-a56d4f2845e4' class='btn bg-primary'><i class='glyphicon glyphicon-copy'></i>Investigation</a></li>";
				}
			}
			return result;
		}

		//WEL - 8.8.8 - 08/30/2019 - to be done later

		//private string GetCFPermitSublist(Guid? AgencyId)
		//{
		//	string result = "";
		//	string tmpPermitTypes = "";
		//	string[] perTypes;
		//	Int16 count = 0;

		//	tmpPermitTypes = cwmContext.Settings.SingleOrDefault(s => s.PropertyField == "CPOCPQuickLinkPermitTypes" && s.AgencyId == AgencyId).ValueField;
		//	if (tmpPermitTypes != null && tmpPermitTypes != "")
		//	{
		//		perTypes = tmpPermitTypes.Split(',');

		//		foreach (string oItem in perTypes)
		//		{
		//			var ptGuid = Guid.Parse(oItem);
		//			//Get a record that contains the ActivityCategoryID, ActivityTypeId, and ActivityType to put in the menu.
		//			var perType = cwmContext.PermitTypes.FirstOrDefault(pt => pt.PermitTypeId == ptGuid);
		//			if (perType != null)
		//			{
		//				count += 1;
		//				result += "<li><a id='myat" + count + "' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/Codepal/CPPermit/AddressForActivityRecord?ActivityTypeId=" + perType.PermitTypeId.ToString() + "&InspectionTypeId=" + perType.InspectionTypeId.ToString() + "' class='btn bg-primary'><i class='glyphicon glyphicon-copy'></i>" + perType.InspectionType1 + "</a></li>";

		//			}
		//		}
		//		//result += "<li><a id='myCVI' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/Codepal/CPActivity/AddressForActivityRecord?ActivityTypeId=6dea84c2-f24a-460d-8d03-c89a0a69f988&InspectionTypeId=1a4575e3-a81c-417d-8614-77fc15ec92a4' class='btn bg-primary'><i class='glyphicon glyphicon-copy'></i>Company Inspection</a></li>";
		//		//result += "<li><a id='myPR' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/Codepal/CPActivity/AddressForActivityRecord?ActivityTypeId=5933809e-4517-4ef7-b21d-37141304b6e4&InspectionTypeId=e8194533-8b57-472f-aacf-fffef3f59808' class='btn bg-primary'><i class='glyphicon glyphicon-copy'></i>Plan Review</a></li>";
		//		//result += "<li><a id='myInv' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/Codepal/CPActivity/AddressForActivityRecord?ActivityTypeId=82b589a9-f5d8-4a1c-9728-cba1dcd466ab&InspectionTypeId=d34abd74-d54f-4acd-b268-a56d4f2845e4' class='btn bg-primary'><i class='glyphicon glyphicon-copy'></i>Investigation</a></li>";
		//	}
		//	return result;
		//}

		//private string GetCFActivitySublist(Guid? AgencyId)
		//{
		//	string result = "";
		//	string tmpActTypes = "";
		//	string[] actTypes;
		//	Int16 count = 0;

		//	tmpActTypes = cwmContext.Settings.SingleOrDefault(s => s.PropertyField == "CPOCPQuickLinkActivityTypes" && s.AgencyId == AgencyId).ValueField;
		//	if (tmpActTypes != null && tmpActTypes != "")
		//	{
		//		actTypes = tmpActTypes.Split(',');

		//		foreach (string oItem in actTypes)
		//		{
		//			var actGuid = Guid.Parse(oItem);
		//			//Get a record that contains the ActivityCategoryID, ActivityTypeId, and ActivityType to put in the menu.
		//			var actType = cwmContext.InspectionTypes.FirstOrDefault(it => it.InspectionTypeId == actGuid);
		//			if (actType != null)
		//			{
		//				count += 1;
		//				result += "<li><a id='myat" + count + "' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/Codepal/CPActivity/AddressForActivityRecord?ActivityTypeId=" + actType.ActivityTypeId.ToString() + "&InspectionTypeId=" + actType.InspectionTypeId.ToString() + "' class='btn bg-primary'><i class='glyphicon glyphicon-copy'></i>" + actType.InspectionType1 + "</a></li>";

		//			}
		//		}
		//		//result += "<li><a id='myCVI' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/Codepal/CPActivity/AddressForActivityRecord?ActivityTypeId=6dea84c2-f24a-460d-8d03-c89a0a69f988&InspectionTypeId=1a4575e3-a81c-417d-8614-77fc15ec92a4' class='btn bg-primary'><i class='glyphicon glyphicon-copy'></i>Company Inspection</a></li>";
		//		//result += "<li><a id='myPR' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/Codepal/CPActivity/AddressForActivityRecord?ActivityTypeId=5933809e-4517-4ef7-b21d-37141304b6e4&InspectionTypeId=e8194533-8b57-472f-aacf-fffef3f59808' class='btn bg-primary'><i class='glyphicon glyphicon-copy'></i>Plan Review</a></li>";
		//		//result += "<li><a id='myInv' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/Codepal/CPActivity/AddressForActivityRecord?ActivityTypeId=82b589a9-f5d8-4a1c-9728-cba1dcd466ab&InspectionTypeId=d34abd74-d54f-4acd-b268-a56d4f2845e4' class='btn bg-primary'><i class='glyphicon glyphicon-copy'></i>Investigation</a></li>";
		//	}
		//	return result;
		//}

		private string GetCFPermitSublist(Guid? AgencyId)
		{
			string result = "";

			string[] perTypes;
			Int16 count = 0;


			var tmpPerTypes = cwmContext.Settings.SingleOrDefault(s => s.PropertyField == "CPOCFQuickLinkPermitTypes" && s.AgencyId == AgencyId);
			if (tmpPerTypes != null)
			{
				if (tmpPerTypes != null && tmpPerTypes.ValueField != "")
				{
					perTypes = tmpPerTypes.ValueField.Split(',');

					foreach (string oItem in perTypes)
					{
						var perGuid = Guid.Parse(oItem);
						//Get a record that contains the ActivityCategoryID, ActivityTypeId, and ActivityType to put in the menu.
						var perType = cwmContext.PermitTypes.FirstOrDefault(it => it.PermitTypeId == perGuid);
						if (perType != null)
						{
							count += 1;
							result += "<li><a id='cfper" + count + "' href='http://" + ConfigurationManager.AppSettings["RootDomain"] + "/CodepalForms/CFPermit/LoadQuicklinkPermitSearch?PermitTypeId=" + perType.PermitTypeId.ToString() + "&agencyId=" + AgencyId.ToString() + "' class='btn bg-primary'><i class='glyphicon glyphicon-file'></i>" + perType.PermitType1 + "</a></li>" + Environment.NewLine;
						}
					}
				}
			}
			return result;
		}
	}
}
