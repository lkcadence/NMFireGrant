//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
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
using System.Security.Cryptography;
using NMSFM.ViewModels;
using AutoMapper;

namespace NMSFM.Services.Project
{
    class ProjectService
    {
        private ICodepalWebModel cwmContext;
        private IAuditService auditService;
        private ILogging logger;


        public ProjectService(ICodepalWebModel codepalWebModel, ILogging codepalLogger)
        {
            cwmContext = codepalWebModel;
            logger = codepalLogger;
            auditService = new AuditService(logger);
        }

        //Task<IEnumerable<v_Projects>> GetProjectsAsync();
        public async Task<IEnumerable<v_Projects>> GetProjectsAsync()
        {
            IEnumerable<v_Projects> result;
            try
            {
                var projects = await cwmContext.v_Projects.ToListAsync();
                var projectTypeList = await cwmContext.ProjectTypes.Where(a => a.Inactive == false && a.WebViewable == true).Select(a => a.ProjectTypeId).ToListAsync();
                if (projects != null && projects.Count() > 0 & projectTypeList != null & projectTypeList.Count() > 0)
                {
                    projects = projects.Where(a => projectTypeList.Contains(a.ProjectTypeId == null ? Guid.Empty : a.ProjectTypeId)).ToList();
                }
                result = projects;
            }
            catch (Exception ex)
            {
                _ = ex;
                logger.Error("Unexpected exception caught while retrieving the Project List.", ex);
                result = new List<v_Projects>();
            }
            return result;
        }

        //Task<List<ProjectType>> GetProjectTypeAsync();
        public async Task<List<ProjectType>> GetProjectTypeAsync()
        {
            List<ProjectType> result;
            try
            {
                var projectTypeList = await cwmContext.ProjectTypes.Where(a => !a.Inactive).ToListAsync();
                result = projectTypeList;
            }
            catch (Exception ex)
            {
                _ = ex;
                logger.Error("Unexpected exception caught while retrieving the Project Types list.", ex);
                result = new List<ProjectType>();
            }
            return result;
        }

        //Task<List<ProjectStatu>> GetProjectStatus();
        public async Task<List<ProjectStatu>> GetProjectStatus()
        {
            List<ProjectStatu> result;
            try
            {
                var projectStatusList = await cwmContext.ProjectStatus.Where(a => !a.Inactive).ToListAsync();
                result = projectStatusList;
            }
            catch (Exception ex)
            {
                _ = ex;
                logger.Error("Unexpected exception caught whil retrieving the Project Status List.", ex);
                result = new List<ProjectStatu>();
            }
            return result;
        }


        //Task<IEnumerable<v_AgreeMents>> GetProjectAgreementsByProjectIdAsync(Guid id);
        public async Task<IEnumerable<v_Agreements>> GetProjectAgreementsByProjectIdAsync(Guid id)
        {
            IEnumerable<v_Agreements> result;
            try
            {
                var projectAgreements = await cwmContext.v_Agreements.Where(p => p.ProjectId == id).ToListAsync();

                result = projectAgreements;
            }
            catch (Exception ex)
            {
                _ = ex;
                logger.Error("Unexpected exception caught while retrieving the Project Agreements List.", ex);
                result = new List<v_Agreements>();
            }
            return result;
        }

        //Task<IEnumerable<v_ProjectAddressSearch>> GetProjectAddressesByProjectIdAsync(Guid id);
        public async Task<IEnumerable<v_ProjectAddressSearch>> GetProjectAddressesByProjectIdAsync(Guid id)
        {
            IEnumerable<v_ProjectAddressSearch> result;
            try
            {
                var projectAddresses = await cwmContext.v_ProjectAddressSearch.Where(p => p.ProjectId == id).ToListAsync();
                result = projectAddresses;
            }
            catch (Exception ex)
            {
                _ = ex;
                logger.Error("Unexpected exception caught while retrieving the Project Addresses List.", ex);
                result = new List<v_ProjectAddressSearch>();                
            }
            return result;
        }


        //Task<IEnumerable<v_ProjectActivitySearch>> GetProjectActivityByProjectIdAsync(Guid id);
        public async Task<IEnumerable<v_ProjectActivitySearch>> GetProjectActivityByProjectIdAsync(Guid id)
        {
            IEnumerable<v_ProjectActivitySearch> result;
            try
            {
                var projectActivities = await cwmContext.v_ProjectActivitySearch.Where(p => p.ProjectId == id).ToListAsync();
                result = projectActivities;
            }
            catch (Exception ex)
            {
                _ = ex;
                logger.Error("Unexpected exception caught while retrieving the Project Activities List.", ex);
                result = new List<v_ProjectActivitySearch>();
            }
            return result;
        }

