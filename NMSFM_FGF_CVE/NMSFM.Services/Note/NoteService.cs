using NMSFM.Data;
using NMSFM.Services.Audit;
using NMSFM.Services.Logging;
using NMSFM.Services.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMSFM.Services.Note
{
    public class NoteService : INoteService
    {
        private ICodepalWebModel cwmContext;
        private IAuditService auditService;
        private ILogging logger;

        public NoteService(ICodepalWebModel codepalWebModel, ILogging codepalLogger)
        {
            cwmContext = codepalWebModel;
            logger = codepalLogger;
            auditService = new AuditService(logger);
        }

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
                logger.Error("Unexpected exception caught while retrieving the Notes List.", ex);
                result = new List<Data.Note>();
            }
            return result;
        }

        public async Task<bool> CreateNoteAsync(Data.Note note)
        {
            var result = false;
            cwmContext.Notes.Add(note);
            var audit = new AuditModel { TableName = "Notes", RecordId = note.NoteId, AuditAction = "RECORD CREATED", Description = "" };
            var auditFields = new List<AuditFieldModel>();
            auditFields.Add(new AuditFieldModel { ControlName = "UserName", FieldDesc = "UserName", OldId = null, OldValue = null, NewId = null, NewValue = note.UserName });
            auditFields.Add(new AuditFieldModel { ControlName = "Description", FieldDesc = "Description", OldId = null, OldValue = null, NewId = null, NewValue = note.Description });
            auditFields.Add(new AuditFieldModel { ControlName = "Note", FieldDesc = "Note", OldId = null, OldValue = null, NewId = null, NewValue = note.Note1 });
            if (cwmContext is DbContext)
            {
                try
                {
                    await ((DbContext)cwmContext).SaveChangesAsync();
                    await auditService.UpdateAudit(audit, auditFields);
                    result = true;
                }
                catch (Exception ex)
            {
                _ = ex;
                    logger.Error("Unable to create note for id '" + note.RecordId.ToString() + "'.", ex);
                }
            }
            else
            {
                logger.Error("Unable to create note for id '" + note.RecordId.ToString() + "', DbContext was not available.");
            }
            return result;
        }

        public async Task<bool> UpdateNoteAsync(Data.Note note)
        {
            var result = false;
            var noteToUpdate = cwmContext.Notes.SingleOrDefault(a => a.NoteId == note.NoteId);
            var audit = new AuditModel { TableName = "Notes", RecordId = note.NoteId, AuditAction = "RECORD UPDATED", Description = "" };
            var auditFields = new List<AuditFieldModel>();
            if (noteToUpdate.UserName != note.UserName)
            {
                auditFields.Add(new AuditFieldModel { ControlName = "UserName", FieldDesc = "UserName", OldId = null, OldValue = noteToUpdate.UserName, NewId = null, NewValue = note.UserName });
                noteToUpdate.UserName = note.UserName;
            }
            if (noteToUpdate.Description != note.Description)
            {
                auditFields.Add(new AuditFieldModel { ControlName = "Description", FieldDesc = "Description", OldId = null, OldValue = noteToUpdate.Description, NewId = null, NewValue = note.Description });
                noteToUpdate.Description = note.Description;
            }
            if (noteToUpdate.Note1 != note.Note1)
            {
                auditFields.Add(new AuditFieldModel { ControlName = "Note", FieldDesc = "Note", OldId = null, OldValue = noteToUpdate.Note1, NewId = null, NewValue = note.Note1 });
                noteToUpdate.Note1 = note.Note1;
            }
            noteToUpdate.DateUpdated = note.DateUpdated;
            if (cwmContext is DbContext)
            {
                try
                {
                    await ((DbContext)cwmContext).SaveChangesAsync();
                    await auditService.UpdateAudit(audit, auditFields);
                    result = true;
                }
                catch (Exception ex)
            {
                _ = ex;
                    logger.Error("Unable to update note for id '" + note.RecordId.ToString() + "'.", ex);
                }
            }
            else
            {
                logger.Error("Unable to update note for id '" + note.RecordId.ToString() + "', DbContext was not available.");
            }
            return result;
        }

        public async Task<bool> DeleteNoteAsync(Guid noteId)
        {
            var result = false;
            var note = cwmContext.Notes.SingleOrDefault(a => a.NoteId == noteId);
            var audit = new AuditModel { TableName = "Notes", RecordId = noteId, AuditAction = "RECORD DELETED", Description = "" };
            var auditFields = new List<AuditFieldModel>();
            auditFields.Add(new AuditFieldModel { ControlName = "", FieldDesc = "", OldId = noteId, OldValue = null, NewId = null, NewValue = null });
            cwmContext.Notes.Remove(note);
            if (auditFields.Count() > 0)
            {
                await auditService.UpdateAudit(audit, auditFields);
            }
            if (cwmContext is DbContext)
            {
                try
                {
                    await ((DbContext)cwmContext).SaveChangesAsync();
                    result = true;
                }
                catch (Exception ex)
            {
                _ = ex;
                    logger.Error("Unable to remove note " + noteId + ".", ex);
                }
            }
            else
            {
                logger.Error("Unable to remove note, " + noteId + " DbContext was not available.");
            }
            return result;
        }
    }
}

