using DSharpPlus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using POI.HostingTest.Configuration;

namespace POI.HostingTest.Services.Implementations
{
    internal class DiscordClientProvider : IInitializableDiscordClientProvider
    {
        private readonly IOptions<DiscordConfigurationOptions> _options;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<DiscordClientProvider> _logger;

        public DiscordClientProvider(
            IOptions<DiscordConfigurationOptions> options,
            ILoggerFactory loggerFactory,
            ILogger<DiscordClientProvider> logger)
        {
            _options = options;
            _loggerFactory = loggerFactory;
            _logger = logger;
        }

        public DiscordClient Client { get; private set; }

        public Task Initialize()
        {
            _logger.LogInformation("Initializing Discord client");
            Client = new DiscordClient(new DiscordConfiguration
            {
                Intents = DiscordIntents.DirectMessages | DiscordIntents.Guilds | DiscordIntents.GuildMessages |
                          DiscordIntents.GuildVoiceStates | DiscordIntents.MessageContents,
                LoggerFactory = _loggerFactory,
                Token = _options.Value.Token,
                TokenType = TokenType.Bot,
                // This is apparently a bad idea according to the documentation... but I'm going to enable it regardless...
                ReconnectIndefinitely = true
            });

            return Task.CompletedTask;
        }
    }
}