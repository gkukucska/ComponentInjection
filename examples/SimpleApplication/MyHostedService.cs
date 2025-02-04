using ComponentGenerator;
using Microsoft.Extensions.Hosting;
using SimpleComponent.Interfaces;

namespace SimpleApplication
{
    [HostedService(typeof(MyHostedServiceOptions))]
    internal class MyHostedService : IHostedService
    {
        private readonly IMyComponent _myComponent;

        public MyHostedService([Alias] IMyComponent myComponent)
        {
            _myComponent = myComponent;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}