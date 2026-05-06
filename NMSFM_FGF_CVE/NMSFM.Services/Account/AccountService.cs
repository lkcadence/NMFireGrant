using NMSFM.Data;
using System;
using System.Web;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using NMSFM.Services.Models;
using log4net;
using NMSFM.Services.Logging;
using System.Security.Cryptography;
using NMSFM.ViewModels;
using AutoMapper;
using System.IO;
using NMSFM.Services.Address;

namespace NMSFM.Services.Account
{
	public class AccountService : IAccountService
	{
		private IUserWebModel uwmContext;
		private ILogging logger;

		public AccountService(IUserWebModel userWebModel, ILogging codepalLogger)
		{
			uwmContext = userWebModel;
			logger = codepalLogger;
		}

		public async Task<User> GetWebUserByInfoAsync(string userName, string password)
		{
			User result = null;
			if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
			{
				try
				{
					result = await uwmContext.Users.SingleOrDefaultAsync(a => a.Login == userName && a.Password == password && a.DatabaseName== "Codepal_NMSFM" && a.NMFGA == true);
				}
				catch (Exception ex)
            {
                _ = ex;
					logger.Error("Unexpected exception caught while retrieving the user '" + userName + "'.", ex);
				}
			}
			else
			{
				logger.Error("GetPartyWebAccessByInfoAsync was called with null arguments.");
			}
			return result;
		}

		public async Task<User> GetWebUserByIdAsync(Guid userId)
		{
			User result = null;
			if (userId != null && userId != Guid.Empty)
			{
				try
				{
					result = await uwmContext.Users.SingleOrDefaultAsync(a => a.UserId == userId);
				}
				catch (Exception ex)
            {
                _ = ex;
					logger.Error("Unexpected exception caught while retrieving the user '" + userId + "'.", ex);
				}
			}
			else
			{
				logger.Error("GetPartyWebAccessByInfoAsync was called with null arguments.");
			}
			return result;
		}

		public async Task<User> GetWebUserByEmailAsync(string email)
		{
			User result = null;
			if (email != null && email != String.Empty)
			{
				try
				{
					result = await uwmContext.Users.SingleOrDefaultAsync(a => a.Email == email);
				}
				catch (Exception ex)
            {
                _ = ex;
					logger.Error("Unexpected exception caught while retrieving the user '" + email + "'.", ex);
				}
			}
			else
			{
				logger.Error("GetPartyWebAccessByInfoAsync was called with null arguments.");
			}
			return result;
		}

		public async Task<List<User>> GetWebUserListByConnectionAsync(string connectionString)
		{
			List<User> result = null;
			if (connectionString != null)
			{
				try
				{
					result = await uwmContext.Users.Where(a => a.ConnectionString == connectionString && a.IsWebAdmin == false).OrderBy(a => a.Login).ToListAsync();
				}
				catch (Exception ex)
            {
                _ = ex;
					logger.Error("Unexpected exception caught while retrieving the users.", ex);
				}
			}
			return result;
		}

		public async Task<User> GetDuplicateWebUserByInfoAsync(User user)
		{
			User result = null;
			var webUser = user;
			if (user != null && user.Login != null)
			{
				try
				{
					if (user.UserId != Guid.Empty)
					{
						result = await uwmContext.Users.SingleOrDefaultAsync(a => (a.Login == user.Login && a.UserId != webUser.UserId) || a.Email == user.Email) ;
					}
					else
					{
						result = await uwmContext.Users.SingleOrDefaultAsync(a => a.Login == user.Login || a.Email == user.Email);
					}
				}
				catch (Exception ex)
            {
                _ = ex;
					logger.Error("Unexpected exception caught while retrieving the User '" + user.Login.ToString() + "'.", ex);
				}
			}
			else
			{
				logger.Error("GetPartyWebAccessByInfoAsync was called with null arguments.");
			}
			return result;
		}

