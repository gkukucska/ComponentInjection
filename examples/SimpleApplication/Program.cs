using ComponentBuilderExtensions;
using Microsoft.Extensions.Hosting;
using ComponentGenerator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleComponent;
using System.ComponentModel;

namespace SimpleApplication
{
    [Application("Components")]
    internal class Program
    {
        static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder();
            builder.Configuration.AddJsonFile("app.json");
            builder.InstallComponents();
            builder.ValidateAliases();
            Console.WriteLine("Hello, World!");
            var app = builder.Build();

            app.Start();
        }
    }
}