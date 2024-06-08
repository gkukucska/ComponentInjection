using ComponentGenerator;

namespace SimpleComponent
{
    [Component(typeof(IMyOtherComponent), typeof(MyOtherComponentOptions), Lifetime.Singleton)]
    public class MyOtherComponent : IMyOtherComponent
    { 
    }
}
