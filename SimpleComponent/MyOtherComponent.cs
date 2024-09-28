using ComponentGenerator;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleComponent
{
    [Component(typeof(MyComponentOptions), ServiceLifetime.Singleton, typeof(IMyComponent), typeof(IMyOtherComponent))]
    public class MyOtherComponent : IMyOtherComponent, IMyComponent
    { 
    }
}
