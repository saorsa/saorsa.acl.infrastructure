namespace Saorsa.ACL.Interception
{
    using Saorsa.ACL.Interception.EF.Model;
    using Saorsa.ACL.Model;

    public interface IAcl
    {
        string UserId { get; set; }
        //Should be implemented for inserts and updates
        //http://stackoverflow.com/questions/9054609/how-to-select-a-single-column-with-entity-framework
        string GetEntityAcl<T>(T entity) where T : AclBase;
        string AddAcl<T>(T entity, ACL acl) where T : AclBase;
        string RemoveAcl<T>(T entity, ACL acl) where T : AclBase;
        ACLUser GetUser();
    }
}
