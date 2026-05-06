using NMSFM.Data;
using NMSFM.Services.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace NMSFM.Services.Signature
{
	public class SignatureService : ISignatureService
	{
		private ICodepalWebModel cwmContext;
		private ILogging logger;

		public SignatureService(ICodepalWebModel codepalWebModel, ILogging codepalLogger)
		{
			cwmContext = codepalWebModel;
			logger = codepalLogger;
		}


		public async Task SaveSignatureAsync(byte[] sigImage, Guid sigTypeId, string sigName, Guid objectId)
		{
			if (sigImage != null)
			{

				var sigCount = (await cwmContext.Signatures.Where(s => s.RecordId == objectId).ToListAsync()).Count() + 1;

				var newSignature = cwmContext.Signatures.Add(new Data.Signature());
				newSignature.SignatureId = Guid.NewGuid();
				newSignature.FileData = sigImage;
				newSignature.RecordId = objectId;
				newSignature.PrintedName = sigName;
				newSignature.Sequence = sigCount;
				newSignature.SignatureTypeId = sigTypeId;
				newSignature.DateSigned = DateTime.Now;
				newSignature.SignatureData = null;

				newSignature.rowguid = Guid.NewGuid();
				newSignature.DateInserted = newSignature.DateSigned ?? DateTime.Now;
				newSignature.DateUpdated = newSignature.DateInserted;
				newSignature.Inactive = false;

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

		public async Task<IEnumerable<SignatureType>> GetSignatureTypeListAsync(string moduleType)
		{
			IEnumerable<SignatureType> result;
			try
			{
				var agencyId = (Guid)System.Web.HttpContext.Current.Session["AgencyId"];

				var modules = (await cwmContext.Modules.Where(m => m.ModuleDesc == moduleType).ToListAsync()).Select(m => m.ModuleId.ToString());

				result = await cwmContext.SignatureTypes.Where(s => !s.Inactive && s.WebViewable && (s.ModuleId == moduleType || modules.Contains(s.ModuleId)) && (s.AgencyId == agencyId || s.AgencyId == null)).ToListAsync();

			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Signature Type List.", ex);
				result = new List<SignatureType>();
			}
			return result;
		}

		public async Task<IEnumerable<Data.Signature>> GetAttachedSignaturesAsync(Guid recordId)
		{
			IEnumerable<Data.Signature> result;
			try
			{
				result = await cwmContext.Signatures.Where(a => a.Inactive == false && a.RecordId == recordId).ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Signature List.", ex);
				result = new List<Data.Signature>();
			}
			return result;
		}

		public async Task<string> GetSignatureImageAsync(Guid signatureId)
		{
			string result = "";
			try
			{
				var sig = await cwmContext.Signatures.Where(a => a.SignatureId == signatureId).FirstOrDefaultAsync();
				if (sig != null)
				{
					result = Convert.ToBase64String(sig.FileData);

				}

			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Signature List.", ex);

			}
			return result;
		}

		public async Task<bool> DeleteAllSignaturesForObject(Guid objectId)
		{
			bool result = false;
			var sigs = (await cwmContext.Signatures.Where(s => s.RecordId == objectId).ToListAsync()).AsEnumerable();
			if (sigs != null && sigs.Count() > 0)
			{
				cwmContext.Signatures.RemoveRange(sigs);
				result = true;
			}
			return result;
		}

		public async Task<bool> DeleteSignature(Guid signatureId)
		{
			bool result = false;
			var sig = await cwmContext.Signatures.FirstAsync(s => s.SignatureId == signatureId);

			if (sig != null)
			{
				cwmContext.Signatures.Remove(sig);
				result = true;
			}
			return result;
		}
	}
}

