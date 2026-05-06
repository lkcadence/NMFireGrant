namespace NMSFM.Data
{
	using System;
	using System.Data.Entity;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;

	public partial class AuditWebModel : DbContext, IAuditWebModel
	{
		public AuditWebModel()
			: base("name=CodepalWebModel")
		{
		}

		public AuditWebModel(string connectionString) : base(connectionString)
		{
		}

		public virtual DbSet<AuditField> AuditFields { get; set; }
		public virtual DbSet<Audit> Audits { get; set; }
		public virtual DbSet<AuditSession> AuditSessions { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<AuditField>()
				.Property(e => e.ControlName)
				.IsUnicode(false);

			modelBuilder.Entity<AuditField>()
				.Property(e => e.FieldDesc)
				.IsUnicode(false);

			modelBuilder.Entity<AuditField>()
				.Property(e => e.OldId)
				.IsUnicode(false);

			modelBuilder.Entity<AuditField>()
				.Property(e => e.OldValue)
				.IsUnicode(false);

			modelBuilder.Entity<AuditField>()
				.Property(e => e.NewId)
				.IsUnicode(false);

			modelBuilder.Entity<AuditField>()
				.Property(e => e.NewValue)
				.IsUnicode(false);

			modelBuilder.Entity<Audit>()
				.Property(e => e.TableName)
				.IsUnicode(false);

			modelBuilder.Entity<Audit>()
				.Property(e => e.AuditAction)
				.IsUnicode(false);

			modelBuilder.Entity<Audit>()
				.Property(e => e.Description)
				.IsUnicode(false);

			modelBuilder.Entity<AuditSession>()
				.Property(e => e.ComputerName)
				.IsUnicode(false);

			modelBuilder.Entity<AuditSession>()
				.Property(e => e.ComputerIP)
				.IsUnicode(false);

			modelBuilder.Entity<AuditSession>()
				.Property(e => e.WindowsUser)
				.IsUnicode(false);

			modelBuilder.Entity<AuditSession>()
				.Property(e => e.UserName)
				.IsUnicode(false);
		}
	}
}