        //Task<IEnumerable<v_ProjectPermitSearch>> GetProjectPermitByProjectIdAsync(Guid id);
        public async Task<IEnumerable<v_ProjectPermitSearch>> GetProjectPermitByProjectIdAsync(Guid id)
        {
            IEnumerable<v_ProjectPermitSearch> result;
            try
            {
                var projectPermits = await cwmContext.v_ProjectPermitSearch.Where(p => p.ProjectId == id).ToListAsync();
                return projectPermits;
            }
            catch (Exception ex)
            {
                _ = ex;
                logger.Error("Unexpected exception caught while retrieving the Project Permits List.", ex);
                result = new List<v_ProjectPermitSearch>();
            }
            return result;
        }

        //Task<IEnumerable<v_ProjectRequestSearch>> GetProjectRequestByProjectIdAsync(Guid id);
        public async Task<IEnumerable<v_ProjectRequestSearch>> GetProjectRequestByProjectIdAsync(Guid id)
        {
            IEnumerable<v_ProjectRequestSearch> result;
            try
            {
                var projectRequests = await cwmContext.v_ProjectRequestSearch.Where(p => p.ProjectId == id).ToListAsync();
                return projectRequests;
            }
            catch (Exception ex)
            {
                _ = ex;
                logger.Error("Unexpected exception caught while retrieving the Project Requests List.", ex);
                result = new List<v_ProjectRequestSearch>();
            }
            return result;
        }

        //Task<IEnumerable<v_ProjectInspectorSearch>> GetProjectInspectorByProjectIdAsync(Guid id);
        public async Task<IEnumerable<v_ProjectInspectorSearch>> GetProjectInspectorByProjectIdAsync(Guid id)
        {

            IEnumerable<v_ProjectInspectorSearch> result;
            try
            {
                var projectInspector = await cwmContext.v_ProjectInspectorSearch.Where(p => p.ProjectId == id).ToListAsync();
                return projectInspector;
            }
            catch (Exception ex)
            {
                _ = ex;
                logger.Error("Unexpected exception caught while retrieving the Project Inspectors List.", ex);
                result = new List<v_ProjectInspectorSearch>();
            }
            return result;
        }

        //Task<IEnumerable<v_Files>> GetProjectFilesByProjectIdAsync(Guid id);
        public async Task<IEnumerable<v_Files>> GetProjectFilesByProjectIdAsync(Guid id)
        {
            IEnumerable<v_Files> result;
            try
            {
                var projectFiles = await cwmContext.v_Files.Where(p => p.RecordId == id).ToListAsync();
                return projectFiles;
            }
            catch (Exception ex)
            {
                _ = ex;
                logger.Error("Unexpected exception caught while retrieving the Project Files List.", ex);
                result = new List<v_Files>();
            }
            return result;
        }

        //Task<IEnumerable<v_Fees>> GetProjectFeesByProjectIdAsync(Guid id);
        public async Task<IEnumerable<v_Fees>> GetProjectFeesByProjectIdAsync(Guid id)
        {
            IEnumerable<v_Fees> result;
            try
            {
                var projectFees = await cwmContext.v_Fees.Where(p => p.ProjectId == id).ToListAsync();
                return projectFees;
            }
            catch (Exception ex)
            {
                _ = ex;
                logger.Error("Unexpected exception caught while retrieving the Project Fees List.", ex);
                result = new List<v_Fees>();
            }
            return result;
        }

        //Task<IEnumerable<Data.Note>> GetNotesByIdAsync(Guid id);
        public async Task<IEnumerable<Data.Note>> GetNotesByIdAsync(Guid id)
        {
            IEnumerable<Data.Note> result;
            try
            {
                result = await cwmContext.Notes.Where(p => p.RecordId == id).ToListAsync();
            }
            catch (Exception ex)
            {
                _ = ex;
                logger.Error("Unexpected exception caught while retrieving the Project Notes List.", ex);
                result = new List<Data.Note>();
            }
            return result;
        }

        //Task<Signature> GetSignatureByProjectId(Guid id);
        public async Task<Data.Signature> GetSignatureByProjectId(Guid id)
        {
            var result = new Data.Signature();
            try
            {
                result = await cwmContext.Signatures.SingleOrDefaultAsync(a => a.RecordId == id && a.Inactive == false) ?? new Data.Signature();
            }
            catch (Exception ex)
            {
                _ = ex;
                logger.Error("Unexpected exception caught while retrieving the Signature for Project Id: " + id + ".", ex);
            }
            return result;
        }

