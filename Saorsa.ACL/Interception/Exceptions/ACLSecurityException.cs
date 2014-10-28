namespace Saorsa.ACL.Interception.Exceptions
{
    using System.Security;
    using Saorsa.ACL.Model;

    public class AclSecurityException : SecurityException
    {
        public AclSecurityException(string message, ACLAction action) : base(message)
        {
            AclAction = action;
        }
        public ACLAction AclAction { get; set; }
    }
}
