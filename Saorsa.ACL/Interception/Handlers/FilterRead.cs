using Microsoft.Practices.Unity.InterceptionExtension;

namespace Saorsa.ACL.Interception.Handlers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Saorsa.ACL.Interception.EF.Model;
    using Saorsa.ACL.Interception.Helpers;

    public class FilterRead : ICallHandler
    {
        private Type _type;

        public FilterRead(Type type)
        {
            _type = type;
        }

        IMethodReturn ICallHandler.Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            IMethodReturn result = getNext()(input, getNext);
            var service = (IAcl)input.Target;
            var user = service.GetUser();

            if (result.ReturnValue is AclBase)
            {
                var aclBase = (AclBase)result.ReturnValue;
                if (!AssertAclRightsHelper.CanRead(aclBase.Acl, user.Id, user.Groups))
                    result.ReturnValue = null;
            }
            if (result.ReturnValue is IEnumerable<AclBase>)
            {
                var returnResult = Activator.CreateInstance(typeof (List<>).MakeGenericType(_type));
                var aclList = (result.ReturnValue as IEnumerable<object>);
                var itemsToRemove = aclList.Where(aclBaseObject => !AssertAclRightsHelper.CanRead(((AclBase)aclBaseObject).Acl, user.Id, user.Groups))
                                           .ToList();
                aclList = aclList.Except(itemsToRemove);
                foreach (var obj in aclList)
                {
                    ((IList) returnResult).Add(obj);
                }
                //Cover the IQueryable
                if(result.ReturnValue is IQueryable<AclBase>)
                    return input.CreateMethodReturn(((IList)returnResult).AsQueryable());
                return input.CreateMethodReturn(returnResult);
            }
            return result;

        }

        int ICallHandler.Order { get; set; }
    }
}