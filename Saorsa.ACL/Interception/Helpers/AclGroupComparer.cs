namespace Saorsa.ACL.Interception.Helpers
{
    using System.Collections.Generic;
    using Saorsa.ACL.Model;

    public class AclGroupComparer : IEqualityComparer<AclGroup>
    {
        public bool Equals(AclGroup x, AclGroup y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(AclGroup obj)
        {
            return obj.Id;
        }
    }
}