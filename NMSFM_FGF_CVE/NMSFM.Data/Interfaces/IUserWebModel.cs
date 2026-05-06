using System.Data.Entity;

namespace NMSFM.Data
{	
	public interface IUserWebModel 
	{		
		DbSet<User> Users { get; set; }
		DbSet<News> News { get; set; }
		DbSet<Inspector> Inspectors { get; set; }
		DbSet<License> Licenses { get; set; }		
	}
}
