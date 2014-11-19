namespace Saorsa.ACL.Interception.EF.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using Saorsa.ACL.Model;
    [Table("AclUsers")]
    public class ACLUser : AclUserBase
    {
        public ICollection<ACLGroup> Groups { get; set; }
    }
}
