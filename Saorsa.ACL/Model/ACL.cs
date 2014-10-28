namespace Saorsa.ACL.Model
{
    using System.Collections.Generic;
    using System.Linq;

    public class ACL
    {
        public ACLAction Action { get; set; }
        public virtual List<AclGroup> Groups { get; set; }
        public virtual List<AclUser> Users { get; set; }
        public ACL()
        {
            Groups = new List<AclGroup>();
            Users = new List<AclUser>();
        }
        public ACL(ACLAction action, List<AclUser> users, List<AclGroup> groups)
        {
            Action = action;
            Users = users;
            Groups = groups;
        }
    }
}
