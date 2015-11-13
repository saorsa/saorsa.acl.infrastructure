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
            var @base = obj as AclUserBase;
            if (@base != null)
            {
                return Id == @base.Id;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
