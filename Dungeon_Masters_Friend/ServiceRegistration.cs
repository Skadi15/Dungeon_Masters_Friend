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
            services.AddSingleton<IDiceRoller, DiceRoller>();

            // View Models
            services.AddSingleton<CombatViewModel>();
            services.AddTransient<CombatantViewModel>();
            services.AddTransient<CombatSetupViewModel>();
            services.AddTransient<DraftCombatantViewModel>();
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<TreasureGeneratorViewModel>();

            // View Model Factories
            services.AddSingleton<ICombatantViewModelFactory, CombatantViewModelFactory>();
            services.AddSingleton<ICombatSetupViewModelFactory, CombatSetupViewModelFactory>();
            services.AddSingleton<IDraftCombatantViewModelFactory, DraftCombatantViewModelFactory>();
        }
    }
}
