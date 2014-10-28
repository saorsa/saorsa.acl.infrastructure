namespace Saorsa.ACL.Interception.EF.Model
{
    using System.Collections.Generic;
    using Saorsa.ACL.Model;

    public class ACLGroup : AclGroup
    {
        public string Name { get; set; }
        public ICollection<ACLUser> Users { get; set; }
    }
}
