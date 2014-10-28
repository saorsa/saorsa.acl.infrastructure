using System.Collections.Generic;

namespace Saorsa.ACL.Interception.Helpers
{
    using Saorsa.ACL.Model;

    public class AclUsersComparer : IEqualityComparer<AclUser>
    {
        public bool Equals(AclUser x, AclUser y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(AclUser obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
