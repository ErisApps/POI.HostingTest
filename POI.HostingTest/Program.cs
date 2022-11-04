// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using POI.HostingTest.Configuration;
using POI.HostingTest.Services;
using POI.HostingTest.Services.Implementations;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using Constants = POI.HostingTest.Constants;

Console.WriteLine("Hello to POI.HostingTest, World!");

var host = Host.CreateDefaultBuilder()
    .ConfigureAppConfiguration(builder => { builder.AddCommandLine(args); })
    .UseSerilog((_, configuration) =>
    {
        const string logOutputTemplate =
            "[{Timestamp:HH:mm:ss.fff} | {Level:u3} | {SourceContext:l}] {Message:lj}{NewLine}{Exception}";

        configuration
            .MinimumLevel.Verbose()
            .MinimumLevel.Override(nameof(DSharpPlus), LogEventLevel.Information)
            .MinimumLevel.Override(nameof(Microsoft), LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console(theme: SystemConsoleTheme.Colored, outputTemplate: logOutputTemplate);
    })
    .ConfigureServices((context, services) =>
    {
        services.Configure<DiscordConfigurationOptions>(context.Configuration.GetSection(Constants.ConfigurationSections.Discord));

        services
            .AddSingleton<SlashCommandsManagementService>()
            .AddSingleton<IInitializableDiscordClientProvider, DiscordClientProvider>()
            .AddSingleton<IDiscordClientProvider, DiscordClientProvider>(provider =>
                (DiscordClientProvider)provider.GetRequiredService<IDiscordClientProvider>())
            .AddHostedService<DiscordHostedService>();
    });

await host.RunConsoleAsync().ConfigureAwait(false);