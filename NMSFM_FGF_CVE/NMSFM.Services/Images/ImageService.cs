using NMSFM.Data;
using NMSFM.Services.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMSFM.Services.Images
{
    public class ImageService : IImageService
    {
        private ICodepalWebModel cwmContext;
        private ILogging logger;
        private List<string> imageSuffixes = new List<string> { ".bmp", ".gif", ".jpeg", ".png", ".tiff", ".tif", ".jpg", ".pdf", ".doc", ".docx", ".xls", ".xlsx" };

        public ImageService(ICodepalWebModel codepalWebModel, ILogging codepalLogger)
        {
            cwmContext = codepalWebModel;
            logger = codepalLogger;
        }

        public async Task<NMSFM.Data.File> RetrieveImageAsync(Guid id)
        {
            NMSFM.Data.File result = null;
            try
            {
                result = await cwmContext.Files.SingleOrDefaultAsync(f => f.FileId == id);
            }
            catch (Exception ex)
            {
                _ = ex;
                logger.Error("Unexpected exception caught while retrieving the File '" + id.ToString() + "'.", ex);
            }
            return result;
        }

        // Only returning the required fields for the displayed list of links, individual requests are required to get the image data.
        // The full record would include the embedded image, the query just returns a small number of fields to minimize network traffic.
        public async Task<IEnumerable<NMSFM.Data.File>> GetAttachedImageListAsync(Guid id)
        {
            IEnumerable<NMSFM.Data.File> result;
            try
        {
            var itemList = await cwmContext.Files.Where(f => !f.AdminViewOnly && f.FileData != null && f.RecordId == id && !f.Linked && f.FileName != null)
                                           .Select(f => new { FileDesc = f.FileDesc, FileId = f.FileId, SeqNum = f.SeqNum, FileName = f.FileName }).ToListAsync();
            itemList = itemList.Where(f => CheckSuffix(f.FileName.TrimEnd())).ToList();

            return itemList.Select(f => new NMSFM.Data.File
            {
                FileDesc = String.IsNullOrWhiteSpace(f.FileDesc) ? "description unavailable" : f.FileDesc,
                FileId = f.FileId,
                SeqNum = f.SeqNum,
                FileName = f.FileName
            }).ToList();
        }
            catch (Exception ex)
            {
                _ = ex;
                logger.Error("Unexpected exception caught while retrieving the File list.", ex);
                result = new List<NMSFM.Data.File>();
            }
            return result;
        }

        public async Task<IEnumerable<NMSFM.Data.File>> GetAttachedImageListForApplicationsAsync(Guid id, short year)
        {
            IEnumerable<NMSFM.Data.File> result;
            try
            {
                string isoFileDesc = year.ToString() + " - ISO Funding Classification Claim";
                string fpfFileDesc = year.ToString() + " - FPF Rollover Description";

                var itemList = await cwmContext.Files.Where(f => !f.AdminViewOnly && f.FileData != null && f.RecordId == id && !f.Linked && f.FileName != null && (f.FileDesc == isoFileDesc || f.FileDesc == fpfFileDesc))
                                               .Select(f => new { FileDesc = f.FileDesc, FileId = f.FileId, SeqNum = f.SeqNum, FileName = f.FileName }).ToListAsync();
                itemList = itemList.Where(f => CheckSuffix(f.FileName.TrimEnd())).ToList();

                return itemList.Select(f => new NMSFM.Data.File
                {
                    FileDesc = String.IsNullOrWhiteSpace(f.FileDesc) ? "description unavailable" : f.FileDesc,
                    FileId = f.FileId,
                    SeqNum = f.SeqNum,
                    FileName = f.FileName
                }).ToList();
            }
            catch (Exception ex)
            {
                _ = ex;
                logger.Error("Unexpected exception caught while retrieving the File list.", ex);
                result = new List<NMSFM.Data.File>();
            }
            return result;
        }

        public async Task<NMSFM.Data.File> GetAttachedImageByIdAsync(Guid id)
        {
            NMSFM.Data.File result = null;
            try
        {
            var item = await cwmContext.Files.Where(f => f.FileId == id).Select(f => new { FileDesc = f.FileDesc, FileId = f.FileId, SeqNum = f.SeqNum, FileName = f.FileName, FileData = f.FileData }).FirstOrDefaultAsync();
                if (item != null)
                {
                    result = new NMSFM.Data.File
            {
                FileDesc = String.IsNullOrWhiteSpace(item.FileDesc) ? "description unavailable" : item.FileDesc,
                FileId = item.FileId,
                SeqNum = item.SeqNum,
                FileName = item.FileName,
                FileData = item.FileData
            };
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                logger.Error("Unexpected exception caught while retrieving the File '" + id.ToString() + "'.", ex);
            }
            return result;
        }

        public async Task RemoveImageAsync(Guid id)
        {
            if (id != null)
            {
                var result = cwmContext.Files.SingleOrDefault(a => a.FileId == id);
                if (result != null)
                {
                    var fileList = await GetAllImagesForObjectAsync(result.RecordId.Value);
                    if (fileList != null && fileList.Count() >= result.SeqNum)
                    {
                        for (int i = 0; i < fileList.Count(); i++)
                        {
                            if (fileList.ElementAt(i).SeqNum >= result.SeqNum && fileList.ElementAt(i).FileId != result.FileId)
                            {
                                try
                                {
                                    Guid tempGuid = fileList.ElementAt(i).FileId;
                                    var movedFile = await cwmContext.Files.SingleOrDefaultAsync(a => a.FileId == tempGuid);
                                    movedFile.SeqNum--;
                                }
                                catch (Exception ex)
            {
                _ = ex;
                                    logger.Error("Unexpected exception caught while updating the file order.", ex);
                                }
                            }
                        }
                    }

                    cwmContext.Files.Remove(result);
                    if (cwmContext is DbContext)
                    {
                        try
                        {
                            await ((DbContext)cwmContext).SaveChangesAsync();
                        }
                        catch (Exception ex)
            {
                _ = ex;
                            logger.Error("Unable to remove file '" + id + "'.", ex);
                        }
                    }
                    else
                    {
                        logger.Error("Unable to remove file '" + id + "', DbContext was not available.");
                    }
                }
            }
            else
            {
                logger.Error("RemoveImageAsync was called with a null reference.");
            }
        }

        public async Task SaveImageAsync(File file)
        {
            if (file != null)
            {
                var fileList = await GetAllImagesForObjectAsync(file.RecordId.Value);
                if (fileList != null && fileList.Count() >= file.SeqNum)
                {
                    for (int i = 0; i < fileList.Count(); i++)
                    {
                        if (fileList.ElementAt(i).SeqNum >= file.SeqNum)
                        {
                            try
                            {
                                Guid tempGuid = fileList.ElementAt(i).FileId;
                                var movedFile = await cwmContext.Files.SingleOrDefaultAsync(a => a.FileId == tempGuid);
                                movedFile.SeqNum++;
                            }
                            catch (Exception ex)
            {
                _ = ex;
                                logger.Error("Unexpected exception caught while updating the file order.", ex);
                            }                            
                        }
                    }
                }                

                var newFile = cwmContext.Files.Add(new Data.File());
                newFile.FileId = file.FileId;
                newFile.FileName = file.FileName;
                newFile.FileDesc = file.FileDesc;
                newFile.FilePath = file.FilePath;
                newFile.FileData = file.FileData;
                newFile.RecordId = file.RecordId;
                newFile.rowguid = file.rowguid;
                newFile.SeqNum = file.SeqNum;
                newFile.AdminViewOnly = file.AdminViewOnly;
                newFile.DateUpdated = file.DateUpdated;
                newFile.DateInserted = file.DateInserted;

                if (cwmContext is DbContext)
                {
                    try
                    {
                        await ((DbContext)cwmContext).SaveChangesAsync();
                    }
                    catch (Exception ex)
            {
                _ = ex;
                        logger.Error("Unexpected exception caught while saving the file.", ex);
                    }
                }
            }
        }

        private bool CheckSuffix(string filename)
        {
            if (!String.IsNullOrWhiteSpace(filename))
            {
                var lowercaseFileName = filename.ToLower();
                return imageSuffixes.Any(f => lowercaseFileName.EndsWith(f));
            }
            return false;
        }

        private async Task<IEnumerable<NMSFM.Data.File>> GetAllImagesForObjectAsync(Guid id) // This is a copy of 'GetAttachedImageListAsync' except is also returns AdminViewOnly and Linked images
        {
            IEnumerable<NMSFM.Data.File> result;
            try
            {
                var itemList = await cwmContext.Files.Where(f => f.RecordId == id)
                                               .Select(f => new { FileDesc = f.FileDesc, FileId = f.FileId, SeqNum = f.SeqNum, FileName = f.FileName }).ToListAsync();
                itemList = itemList.Where(f => CheckSuffix(f.FileName.TrimEnd())).ToList();

                result = itemList.Select(f => new NMSFM.Data.File
                {
                    FileDesc = String.IsNullOrWhiteSpace(f.FileDesc) ? "description unavailable" : f.FileDesc,
                    FileId = f.FileId,
                    SeqNum = f.SeqNum,
                    FileName = f.FileName
                }).ToList();
            }
            catch (Exception ex)
            {
                _ = ex;
                logger.Error("Unexpected exception caught while retrieving the File list.", ex);
                result = new List<NMSFM.Data.File>();
            }
            return result;
        }
    }
}

