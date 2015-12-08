
namespace Saorsa.ACL.Interception.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;
    using Saorsa.ACL.Model;

    public static class AssertAclRightsHelper
    {
        public static bool CanWrite(string entityAcl, string userId, IEnumerable<AclGroup> userGroups)
        {
            var acl = JsonConvert.DeserializeObject<List<ACL>>(entityAcl);
            return acl.Any(a => a.Action == ACLAction.Write &&
                                (
                                    (a.U!=null && a.U.Any(u => u.Id == userId)) ||
                                    (a.G!= null && userGroups != null && a.G.Any(g=>userGroups.Any(ug=>ug.Id == g.Id)))
                                )
                );
        }
        public static bool CanRead(string entityAcl, string userId, IEnumerable<AclGroup> userGroups)
        {
            if (entityAcl == null)
            {
                return false;
            }
            var acl = JsonConvert.DeserializeObject<List<ACL>>(entityAcl);
            return acl.Any(a => (a.Action == ACLAction.Write || a.Action == ACLAction.Read) &&
                                (
                                    (a.U!=null && a.U.Any(u => u.Id == userId)) ||
                                    (a.G != null && userGroups != null && a.G.Any(g => userGroups.Any(ug => ug.Id == g.Id)))
                                )
                );
        }
    }
}