		public async Task<bool> SaveWebUserAsync(User user)
		{
			var result = true;
			//Encrypted Connection String Code
			var userContext = new UserWebModel(HttpContext.Current.Session["userConnection"].ToString());
			//End Encrypted Connection String Code

			//Commented Out for encrypted connection string
			//var userContext = new UserWebModel(user.ConnectionString);
			//End Comment
			var service = new AccountService(userContext, logger);
			//var limit = await service.CheckLiscenseLimitAsync();
			var inspectors = await userContext.Inspectors.Select(a => a.InspectorId).ToListAsync();
			var existingUser = await uwmContext.Users.Where(a => inspectors.Contains(a.CodepalId ?? Guid.Empty) && a.ConnectionString == user.ConnectionString && (a.Inactive == null || a.Inactive == false)).ToListAsync();
			// Took out to allow many users. Currently not limited to the number licensed.  lok - 04/30/18
			//if (existingUser.Count() >= limit)
			//{
			//    return false;
			//}

			var webUser = new User();
			webUser = uwmContext.Users.Add(new Data.User());
			webUser.UserId = Guid.NewGuid();
			webUser.Login = user.Login;
			webUser.Password = user.Password;
			webUser.Email = user.Email;
			webUser.ConnectionString = user.ConnectionString;
			webUser.DateUpdated = DateTime.Now;
			webUser.DateInserted = DateTime.Now;
			webUser.CodepalId = user.CodepalId;
			webUser.IsWebAdmin = false;
			webUser.DatabaseName = user.DatabaseName;
			webUser.Organization = user.Organization;
			webUser.AHJUser = false;
			webUser.ClientUser = user.ClientUser;
			webUser.ThirdPartyUser = false;
			webUser.PublicUser = false;
			webUser.Readonly = user.Readonly;
			webUser.CPTK = user.CPTK;
			webUser.NMFPF = user.NMFPF;
			webUser.NMFGA = user.NMFGA;
			webUser.NMFGC = user.NMFGC;
			webUser.Inactive = user.Inactive;


			if (uwmContext is DbContext)
			{
				try
				{
					await ((DbContext)uwmContext).SaveChangesAsync();
				}
				catch (Exception ex)
            {
                _ = ex;
					logger.Error("Unexpected exception caught while saving the web user.", ex);
				}
			}
			return result;
		}

		public async Task<bool> UpdateExistingUser(User user)
		{
			var result = true;
			var webUser = await uwmContext.Users.SingleOrDefaultAsync(a => a.UserId == user.UserId);
			if ((user.Inactive == false || user.Inactive == null) && webUser.Inactive == true)
			{
				var userContext = new UserWebModel((string)HttpContext.Current.Session["userConnection"]);// Connection
				var service = new AccountService(userContext, logger);
				//var limit = await service.CheckLiscenseLimitAsync();
				var inspectors = await userContext.Inspectors.Select(a => a.InspectorId).ToListAsync();
				var existingUser = await uwmContext.Users.Where(a => inspectors.Contains(a.CodepalId ?? Guid.Empty) && a.ConnectionString == user.ConnectionString && (a.Inactive == null || a.Inactive == false)).ToListAsync();
				//if (existingUser.Count() >= limit)
				//{
				//	return false;
				//}
			}
			if (!string.IsNullOrWhiteSpace(user.Login))
			{
				webUser.Login = user.Login;
			}
			if (!string.IsNullOrWhiteSpace(user.Password) && user.Password != "vuHuH2EPS9Q=") // vuHuH2EPS9Q= is an encrypted empty string
			{
				webUser.Password = user.Password;
			}
			webUser.Inactive = user.Inactive;
			webUser.DateUpdated = DateTime.Now;
			webUser.Email = user.Email;
			webUser.ForgotPasswordToken = user.ForgotPasswordToken;
			webUser.Readonly = user.Readonly;

			if (uwmContext is DbContext)
			{
				try
				{
					await ((DbContext)uwmContext).SaveChangesAsync();
				}
				catch (Exception ex)
            {
                _ = ex;
					logger.Error("Unexpected exception caught while saving the web user.", ex);
				}
			}
			return result;
		}

