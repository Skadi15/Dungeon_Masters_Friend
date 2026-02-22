using Dungeon_Masters_Friend.Utilities;
using Dungeon_Masters_Friend.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Dungeon_Masters_Friend
{
    /// <summary>
    /// Utility class for registering services for use in dependency injection.
    /// </summary>
    public static class ServiceRegistration
    {
        /// <summary>
        /// Configure this service collection with the services needed for this application.
        /// </summary>
        /// <param name="services"></param>
        public static void Configure(this IServiceCollection services)
        {
            // Utilities
            services.AddSingleton<IDiceRoller>();

            // View Models
            services.AddSingleton<CombatViewModel>();
            services.AddTransient<CombatantViewModel>();
            services.AddTransient<CombatSetupViewModel>();
            services.AddTransient<DraftCombatantViewModel>();
            services.AddSingleton<MainWindowViewModel>();

            // View Model Factories
            services.AddSingleton<ICombatantViewModelFactory>();
            services.AddSingleton<CombatSetupViewModelFactory>();
            services.AddSingleton<IDraftCombatantViewModelFactory, DraftCombatantViewModelFactory>();
        }
    }
}
