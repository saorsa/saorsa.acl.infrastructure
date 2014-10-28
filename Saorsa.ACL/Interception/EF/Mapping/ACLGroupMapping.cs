namespace Saorsa.ACL.Interception.EF.Mapping
{
    using System.Data.Entity.ModelConfiguration;
    using Saorsa.ACL.Interception.EF.Model;

    public class ACLGroupMapping : EntityTypeConfiguration<ACLGroup>
    {
        public ACLGroupMapping()
        {
            HasMany(t => t.Users)
                .WithMany(u => u.Groups);
        }
    }
}