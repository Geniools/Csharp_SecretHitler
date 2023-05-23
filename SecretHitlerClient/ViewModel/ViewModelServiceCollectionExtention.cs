
namespace SecretHitler.ViewModel
{
    public static class ViewModelServiceCollectionExtention
    {
        public static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            services.AddTransient<StartPageViewModel>();
            services.AddTransient<JoinGamePageViewModel>();
            services.AddTransient<LobbyPageViewModel>();
            services.AddTransient<AboutPageViewModel>();
            services.AddTransient<MainPageViewModel>();

            return services;
        }
    }
}
