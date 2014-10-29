using System.Collections.Generic;
using System.Linq;

namespace Saorsa.ACL.Tests.Interception.Handlers
{
    using Saorsa.ACL.Interception;
    using Saorsa.ACL.Interception.EF.Model;

    public class AclDomainObject : AclBase
    {
        public string Name { get; set; }
    }
    public interface IMyService : IAcl
    {
        AclDomainObject GetAllowed();
        AclDomainObject GetNotAllowed();
        IQueryable<AclDomainObject> GetAllAsIQueryable();
        IEnumerable<AclDomainObject> GetAllAsIEnumerable();
        IEnumerable<AclDomainObject> GetAllAsIEnumerable_FiveHundredThousandRecords();
        void UpdateNoArguments(AclDomainObject obj);
        void UpdateArgumentsList(AclDomainObject obj, AclDomainObject controlled);
        void UpdateArgumentsListWrongArgumentName(AclDomainObject obj, AclDomainObject controlled);
        void UpdateArgumentsListWrongArgumentType(object obj, object controlled);
        void UpdateEnumerableArgumentsList(IEnumerable<AclDomainObject> domainObjects);
    }

}
