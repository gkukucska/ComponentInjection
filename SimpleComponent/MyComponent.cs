using ComponentGenerator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SimpleComponent
{
    [Component(typeof(MyComponentOptions), ServiceLifetime.Singleton, typeof(IMyComponent))]
    public class MyComponent : IMyComponent
    {
        private readonly MyComponentOptions _options;
        private readonly IMyOtherComponent? _otherComponent;
        private readonly IMyOtherComponent? _optionalOtherComponent;
        private readonly ILogger<MyComponent> _logger;
        private readonly ILogger<MyComponent> _optionalLogger;

        public MyComponent(MyComponentOptions options, [Alias] IMyOtherComponent otherComponent, [Alias] IMyOtherComponent? optionalOtherComponent, 
            ILogger<MyComponent> logger, [Optional]ILogger<MyComponent> optionalLogger)
        {
            _options = options;
            _otherComponent = otherComponent;
            _optionalOtherComponent = optionalOtherComponent;
            _logger = logger;
            _optionalLogger = optionalLogger;
        }
    }
}
