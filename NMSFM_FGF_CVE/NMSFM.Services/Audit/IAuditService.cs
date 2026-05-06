using System.Collections.Generic;
using System.Threading.Tasks;
using NMSFM.Data;
using System;
using NMSFM.Services.Models;
using NMSFM.ViewModels;

namespace NMSFM.Services.Audit
{
    public interface IAuditService
    {
        Task UpdateAudit(AuditModel audit, List<AuditFieldModel> auditFields);
    }
}
