namespace Saorsa.ACL.Interception
{
    using Saorsa.ACL.Interception.EF.Model;
    using Saorsa.ACL.Model;

    public interface IAcl
    {
        string UserId { get; set; }
        ACLUser GetUser();
    }
}
