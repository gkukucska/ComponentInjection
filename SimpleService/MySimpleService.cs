using ComponentGenerator;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleService
{
    [Service(ServiceLifetime.Singleton, typeof(IMyService))]
    public class MySimpleService : IMyService
    {

    }

    [KeyedService(nameof(MySimpleKeyedService),ServiceLifetime.Singleton,typeof(IMyService))]
    public class MySimpleKeyedService : IMyService
    {

    }
}
