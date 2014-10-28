namespace Saorsa.ACL.Interception.Handlers.Attributes
{
    using System;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.InterceptionExtension;

    public class FilterReadAttribute : HandlerAttribute
    {
        private Type _type;

        public FilterReadAttribute()
        {
            
        }
        public FilterReadAttribute(Type type)
        {
            _type = type;
        }
        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            return new FilterRead(_type);
        }
    }
}
