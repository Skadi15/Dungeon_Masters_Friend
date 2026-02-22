using CommunityToolkit.Mvvm.Input;
using Dungeon_Masters_Friend.Models;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;

namespace Dungeon_Masters_Friend.ViewModels
{
    /// <summary>
    /// View model for a single combatant.
    /// </summary>
    public partial class CombatantViewModel(Entity entity) : ViewModelBase
    {
        /// <summary>
        /// The underlying combatant model.
        /// </summary>
        public Entity Entity { get; } = entity;

        private int _currentHp = entity.MaxHp;
        /// <summary>
        /// This combatant's current hit points
        /// </summary>
        public int CurrentHp
        {
            get => _currentHp;
            set => this.RaiseAndSetIfChanged(ref _currentHp, value);
        }

        private int _initiative;
        /// <summary>
        /// This combatant's priority in the combat queue
        /// </summary>
        public int Initiative
        {
            get => _initiative;
            set => this.RaiseAndSetIfChanged(ref _initiative, value);
        }

        private bool _isPlayer;
        /// <summary>
        /// True if the combatant is a player character
        /// </summary>
        public bool IsPlayer
        {
            get => _isPlayer;
            set => this.RaiseAndSetIfChanged(ref _isPlayer, value);
        }

        /// <summary>
        /// The statuses currently applied to this combatant
        /// </summary>
        public ObservableCollection<string> Statuses { get; } = [];

        /// <summary>
        /// Default constructor
        /// </summary>
        public CombatantViewModel() : this(new()) { }

        /// <summary>
        /// Increases the combatant's current hit points by the given amount, up to their maximum hit points.
        /// </summary>
        /// <param name="amount">The amount of hit points to restore</param>
        [RelayCommand]
        public void Heal(int amount) => CurrentHp = Math.Min(Entity.MaxHp, CurrentHp + amount);

        /// <summary>
        /// Reduces the combatant's current hit points by the given amount, down to a minimum of zero.
        /// </summary>
        /// <param name="amount">The amount of hit points to deduct</param>
        [RelayCommand]
        public void Damage(int amount) => CurrentHp = Math.Max(0, CurrentHp - amount);

        /// <summary>
        /// Adds the given status to the combatant's list of active statuses.
        /// </summary>
        /// <param name="status">The status to apply</param>
        [RelayCommand]
        public void AddStatus(string status) => Statuses.Add(status);
    }

    public interface ICombatantViewModelFactory
    {
        CombatantViewModel Create(Entity entity);
    }

    /// <summary>
    /// Factory for creating CombatantViewModel instances with dependency injection.
    /// </summary>
    public class CombatantViewModelFactory : ICombatantViewModelFactory
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="serviceProvider">Service provider that provides CombatantViewModel instances</param>
        public CombatantViewModelFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Creates an injected CombatantViewModel instance.
        /// </summary>
        /// <param name="combatant">The data model to pass to the CombatViewModel</param>
        /// <returns>An injected CombatantViewModel instance</returns>
        public CombatantViewModel Create(Entity entity)
        {
            return ActivatorUtilities.CreateInstance<CombatantViewModel>(_serviceProvider, entity);
        }
    }
}