        //Task<IEnumerable<UserDefinedValue>> GetUserDefinedValuesByItemIdAsync(Guid id, Guid pTypeId, Guid agency);
        public async Task<IEnumerable<UserDefinedValue>> GetUserDefinedValuesByItemIdAsync(Guid id, Guid pTypeId, Guid agency)
        {
            IEnumerable<UserDefinedValue> results = new List<UserDefinedValue>();
            Guid ProjectTypeId = pTypeId;
            var ModuleId = new Guid();
            try
            {
                v_Projects project = null;
                project = await cwmContext.v_Projects.SingleOrDefaultAsync(a => a.ProjectId == id);
                if (project != null)
                {
                    ProjectTypeId = (Guid)project.ProjectTypeId;
                }

                Guid AgencyId = new Guid("62a16726-f85b-4183-8556-b87154617d42");
                if (agency != null && agency != Guid.Empty)
                {
                    AgencyId = agency;
                }
                Module module = null;
                module = await cwmContext.Modules.SingleOrDefaultAsync(a => (a.AgencyId.HasValue ? a.AgencyId.Value : Guid.Empty) == AgencyId && a.ModuleDesc == "Project");
                ModuleId = module.ModuleId;

                var models = from cats in cwmContext.UserDefCategories
                             join catt in cwmContext.UserDefCategoryTypes on new { cd = cats.UserDefCategoryId, td = ProjectTypeId } equals new { cd = catt.UserDefCategoryId, td = catt.TypeId } into subcat
                             from usecat in subcat.DefaultIfEmpty()
                             where (cats.ModuleId == ModuleId && (((cats.ModuleTypeId.Equals(null) ? Guid.Empty : cats.ModuleTypeId.Value) == ProjectTypeId) || (usecat.TypeId == ProjectTypeId || cats.AllModuleTypes == true) || ((cats.ModuleTypeId.Equals(null) ? Guid.Empty : cats.ModuleTypeId.Value) == ProjectTypeId && cats.AllModuleTypes == true) || usecat.TypeId == ProjectTypeId) && ((cats.WebViewable.Equals(null) ? false : cats.WebViewable.Value) == true && cats.Inactive == false))
                             join flds in cwmContext.UserDefFields on cats.UserDefCategoryId equals flds.UserDefCategoryId
                             join vals in cwmContext.UserDefValues on new { id = flds.UserDefFieldId, ad = id } equals new { id = vals.UserDefFieldId, ad = vals.RecordId } into subvals
                             from usevals in subvals.DefaultIfEmpty()
                             select new UserDefinedValue
                             {
                                 Category = cats.Category,
                                 CategoryId = cats.UserDefCategoryId,
                                 FieldDescription = flds.FieldDesc,
                                 FieldValue = usevals.UserDefValue1.Equals(null) ? String.Empty : usevals.UserDefValue1,
                                 FieldOldValue = usevals.UserDefValue1.Equals(null) ? String.Empty : usevals.UserDefValue1,
                                 ValueId = usevals.UserDefValueId.Equals(null) ? Guid.Empty : usevals.UserDefValueId,
                                 FieldId = flds.UserDefFieldId,
                                 FieldType = flds.UserDefTypeId,
                                 SequenceNumber = cats.SeqNum.HasValue ? cats.SeqNum.Value : 0,
                                 FieldSequenceNumber = flds.SeqNum.HasValue ? flds.SeqNum.Value : 0,
                                 WebViewable = (cats.WebViewable.HasValue ? cats.WebViewable.Value : false) //&& flds.WebViewable,
                             };

                var resolutionResults = await models.ToListAsync();

                for (int i = 0; i < resolutionResults.Count(); i++)
                {
                    resolutionResults[i].Resolutions = new List<Resolution>();
                    Guid cbId = new Guid(cwmContext.UserDefTypes.Single(t => t.UserDefType1 == "check box").UserDefTypeId.ToString());
                    Guid lstId = new Guid(cwmContext.UserDefTypes.Single(t => t.UserDefType1 == "list").UserDefTypeId.ToString());
                    if (resolutionResults[i].FieldType == cbId) // Check Box
                    {
                        var fieldId = resolutionResults[i].FieldId;
                        resolutionResults[i].Resolutions = await cwmContext.Resolutions.Where(a => (a.ResolutionType.HasValue ? a.ResolutionType.Value : Guid.Empty) == fieldId).OrderBy(a => a.Sequence).ToListAsync();
                        if (resolutionResults[i].Resolutions != null && resolutionResults[i].Resolutions.Count() > 0)
                        {
                            resolutionResults[i].boolValue = new List<bool>();
                            for (int j = 0; j < resolutionResults[i].Resolutions.Count(); j++)
                            {
                                if (resolutionResults[i].FieldValue != String.Empty && resolutionResults[i].FieldValue.Length == resolutionResults[i].Resolutions.Count())
                                {
                                    if (resolutionResults[i].FieldValue.ElementAt(j) == '1')
                                    {
                                        resolutionResults[i].boolValue.Add(true);
                                    }
                                    else
                                    {
                                        resolutionResults[i].boolValue.Add(false);
                                    }
                                }
                                else
                                {
                                    resolutionResults[i].boolValue.Add(false);
                                }
                            }
                        }
                    }
                    else if (resolutionResults[i].FieldType == lstId) // List
                    {
                        var fieldId = resolutionResults[i].FieldId;
                        resolutionResults[i].Resolutions = await cwmContext.Resolutions.Where(a => (a.ResolutionType.HasValue ? a.ResolutionType.Value : Guid.Empty) == fieldId || a.ResolutionType == null).OrderBy(a => a.Sequence).ToListAsync();
                        var linkedResolution = resolutionResults[i].Resolutions.Find(a => a.Resolution1 == resolutionResults[i].FieldValue);
                        if (linkedResolution != null)
                        {
                            resolutionResults[i].ResolutionId = linkedResolution.ResolutionId;
                        }
                    }
                }
                results = resolutionResults;
            }
            catch (Exception ex)
            {
                _ = ex;
                logger.Error("Unexpected exception caught while retrieving user defined values for project '" + id.ToString() + "'.", ex);
            }

            return results;
        }

