namespace Saorsa.ACL.Model
{
    public class AclUser : AclUserBase
    {
        public AclUser()
        {
            
        }

        public AclUser(string id) : base(id)
        {
            
        }

        public override bool Equals(object obj)
        {
            if (obj is AclUser)
                return Id == ((AclUser)obj).Id;
            return base.Equals(obj);
        }

        public AclUser ShallowCopy()
        {
            return new AclUser
            {
                Id = Id
            };
        }
    }
}