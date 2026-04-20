using Dungeon_Masters_Friend.Models;
using Dungeon_Masters_Friend.Repositories;
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

            // Repositories
            services.AddTransient<IBestiaryRepository, BestiaryRepository>();

            // View Models
            services.AddSingleton<BestiaryViewModel>();
            services.AddSingleton<CombatViewModel>();
            services.AddTransient<CombatantViewModel>();
            services.AddTransient<CombatSetupViewModel>();
            services.AddTransient<CreatureViewModel>();
            services.AddTransient<DraftCombatantViewModel>();
            services.AddTransient<DraftCreatureViewModel>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<TreasureGeneratorViewModel>();

            // View Model Factories
            services.AddSingleton<ICombatantViewModelFactory, CombatantViewModelFactory>();
            services.AddSingleton<ICombatSetupViewModelFactory, CombatSetupViewModelFactory>();
            services.AddSingleton<IDraftCombatantViewModelFactory, DraftCombatantViewModelFactory>();
            services.AddSingleton<IDraftCreatureViewModelFactory, DraftCreatureViewModelFactory>();
        }
    }
}
