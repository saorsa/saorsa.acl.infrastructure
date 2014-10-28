namespace Saorsa.ACL.Interception.EF.Model
{
    using System.Collections.Generic;
    using Saorsa.ACL.Model;

    public class ACLUser : AclUser
    {
        public ICollection<ACLGroup> Groups { get; set; }
    }
}
