using Microsoft.Extensions.Hosting;
using ComponentGenerator;
using SimpleComponent;

namespace SimpleApplication
{
    [Application("Components")]
    internal class Program
    {
        static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder();
            builder.InstallAliases();
            Console.WriteLine("Hello, World!");
        }
    }
}
