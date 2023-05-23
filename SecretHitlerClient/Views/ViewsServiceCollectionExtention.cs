
namespace SecretHitler.Views
{
    public static class ViewsServiceCollectionExtention
    {
        public static IServiceCollection AddViews(this IServiceCollection services)
        {
            Routing.RegisterRoute(nameof(StartPage), typeof(StartPage));
            Routing.RegisterRoute(nameof(JoinGamePage), typeof(JoinGamePage));
            Routing.RegisterRoute(nameof(LobbyPage), typeof(LobbyPage));
            Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));

            services.AddTransient<StartPage>();
            services.AddTransient<JoinGamePage>();
            services.AddTransient<LobbyPage>();
            services.AddTransient<AboutPage>();
            services.AddTransient<MainPage>();

            return services;
        }
    }
}
