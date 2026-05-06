//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using NMSFM.Data;
using System;
using System.Web.Mvc;
using NMSFM.Services.Models;
using NMSFM.ViewModels;

namespace NMSFM.Services.Complaint
{
    public interface IComplaintService
    {
        Task<IEnumerable<v_Complaints>> GetComplaintAsync();
        Task<IEnumerable<ComplaintType>> GetComplaintTypeListAsync(Guid id);
        Task<IEnumerable<ComplaintStatu>> GetComplaintStatusAsync(Guid id);
        Task<IEnumerable<v_ComplaintActivities>> GetActivitiesByComplaintIdAsync(Guid id);
        Task<IEnumerable<v_ComplaintParties>> GetPartiesByComplaintIdAsync(Guid id);
        Task<IEnumerable<v_ComplaintPermits>> GetPermitsByComplaintIdAsync(Guid id);
        Task<IEnumerable<File>> GetComplaintFilesByComplaintIdAsync(Guid id);
        Task<IEnumerable<Data.Note>> GetNotesById(Guid id);
    }
}
