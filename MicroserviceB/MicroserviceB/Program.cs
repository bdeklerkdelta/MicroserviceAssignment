using System;
using RabbitMQ.Client;
using System.Text;
using RabbitMQ.Client.Events;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MicroserviceB.Service.Services;
using MicroserviceB.Messaging.Receive.Receiver;
using Microsoft.Extensions.Configuration;
using MicroserviceB.Messaging.Receive.Options;
using MediatR;
using System.Reflection;

namespace MicroserviceB
{
    class Program
    {
        public static IConfigurationSection ConfigSection;
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, configuration) =>
        {
            configuration.Sources.Clear();

            IHostEnvironment env = hostingContext.HostingEnvironment;

            configuration
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfigurationRoot configurationRoot = configuration.Build();

            ConfigSection = configurationRoot.GetSection(nameof(RabbitMqOptions));
            var configValue = ConfigSection.Get<RabbitMqOptions>();

        })
            .ConfigureServices((_, services) =>
                     services.AddHostedService<DisplayNameConsumer>()
                             .AddScoped<IDisplayNameService, DisplayNameService>()
                             .Configure<RabbitMqOptions>(ConfigSection)         
                             .AddMediatR(Assembly.GetExecutingAssembly(), typeof(IDisplayNameService).Assembly)
                             .AddOptions());

    }
}