		public async Task<List<News>> GetNewsListAsync()
		{
			List<News> result = null;
			try
			{
				result = await uwmContext.News.Where(a => a.Inactive == null || a.Inactive == false).OrderByDescending(a => a.DateUpdated).ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the party list.", ex);
			}
			return result;
		}

		public async Task<bool> UserLoginAsync(Guid userId)
		{
			var result = false;
			try
			{
				var user = uwmContext.Users.SingleOrDefault(a => a.UserId == userId);
				user.LastSessionId = (Guid)HttpContext.Current.Session["SessionId"];
				user.LastLoginDate = DateTime.Now;

				if (uwmContext is DbContext)
				{
					await ((DbContext)uwmContext).SaveChangesAsync();
					result = true;
				}
				else
				{
					logger.Error("Unable to secure the user login. DbContext was not available.");
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while securing the user login.", ex);
			}

			return result;
		}

		public bool CheckUserLogin()
		{
			var result = false;
			try
			{
				var userId = (Guid?)HttpContext.Current.Session["WebUserId"];
				var sessionId = (Guid?)HttpContext.Current.Session["SessionId"];
				if (userId != null && sessionId != null)
				{
					var user = uwmContext.Users.SingleOrDefault(a => a.UserId == userId.Value && a.LastSessionId == sessionId);
					if (user != null)
					{
						result = true;
					}
				}
                else
                {
					logger.Error("CheckUserLogin: Session no longer active.");
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while confirming the user's session.", ex);
			}
			return result;
		}

		public async Task<int> CheckLiscenseLimitAsync()
		{
			var result = 0;
			var license = await uwmContext.Licenses.SingleOrDefaultAsync();
			if (license != null)
			{
				var licenseKey = license.LicenseKey;
				licenseKey = licenseKey.Replace(" ", "+");
				byte[] byKey = { 7, 2, 0, 1, 5, 6, 7, 8, 9, 1, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 24, 24 };	  
				byte[] IV = { 1, 0, 2, 4, 1, 9, 7, 5 };
				TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
				byte[] inputByteArray = Convert.FromBase64String(licenseKey);
				MemoryStream ms = new MemoryStream();
				CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
				cs.Write(inputByteArray, 0, inputByteArray.Length);
				cs.FlushFinalBlock();
				var decodedKey = System.Text.UTF8Encoding.UTF8.GetString(ms.ToArray());
				var keyParts = decodedKey.Split('!');
				Int32.TryParse(keyParts[4], out result);
			}
			return result;
		}

		public async Task<string> GetAdminUserIdAsync()
		{
			var result = "";
			try
			{
				//Encrypted Connection String Code
				var userConnection = (string)HttpContext.Current.Session["userConnectionEncrypted"];
				//End Encrypted Connection String Code

				//Commented Out for encrypted connection string
				//var userConnection = (string)System.Web.HttpContext.Current.Session["userConnection"];
				//End Comment
				var userId = await uwmContext.Users.SingleOrDefaultAsync(a => a.ConnectionString == userConnection && a.IsWebAdmin == true);
				result = userId != null ? userId.UserId.ToString() : "";
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the admin user Id.", ex);
			}
			return result;
		}

		public async Task<string> GetAdminUserConnectionAsync(string id)
		{
			var result = "";
			var userId = Guid.Parse(id);
			try
			{
				var user = await uwmContext.Users.SingleOrDefaultAsync(a => a.UserId == userId && a.IsWebAdmin == true);
				result = user != null ? user.ConnectionString : "";
			}			
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the admin user connection.", ex);
			}
			return result;
		}

		public async Task<List<User>> GetUserList()
		{
			var results = new List<User>();
			try
			{
				//Encrypted Connection String Code
				var connectionString = (string)HttpContext.Current.Session["userConnectionEncrypted"];
				//End Encrypted Connection String Code

				//Commented Out for encrypted connection string
				//var connectionString = (string)HttpContext.Current.Session["userConnection"];
				//End Comment
				results = await uwmContext.Users.Where(a => a.ConnectionString == connectionString && a.IsWebAdmin == false && a.NMFGA == true).ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the user list.", ex);
			}
			return results;
		}

		public async Task<bool> ChangePasswordAsync(string oldPass, string newPass)
		{
			try
			{
				var userId = (Guid?)HttpContext.Current.Session["WebUserId"];
				var sessionId = (Guid)HttpContext.Current.Session["SessionId"];
				var user = uwmContext.Users.SingleOrDefault(a => a.UserId == userId.Value && a.LastSessionId == sessionId && a.Password == oldPass);
				if (user != null)
				{
					user.Password = newPass;
					if (uwmContext is DbContext)
					{
						await ((DbContext)uwmContext).SaveChangesAsync();
						return true;
					}
					else
					{
						logger.Error("Unable to update the user password. DbContext was not available.");
					}
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while updating the user password.", ex);
			}
			return false;
		}

		public string EncryptString(string baseString)
		{
			byte[] byKey = { 7, 2, 0, 1, 5, 6, 7, 8, 9, 1, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 24, 24 };
			byte[] IV = { 1, 0, 2, 4, 1, 9, 7, 5 };
			TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
			byte[] inputByteArray = System.Text.UTF8Encoding.UTF8.GetBytes(baseString);
			MemoryStream ms = new MemoryStream();
			CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
			cs.Write(inputByteArray, 0, inputByteArray.Length);
			cs.FlushFinalBlock();
			var encryptString = Convert.ToBase64String(ms.ToArray());

			return encryptString;
		}

		public async Task<string> EncryptStringAsync(string baseString)
		{
			byte[] byKey = { 7, 2, 0, 1, 5, 6, 7, 8, 9, 1, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 24, 24 };
			byte[] IV = { 1, 0, 2, 4, 1, 9, 7, 5 };
			TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
			byte[] inputByteArray = System.Text.UTF8Encoding.UTF8.GetBytes(baseString);
			MemoryStream ms = new MemoryStream();
			CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
			cs.Write(inputByteArray, 0, inputByteArray.Length);
			cs.FlushFinalBlock();
			var encryptString = Convert.ToBase64String(ms.ToArray());

			return encryptString;
		}

		public async Task<string> DecryptString(string encryptedString)
		{
			string baseString = "";
			encryptedString = encryptedString.Replace(" ", "+");
			byte[] byKey = { 7, 2, 0, 1, 5, 6, 7, 8, 9, 1, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 24, 24 };
			byte[] IV = { 1, 0, 2, 4, 1, 9, 7, 5 };
			TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
			byte[] inputByteArray = Convert.FromBase64String(encryptedString);
			MemoryStream ms = new MemoryStream();
			CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
			cs.Write(inputByteArray, 0, inputByteArray.Length);
			cs.FlushFinalBlock();
			baseString = System.Text.UTF8Encoding.UTF8.GetString(ms.ToArray());
			return baseString;
		}

		public string DecryptStringNoAsync(string encryptedString)
		{
			string baseString = "";
			encryptedString = encryptedString.Replace(" ", "+");
			byte[] byKey = { 7, 2, 0, 1, 5, 6, 7, 8, 9, 1, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 24, 24 };
			byte[] IV = { 1, 0, 2, 4, 1, 9, 7, 5 };
			TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
			byte[] inputByteArray = Convert.FromBase64String(encryptedString);
			MemoryStream ms = new MemoryStream();
			CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
			cs.Write(inputByteArray, 0, inputByteArray.Length);
			cs.FlushFinalBlock();
			baseString = System.Text.UTF8Encoding.UTF8.GetString(ms.ToArray());
			return baseString;
		}
	}
}
