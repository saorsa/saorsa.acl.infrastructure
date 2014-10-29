namespace Saorsa.ACL.Model
{
    using System.Collections.Generic;
    /// <summary>
    /// Describes an access control list containing of:
    /// Action (Read/Write, etc...)
    /// List of users allowed to perform the action
    /// List of groups allowed to perform the action
    /// </summary>
    public class ACL
    {
        public virtual ACLAction Action { get; set; }
        /// <summary>
        /// A list of Groups that can perform the action
        /// </summary>
        public virtual List<AclGroup> G { get; set; }
        /// <summary>
        /// A list of users that can perform the action
        /// </summary>
        public virtual List<AclUser> U { get; set; }
        public ACL()
        {
            G = new List<AclGroup>();
            U = new List<AclUser>();
        }
        public ACL(ACLAction action, List<AclUser> users, List<AclGroup> groups)
        {
            Action = action;
            U = users;
            G = groups;
        }
    }
}
