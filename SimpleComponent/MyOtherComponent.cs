using ComponentGenerator;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleComponent
{
    [Component(typeof(MyOtherComponentOptions), ServiceLifetime.Singleton, typeof(IMyComponent), typeof(IMyOtherComponent))]
    public class MyOtherComponent : IMyOtherComponent, IMyComponent
    { 
    }
}
