using System.Data.Entity;

namespace NMSFM.Data
{	
	public partial class UserWebModel : DbContext, IUserWebModel
	{
		public UserWebModel()
			: base("name=CodepalWebModel")
		{
		}
		public UserWebModel(string connectionString) : base(connectionString)
		{
		}

		public virtual DbSet<User> Users { get; set; }
		public virtual DbSet<News> News { get; set; }
		public virtual DbSet<Inspector> Inspectors { get; set; }
		public virtual DbSet<License> Licenses { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>()
				.Property(e => e.Login)
				.IsUnicode(false);

			modelBuilder.Entity<User>()
				.Property(e => e.Password)
				.IsUnicode(false);

			modelBuilder.Entity<User>()
				.Property(e => e.ConnectionString)
				.IsUnicode(false);

			modelBuilder.Entity<User>()
				.Property(e => e.DatabaseName)
				.IsUnicode(false);

			modelBuilder.Entity<User>()
				.Property(e => e.Organization)
				.IsUnicode(false);

			modelBuilder.Entity<News>()
				.Property(e => e.NewsTitle)
				.IsUnicode(false);

			modelBuilder.Entity<News>()
				.Property(e => e.NewsText)
				.IsUnicode(false);

			modelBuilder.Entity<Inspector>()
				.Property(e => e.Code)
				.IsUnicode(false);

			modelBuilder.Entity<Inspector>()
				.Property(e => e.InspectorName)
				.IsUnicode(false);

			modelBuilder.Entity<Inspector>()
				.Property(e => e.Login)
				.IsUnicode(false);

			modelBuilder.Entity<Inspector>()
				.Property(e => e.Password)
				.IsUnicode(false);

			modelBuilder.Entity<Inspector>()
				.Property(e => e.InspectorPhone)
				.IsUnicode(false);

			modelBuilder.Entity<Inspector>()
				.Property(e => e.ExternalId)
				.IsUnicode(false);

			modelBuilder.Entity<Inspector>()
				.Property(e => e.Email)
				.IsUnicode(false);

			modelBuilder.Entity<Inspector>()
				.Property(e => e.RCLevel)
				.IsUnicode(false);

			modelBuilder.Entity<Inspector>()
				.Property(e => e.ActiveModules)
				.IsUnicode(false);

			modelBuilder.Entity<Inspector>()
				.Property(e => e.SecQOne)
				.IsUnicode(false);

			modelBuilder.Entity<Inspector>()
				.Property(e => e.SecAOne)
				.IsUnicode(false);

			modelBuilder.Entity<Inspector>()
				.Property(e => e.SecQTwo)
				.IsUnicode(false);

			modelBuilder.Entity<Inspector>()
				.Property(e => e.SecATwo)
				.IsUnicode(false);

			modelBuilder.Entity<Inspector>()
				.Property(e => e.Title)
				.IsUnicode(false);

			modelBuilder.Entity<License>()
				.Property(e => e.Licensee)
				.IsUnicode(false);

			modelBuilder.Entity<License>()
				.Property(e => e.LicenseKey)
				.IsUnicode(false);
		}
	}
}
