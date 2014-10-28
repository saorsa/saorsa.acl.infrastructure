namespace Saorsa.ACL.Interception.Handlers.Attributes
{
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.InterceptionExtension;

    public class FilterUpdateAttribute : HandlerAttribute
    {
        public string[] ControlArgumentNames { get; set; }

        public FilterUpdateAttribute(string[] controlArgumentNames)
        {
            ControlArgumentNames = controlArgumentNames;
        }

        public FilterUpdateAttribute()
        {
            
        }
        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            return new FilterUpdate(ControlArgumentNames);
        }
    }
}
