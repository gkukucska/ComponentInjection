using Microsoft.Extensions.Hosting;
using ComponentGenerator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleApplication
{
    [Application("Components")]
    internal class Program
    {
        static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder();
            builder.Configuration.AddJsonFile("app.json");
            builder.InstallAliases();
            Console.WriteLine("Hello, World!");
            var app = builder.Build();
			
			app.Start();
        }
    }
    
    [HostedService(typeof(MyHostedServiceOptions))]
    internal class MyHostedService : IHostedService
    {
        private readonly SimpleComponent.IMyComponent _myComponent;

        public MyHostedService([Alias] SimpleComponent.IMyComponent myComponent)
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

    internal partial class MyHostedServiceOptions
    {
    }
}