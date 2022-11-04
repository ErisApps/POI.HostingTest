using DSharpPlus;

namespace POI.HostingTest.Services
{
    public interface IDiscordClientProvider
    {
        DiscordClient Client { get; }
    }
}