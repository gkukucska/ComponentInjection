using ComponentGenerator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleComponent.Interfaces;

namespace SimpleComponent
{
    [KeylessComponent(typeof(MyKeylessComponentOptions), ServiceLifetime.Singleton, typeof(IMyComponent))]
    public class MyKeylessComponent : IMyComponent
    {
        private readonly MyKeylessComponentOptions _options;
        private readonly IMyOtherComponent _otherComponent;
        private readonly ILogger<MyComponent> _logger;

        public MyKeylessComponent(MyKeylessComponentOptions options, [Alias] IMyOtherComponent otherComponent, ILogger<MyComponent> logger)
        {
            _options = options;
            _otherComponent = otherComponent;
            _logger = logger;
        }
    }
}