        //Task SaveUserDefinedValuesAsync(List<UserDefValue> list);
        public async Task SaveUserDefinedValuesAsync(List<UserDefValue> list)
        {
            if (list != null && list.Count() > 0)
            {
                try
                {
                    for (int i = 0; i < list.Count(); i++)
                    {
                        //var audit = new AuditModel { TableName = "UserDefValues", Description = "" };
                        //var auditFields = new List<AuditFieldModel>();
                        //var auditField = new AuditFieldModel { ControlName = "UserValues[i].FieldValue", };
                        var userDefinedValue = new UserDefValue();

                        if (list[i].UserDefValueId != null && list[i].UserDefValueId != Guid.Empty)
                        {
                            Guid tempGuid = list[i].UserDefValueId;
                            userDefinedValue = await cwmContext.UserDefValues.SingleOrDefaultAsync(a => a.UserDefValueId == tempGuid);
                            //auditField.OldId = userDefinedValue.UserDefValueId;
                            //auditField.OldValue = userDefinedValue.UserDefValue1;
                            //audit.AuditAction = "RECORD UPDATED";
                        }
                        else
                        {
                            userDefinedValue = cwmContext.UserDefValues.Add(new UserDefValue());
                            userDefinedValue.UserDefValueId = Guid.NewGuid();
                            userDefinedValue.UserDefFieldId = list[i].UserDefFieldId;
                            userDefinedValue.DateInserted = DateTime.Now;
                            userDefinedValue.RecordId = list[i].RecordId;
                            userDefinedValue.rowguid = list[i].rowguid != Guid.Empty ? list[i].rowguid : Guid.NewGuid();
                            userDefinedValue.VActPrint = false;
                            userDefinedValue.ExternalId = null;
                            //auditField.OldId = null;
                            //auditField.OldValue = null;
                            //audit.AuditAction = "RECORD CREATED";
                        }
                        userDefinedValue.UserDefValue1 = list[i].UserDefValue1;
                        userDefinedValue.DateUpdated = DateTime.Now;
                        //auditField.NewId = userDefinedValue.UserDefValueId;
                        //auditField.NewValue = userDefinedValue.UserDefValue1;
                        //auditField.FieldDesc = cwmContext.UserDefFields.FirstOrDefault(a => a.UserDefFieldId == userDefinedValue.UserDefFieldId).FieldDesc;
                        //audit.RecordId = userDefinedValue.UserDefValueId;

                        if (cwmContext is DbContext)
                        {
                            try
                            {
                                await ((DbContext)cwmContext).SaveChangesAsync();
                                //bool idCheck = (auditField.OldId ?? Guid.Empty) != (auditField.NewId ?? Guid.Empty);
                                //bool valCheck = (auditField.OldValue ?? String.Empty) != (auditField.NewValue ?? String.Empty);
                                //if (idCheck || valCheck)
                                //{
                                //	auditFields.Add(auditField);
                                //	await auditService.UpdateAudit(audit, auditFields);
                                //}
                            }
                            catch (Exception ex)
            {
                _ = ex;
                                logger.Error("Unable to save the user defined value changes for project '" + list[i].RecordId.ToString() + "'.", ex);
                                return;
                            }
                        }
                        else
                        {
                            logger.Error("Unable to update the user defined values for project '" + list[i].RecordId.ToString() + "', DbContext was not available.");
                            return;
                        }
                    }
                }
                catch (Exception ex)
            {
                _ = ex;
                    logger.Error("Unable to complete function: SaveUserDefinedValuesAsync. " + ex);
                }
            }
            else
            {
                logger.Error("SaveUserDefinedValuesAsync was called with a null reference.");
            }
        }
    }
}

