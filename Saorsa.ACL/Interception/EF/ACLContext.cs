namespace Saorsa.BS.Domain
{
    using System.Data.Entity;
    using System.Diagnostics;
    using Saorsa.ACL.Interception.EF.Mapping;
    using Saorsa.ACL.Interception.EF.Model;

    public class AclContext : DbContext
    {
        public AclContext()
            : base("Name=AclContext")
        {
#if DEBUG
            Database.Log = (e) => Trace.WriteLine(e);
#endif
        }

        public AclContext(string connectionString)
            : base(connectionString)
        {
        }

        public DbSet<ACLGroup> Groups { get; set; }
        public DbSet<ACLUser> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ACLGroupMapping());
            modelBuilder.Configurations.Add(new ACLUserMapping());
        }

    }
}