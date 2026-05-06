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
namespace NMSFM.Services.Complaint
{
    class ComplaintService
    {
        private ICodepalWebModel cwmContext;
        private IAuditService auditService;
        private ILogging logger;


        public ComplaintService(ICodepalWebModel codepalWebModel, ILogging codepalLogger)
        {
            cwmContext = codepalWebModel;
            logger = codepalLogger;
            auditService = new AuditService(logger);
        }


        //Task<IEnumerable<v_Complaints>> GetComplaintAsync();
        public async Task<IEnumerable<v_Complaints>> GetComplaintAsync()
        {
            IEnumerable<v_Complaints> result;
            try
            {
                var complaintList = await cwmContext.v_Complaints.Where(a => a.ComplaintId != null).ToListAsync();
                var complaintTypeList = await cwmContext.ComplaintTypes.Where(a => a.Inactive == false && a.WebViewable == true).Select(a => a.ComplaintTypeId).ToListAsync();
                if (complaintList != null && complaintList.Count() > 0)
                {
                    for (int i = complaintList.Count() - 1; i > -1; i--)
                    {
                        var complaintType = complaintList[i].ComplaintTypeId == null ? Guid.Empty : complaintList[i].ComplaintTypeId.Value;
                        if (!complaintTypeList.Contains(complaintType))
                        {
                            complaintList.RemoveAt(i);
                        }
                    }
                }
                result = complaintList;
            }
            catch (Exception ex)
            {
                _ = ex;
                logger.Error("Unexpected exception caught while retrieving the Complaint list.", ex);
                result = new List<v_Complaints>();
            }
            return result;
        }


        //Task<IEnumerable<PermitType>> GetComplaintTypeListAsync(Guid agencyId);
        public async Task<IEnumerable<ComplaintType>> GetComplaintTypeListAsync(Guid agencyId)
        {
            IEnumerable<ComplaintType> result;
            try
            {
                result = await cwmContext.ComplaintTypes.Where(a => !a.Inactive && a.WebViewable && (a.AgencyId == agencyId || a.AgencyId == null)).ToListAsync();
            }
            catch (Exception ex)
            {
                _ = ex;
                logger.Error("Unexpected exception caught while retrieving the Complaint Type List.", ex);
                result = new List<ComplaintType>();
            }
            return result;
        }


        //Task<IEnumerable<PermitStatu>> GetComplaintStatusListAsync(Guid agencyId);
        public async Task<IEnumerable<ComplaintStatu>> GetComplaintStatusListAsync(Guid agencyId)
        {
            IEnumerable<ComplaintStatu> result;
            try
            {
                result = await cwmContext.ComplaintStatus.Where(a => !a.Inactive == true && a.WebViewable && (a.AgencyId == agencyId || a.AgencyId == null)).ToListAsync();
            }
            catch (Exception ex)
            {
                _ = ex;
                logger.Error("Unexpected exception caught while retrieving the Complaint Status List.", ex);
                result = new List<ComplaintStatu>();
            }
            return result;
        }


        //Task<List<v_ComplaintActivities>> GetActivitiesByComplaintIdAsync(Guid id);
        public async Task<List<v_ComplaintActivities>> GetActivitiesByComplaintIdAsync(Guid id)
        {
            List<v_ComplaintActivities> result;
            try
            {
                result = await cwmContext.v_ComplaintActivities.Where(p => p.ComplaintId == id).ToListAsync();
            }
            catch (Exception ex)
            {
                _ = ex;
                logger.Error("Unexpected exception caught while retrieving the Complaint Activity List.", ex);
                result = new List<v_ComplaintActivities>();
            }
            return result;
        }

        //Task<List<v_ComplaintParties>> GetPartiesByComplaintIdAsync(Guid id)
        public async Task<List<v_ComplaintParties>> GetPartiesByComplaintIdAsync(Guid id)
        {
            List<v_ComplaintParties> result;
            try
            {
                result = await cwmContext.v_ComplaintParties.Where(p => p.ComplaintId == id).ToListAsync();
            }
            catch (Exception ex)
            {
                _ = ex;
                logger.Error("Unexpected exception caught while retrieving the Complaint Party List.", ex);
                result = new List<v_ComplaintParties>();
            }
            return result;
        }

