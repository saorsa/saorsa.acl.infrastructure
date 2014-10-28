namespace Saorsa.ACL.Model
{
    public class AclUser
    {
        public AclUser()
        {
            
        }
        public AclUser(string id)
        {
            Id = id;
        }

        public virtual string Id { get; set; }
        public override bool Equals(object obj)
        {
            return Id == ((AclUser) obj).Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
