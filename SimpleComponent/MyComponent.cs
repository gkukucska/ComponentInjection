﻿using ComponentGenerator;
using Microsoft.Extensions.Logging;

namespace SimpleComponent
{
    [Component(typeof(IMyComponent),typeof(MyComponentOptions),Lifetime.Singleton)]
    public class MyComponent : IMyComponent
    {
        private readonly MyComponentOptions _options;
        private readonly IMyOtherComponent _otherComponent;
        private readonly ILogger<MyComponent> _logger;

        public MyComponent(MyComponentOptions options,[Alias]IMyOtherComponent otherComponent, ILogger<MyComponent> logger)
        {
            _options = options;
            _otherComponent = otherComponent;
            _logger = logger;
        }
    }
}