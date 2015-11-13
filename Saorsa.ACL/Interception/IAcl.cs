namespace Saorsa.ACL.Interception
{
    using EF.Model;

    public interface IAcl
    {
        string UserId { get; set; }
        ACLUser GetUser();
    }
}
