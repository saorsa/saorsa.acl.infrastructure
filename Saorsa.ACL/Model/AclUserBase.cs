namespace Saorsa.ACL.Model
{
    public abstract class AclUserBase
    {
        public AclUserBase()
        {
            
        }
        public AclUserBase(string id)
        {
            Id = id;
        }

        public virtual string Id { get; set; }
        public override bool Equals(object obj)
        {
            return Id == ((AclUserBase) obj).Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
