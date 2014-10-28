namespace Saorsa.ACL.Interception
{
    using Saorsa.ACL.Interception.EF.Model;

    public interface IAcl
    {
        string UserId { get; set; }
        //Should be implemented for inserts and updates
        //http://stackoverflow.com/questions/9054609/how-to-select-a-single-column-with-entity-framework
        string GetEntityAcl<T>(T entity);
        ACLUser GetUser();
    }
}
