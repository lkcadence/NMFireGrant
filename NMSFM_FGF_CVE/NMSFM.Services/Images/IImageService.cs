using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NMSFM.Data;

namespace NMSFM.Services.Images
{
    public interface IImageService
    {
        Task<File> GetAttachedImageByIdAsync(Guid id);
        Task<IEnumerable<File>> GetAttachedImageListAsync(Guid id);
        Task<IEnumerable<File>> GetAttachedImageListForApplicationsAsync(Guid id, short year);
        Task RemoveImageAsync(Guid id);
        Task SaveImageAsync(File file);
        Task<File> RetrieveImageAsync(Guid id);
    }
}