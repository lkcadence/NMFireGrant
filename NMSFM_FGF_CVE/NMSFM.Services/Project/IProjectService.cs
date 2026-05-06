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

namespace NMSFM.Services.Project
{
    public interface IProjectService
    {
        Task<IEnumerable<v_Projects>> GetProjectsAsync();
        Task<List<ProjectType>> GetProjectTypeAsync();

        Task<List<ProjectStatu>> GetProjectStatus();
        Task<IEnumerable<v_Agreements>> GetProjectAgreementsByProjectIdAsync(Guid id);
        Task<IEnumerable<v_ProjectAddressSearch>> GetProjectAddressesByProjectIdAsync(Guid id);
        Task<IEnumerable<v_ProjectActivitySearch>> GetProjectActivityByProjectIdAsync(Guid id);
        Task<IEnumerable<v_ProjectPermitSearch>> GetProjectPermitByProjectIdAsync(Guid id);
        Task<IEnumerable<v_ProjectRequestSearch>> GetProjectRequestByProjectIdAsync(Guid id);
        Task<IEnumerable<v_ProjectInspectorSearch>> GetProjectInspectorByProjectIdAsync(Guid id);
        Task<IEnumerable<v_Files>> GetProjectFilesByProjectIdAsync(Guid id);
        Task<IEnumerable<v_Fees>> GetProjectFeesByProjectIdAsync(Guid id);
        Task<IEnumerable<Data.Note>> GetNotesByIdAsync();
        Task<IEnumerable<v_Signature>> GetSignaturebyProjectId(Guid id);

    }
}
