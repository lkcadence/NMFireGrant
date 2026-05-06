using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using NMSFM.Data;
using NMSFM.ViewModels;

namespace NMSFM.Services.Signature
{
    public interface ISignatureService
    {        
        Task SaveSignatureAsync(byte[] sigImage, Guid sigTypeId, string sigName, Guid objectId);
        Task<IEnumerable<SignatureType>> GetSignatureTypeListAsync(string moduleId);
        Task<IEnumerable<Data.Signature>> GetAttachedSignaturesAsync(Guid recordId);
        Task<string> GetSignatureImageAsync(Guid signatureId);
        Task<bool> DeleteAllSignaturesForObject(Guid objectId);
        Task<bool> DeleteSignature(Guid signatureId);
    }
}