        //Task<List<v_ComplaintPermits>> GetPermitsByComplaintIdAsync(Guid id)
        public async Task<List<v_ComplaintPermits>> GetPermitsByComplaintIdAsync(Guid id)
        {
            List<v_ComplaintPermits> result;
            try
            {
                result = await cwmContext.v_ComplaintPermits.Where(p => p.ComplaintId == id).ToListAsync();
            }
            catch (Exception ex)
            {
                _ = ex;
                logger.Error("Unexpected exception caught while retrieving the Complaint Permit List.", ex);
                result = new List<v_ComplaintPermits>();
            }
            return result;
        }

        //Task<List<File>> GetComplaintFilesByComplaintId(Guid id)
        public async Task<List<File>> GetComplaintFilesByComplaintId(Guid id)
        {
            List<File> result;
            try
            {
                result = await cwmContext.Files.Where(p => p.RecordId == id).ToListAsync();
            }
            catch (Exception ex)
            {
                _ = ex;
                logger.Error("Unexpected exception caught while retrieving the Complaint File List.", ex);
                result = new List<File>();
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
                logger.Error("Unexpected exception caught while retrieving the Complaint Notes List.", ex);
                result = new List<Data.Note>();
            }
            return result;
        }

        
        //Task<IEnumerable<UserDefinedValue>> GetUserDefinedValuesByItemIdAsync(Guid id, Guid agency);
        public async Task<IEnumerable<UserDefinedValue>> GetUserDefinedValuesByItemIdAsync(Guid id, Guid pTypeId, Guid agency)
        {
            IEnumerable<UserDefinedValue> results = new List<UserDefinedValue>();
            Guid ComplaintTypeId = pTypeId;
            var ModuleId = new Guid();
            try
            {
                v_Complaints complaint = null;
                complaint = await cwmContext.v_Complaints.SingleOrDefaultAsync(a => a.ComplaintId == id);
                if (complaint != null)
                {
                    ComplaintTypeId = (Guid)complaint.ComplaintTypeId;
                }

                Guid AgencyId = new Guid("62a16726-f85b-4183-8556-b87154617d42");
                if (agency != null && agency != Guid.Empty)
                {
                    AgencyId = agency;
                }
                Module module = null;
                module = await cwmContext.Modules.SingleOrDefaultAsync(a => (a.AgencyId.HasValue ? a.AgencyId.Value : Guid.Empty) == AgencyId && a.ModuleDesc == "Request");
                ModuleId = module.ModuleId;

                var models = from cats in cwmContext.UserDefCategories
                             join catt in cwmContext.UserDefCategoryTypes on new { cd = cats.UserDefCategoryId, td = ComplaintTypeId } equals new { cd = catt.UserDefCategoryId, td = catt.TypeId } into subcat
                             from usecat in subcat.DefaultIfEmpty()
                             where (cats.ModuleId == ModuleId && (((cats.ModuleTypeId.Equals(null) ? Guid.Empty : cats.ModuleTypeId.Value) == ComplaintTypeId) || (usecat.TypeId == ComplaintTypeId || cats.AllModuleTypes == true) || ((cats.ModuleTypeId.Equals(null) ? Guid.Empty : cats.ModuleTypeId.Value) == ComplaintTypeId && cats.AllModuleTypes == true) || usecat.TypeId == ComplaintTypeId) && ((cats.WebViewable.Equals(null) ? false : cats.WebViewable.Value) == true && cats.Inactive == false))
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
                logger.Error("Unexpected exception caught while retrieving user defined values for complaint '" + id.ToString() + "'.", ex);
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
                                logger.Error("Unable to save the user defined value changes for complaint '" + list[i].RecordId.ToString() + "'.", ex);
                                return;
                            }
                        }
                        else
                        {
                            logger.Error("Unable to update the user defined values for complaint '" + list[i].RecordId.ToString() + "', DbContext was not available.");
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

