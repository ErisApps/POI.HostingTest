namespace POI.HostingTest.Services
{
    public interface IAddDiscordClientFunctionality : IAsyncDisposable, IDisposable
    {
        Task Setup(IDiscordClientProvider clientProvider);
    }
}