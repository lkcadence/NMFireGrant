using System.Collections.Generic;
using System.Threading.Tasks;
using NMSFM.Data;
using System;
using NMSFM.Services.Models;
using NMSFM.ViewModels;


namespace NMSFM.Services.Account
{
	public interface IAccountService
	{
		Task<User> GetWebUserByInfoAsync(string userName, string password);
		Task<User> GetDuplicateWebUserByInfoAsync(User user);
		Task<User> GetWebUserByIdAsync(Guid userId);
		Task<User> GetWebUserByEmailAsync(string email);
		Task<bool> SaveWebUserAsync(User user);
		Task<bool> UpdateExistingUser(User user);
		Task<List<News>> GetNewsListAsync();
		Task<bool> UserLoginAsync(Guid userId);
		bool CheckUserLogin();
		Task<int> CheckLiscenseLimitAsync();
		Task<List<User>> GetUserList();
		Task<bool> ChangePasswordAsync(string oldPass, string newPass);
		Task<string> GetAdminUserConnectionAsync(string id);
		Task<string> GetAdminUserIdAsync();
		string EncryptString(string baseString);
		Task<string> EncryptStringAsync(string baseString);
		Task<string> DecryptString(string encryptedString);
		string DecryptStringNoAsync(string encryptedString);
	}
}
