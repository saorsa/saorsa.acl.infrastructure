using System.Collections.Generic;
using System.Linq;

namespace Saorsa.ACL.Tests.Interception.Handlers
{
    using Saorsa.ACL.Interception;
    using Saorsa.ACL.Interception.EF.Model;

    public class MyDomainObject : AclBase
    {
        public string Name { get; set; }
    }
    public interface IMyService : IAcl
    {
        MyDomainObject GetAllowed();
        MyDomainObject GetNotAllowed();
        IQueryable<MyDomainObject> GetAllAsIQueryable();
        IEnumerable<MyDomainObject> GetAllAsIEnumerable();
        IEnumerable<MyDomainObject> GetAllAsIEnumerable_FiveHundredThousandRecords();
        void UpdateNoArguments(MyDomainObject obj);
        void UpdateArgumentsList(MyDomainObject obj, MyDomainObject controlled);
        void UpdateArgumentsListWrongArgumentName(MyDomainObject obj, MyDomainObject controlled);
        void UpdateArgumentsListWrongArgumentType(object obj, object controlled);
    }

}
