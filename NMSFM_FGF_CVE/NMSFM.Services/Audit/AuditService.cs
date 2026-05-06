using NMSFM.Data;
using System;
using System.Web;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using NMSFM.Services.Models;
using NMSFM.Services.Logging;

namespace NMSFM.Services.Audit
{
    public class AuditService : IAuditService
    {
        private IAuditWebModel awmContext;
        private ILogging logger;

        public AuditService(ILogging codepalLogger)
        {
            awmContext = null;
            logger = codepalLogger;
        }

        public async Task UpdateAudit(AuditModel audit, List<AuditFieldModel> auditFields)
        {
            try
            {
                var userConnection = (string)System.Web.HttpContext.Current.Session["userConnection"];
                var auditConnection = GetAuditConnection(userConnection);
                this.awmContext = new AuditWebModel(auditConnection);
                var sessionId = (Guid)System.Web.HttpContext.Current.Session["SessionId"];

                var session = await awmContext.AuditSessions.SingleOrDefaultAsync(a => a.SessionId == sessionId);
                if (session == null)
                {
                    session = awmContext.AuditSessions.Add(new Data.AuditSession());
                    session.SessionId = sessionId;
                    session.UserId = (Guid)HttpContext.Current.Session["CodepalUserId"];
                    session.ComputerName = "Website";
                    session.ComputerIP = HttpContext.Current.Request.UserHostAddress;
                    session.WindowsUser = Environment.UserName; // The server's Windows user is used.
                    session.SessionStart = DateTime.Now;
                    session.SessionEnd = null;
                    session.UserName = (string)HttpContext.Current.Session["CodepalUserName"];
                    session.rowguid = Guid.NewGuid();
                }
                else
                {
                    session.SessionEnd = DateTime.Now;
                }
                var newAudit = awmContext.Audits.Add(new Data.Audit());
                newAudit.AuditId = Guid.NewGuid();
                newAudit.SessionId = sessionId;
                newAudit.TableName = audit.TableName;
                newAudit.RecordId = audit.RecordId;
                newAudit.AuditAction = audit.AuditAction;
                newAudit.Description = audit.Description;
                newAudit.DateStamp = DateTime.Now;
                newAudit.rowguid = Guid.NewGuid();

                for (int i = 0; i < auditFields.Count(); i++)
                {
                    var newField = awmContext.AuditFields.Add(new Data.AuditField());
                    newField.AuditFieldId = Guid.NewGuid();
                    newField.AuditId = newAudit.AuditId;
                    newField.ControlName = auditFields[i].ControlName;
                    newField.FieldDesc = auditFields[i].FieldDesc;
                    newField.OldId = auditFields[i].OldId.HasValue ? auditFields[i].OldId.Value.ToString() : null;
                    newField.OldValue = auditFields[i].OldValue;
                    newField.NewId = auditFields[i].NewId.HasValue ? auditFields[i].NewId.Value.ToString() : null;
                    newField.NewValue = auditFields[i].NewValue;
                    newField.rowguid = Guid.NewGuid();
                }
                if (awmContext is DbContext)
                {
                    await ((DbContext)awmContext).SaveChangesAsync();
                }
                else
                {
                    logger.Error("Unable to update audit records, DbContext was not available.");
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                logger.Error("Unable to update audit records.", ex);
            }

            return;
        }

        private string GetAuditConnection(string userConnection)
        {
            var result = "";
            if (userConnection != null && userConnection != String.Empty)
            {
                var userConnectionParts = userConnection.Split(';');
                for (int i = 0; i < userConnectionParts.Count(); i++)
                {
                    if (userConnectionParts[i].Contains("initial catalog"))
                    {
                        userConnectionParts[i] = userConnectionParts[i] + "Audits";
                    }
                    result += userConnectionParts[i];
                    if (i != userConnectionParts.Count() - 1)
                    {
                        result += ";";
                    }
                }
            }
            return result;
        }
    }
}

