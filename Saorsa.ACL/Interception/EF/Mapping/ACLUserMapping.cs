namespace Saorsa.ACL.Interception.EF.Mapping
{
    using System.Data.Entity.ModelConfiguration;
    using Saorsa.ACL.Interception.EF.Model;

    public class ACLUserMapping : EntityTypeConfiguration<ACLUser>
    {
        public ACLUserMapping()
        {
            HasMany(t => t.Groups)
                .WithMany(u => u.Users);
        }
    }
}