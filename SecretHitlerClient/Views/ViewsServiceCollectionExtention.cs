
namespace SecretHitler.Views
{
    public static class ViewsServiceCollectionExtention
    {
        public static IServiceCollection AddViews(this IServiceCollection services)
        {
            services.AddTransient<StartPage>();
            services.AddTransient<JoinGamePage>();
            services.AddTransient<LobbyPage>();
            services.AddTransient<MainPage>();
            services.AddTransient<AboutPage>();

            return services;
        }
    }
}
