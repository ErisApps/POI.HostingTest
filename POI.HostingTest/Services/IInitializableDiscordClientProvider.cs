namespace POI.HostingTest.Services
{
    public interface IInitializableDiscordClientProvider : IDiscordClientProvider
    {
        Task Initialize();
    }
}