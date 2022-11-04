using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace POI.HostingTest.Services.Implementations
{
    public class DiscordHostedService : IHostedService
    {
        private readonly ILogger<DiscordHostedService> _logger;
        private readonly IInitializableDiscordClientProvider _discordClientProvider;
        private readonly SlashCommandsManagementService _slashCommandsManagementService;

        public DiscordHostedService(
            ILogger<DiscordHostedService> logger,
            IInitializableDiscordClientProvider discordClientProvider,
            SlashCommandsManagementService slashCommandsManagementService)
        {
            _logger = logger;
            _discordClientProvider = discordClientProvider;
            _slashCommandsManagementService = slashCommandsManagementService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _discordClientProvider.Initialize().ConfigureAwait(false);
            await _slashCommandsManagementService.Setup(_discordClientProvider).ConfigureAwait(false);

            _logger.LogInformation("Starting Discord client");
            await _discordClientProvider.Client.ConnectAsync().ConfigureAwait(false);
            _logger.LogInformation("Discord client started");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping Discord client");
            await _discordClientProvider.Client.DisconnectAsync().ConfigureAwait(false);
            _logger.LogInformation("Discord client stopped");
        }
    }
}