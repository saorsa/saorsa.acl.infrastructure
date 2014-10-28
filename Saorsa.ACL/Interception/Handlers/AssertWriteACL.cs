using System.Diagnostics;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Saorsa.ACL.Interception.Handlers
{
    public class AssertWriteACL : ICallHandler
    {
        IMethodReturn ICallHandler.Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            Trace.WriteLine(input.Arguments[0] + ((IAcl)input.Target).UserId);
            IMethodReturn result = getNext()(input, getNext);
            return result;
        }

        int ICallHandler.Order { get; set; }
    }
}