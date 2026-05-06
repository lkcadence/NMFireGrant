using System.Data.Entity;

namespace NMSFM.Data
{	
	public interface IAuditWebModel
	{		

		DbSet<AuditField> AuditFields { get; set; }
		DbSet<Audit> Audits { get; set; }
		DbSet<AuditSession> AuditSessions { get; set; }

	}
}
