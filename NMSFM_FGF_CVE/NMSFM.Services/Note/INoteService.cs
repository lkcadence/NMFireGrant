using System.Collections.Generic;
using System.Threading.Tasks;
using NMSFM.Data;
using System;
using System.Web.Mvc;
using NMSFM.Services.Models;
using NMSFM.ViewModels;

namespace NMSFM.Services.Note
{
    public interface INoteService
    {
        Task<IEnumerable<Data.Note>> GetNotesByIdAsync(Guid id);
        Task<bool> CreateNoteAsync(Data.Note note);
        Task<bool> UpdateNoteAsync(Data.Note note);
        Task<bool> DeleteNoteAsync(Guid noteId);
    }
}
