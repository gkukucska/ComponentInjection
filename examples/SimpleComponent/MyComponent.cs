using ComponentGenerator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleComponent.Interfaces;

namespace SimpleComponent
{
    [Component(typeof(MyComponentOptions), ServiceLifetime.Singleton, typeof(IMyComponent))]
    public class MyComponent : IMyComponent
    {
        private readonly MyComponentOptions _options;
        private readonly IMyOtherComponent _otherComponent;
        private readonly ILogger<MyComponent>? _optionalLogger;

        public MyComponent(MyComponentOptions options, [Alias] IMyOtherComponent otherComponent, [Optional]ILogger<MyComponent>
            optionalLogger)
        {
            _options = options;
            _otherComponent = otherComponent;
            _optionalLogger = optionalLogger;
        }
    }
}