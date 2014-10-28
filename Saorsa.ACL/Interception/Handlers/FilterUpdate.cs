using Microsoft.Practices.Unity.InterceptionExtension;

namespace Saorsa.ACL.Interception.Handlers
{
    using System;
    using System.Linq;
    using Saorsa.ACL.Interception.EF.Model;
    using Saorsa.ACL.Interception.Exceptions;
    using Saorsa.ACL.Interception.Helpers;
    using Saorsa.ACL.Model;

    public class FilterUpdate : ICallHandler
    {
        private readonly string[] _argumentNames;

        public FilterUpdate(string[] argumentNames)
        {
            this._argumentNames = argumentNames;
        }

        public FilterUpdate()
        {
            
        }
        IMethodReturn ICallHandler.Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            var service = (IAcl)input.Target;
            var user = service.GetUser();
            //Control a list of arguments
            if (_argumentNames != null && _argumentNames.Length > 0)
            {
                foreach (var parameter in this._argumentNames.Where(parameter => !input.Arguments.ContainsParameter(parameter)))
                {
                    throw new ArgumentException(string.Format("You cannot filter ACLS based on the argument {0}, because it doesn't exist. The parameterName is controlled in the service definition, please check it.", parameter));
                }
                foreach (var parameter in _argumentNames)
                {
                    var obj = input.Arguments[parameter] as AclBase;
                    if(obj == null)
                        throw new ArgumentException(string.Format("The parameter {0} cannot be controlled, because it is not of the type AclBase.",parameter),parameter);
                    if(!AssertAclRightsHelper.CanWrite(obj.Acl, user.Id, user.Groups))
                    {
                        throw new AclSecurityException(string.Format("You cannot update an ACL controlled parameter {0}, because you or your groups are not in it's Access Control List", parameter), ACLAction.Write);
                    }
                }
            }
            else
            {
                //Control all arguments
                if (input.Arguments.OfType<AclBase>()
                            .Select(argument => argument)
                            .Any(aclObject => !AssertAclRightsHelper.CanWrite(aclObject.Acl, user.Id, user.Groups)))
                {
                    throw new AclSecurityException("You cannot update on.", ACLAction.Write);
                }
            }
            IMethodReturn result = getNext()(input, getNext);
            return result;

        }

        int ICallHandler.Order { get; set; }
    }
}