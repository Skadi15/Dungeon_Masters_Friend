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
    public partial class CombatantViewModel(Creature creature) : ViewModelBase
    {
        /// <summary>
        /// The underlying combatant model.
        /// </summary>
        public Creature Creature { get; } = creature;

        private int _currentHp = creature.MaxHp;
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

        private bool _isCurrentTurn = false;
        /// <summary>
        /// True if it is currently this combatant's turn in the combat queue
        /// </summary>
        public bool IsCurrentTurn
        {
            get => _isCurrentTurn;
            set => this.RaiseAndSetIfChanged(ref _isCurrentTurn, value);
        }

        private int? _hpAmount;
        /// <summary>
        /// Amount of hit points to heal or damage when the Heal or Damage command is executed, respectively.
        /// </summary>
        public int? HPAmount
        {
            get => _hpAmount;
            set => this.RaiseAndSetIfChanged(ref _hpAmount, value);
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
        [RelayCommand]
        public void Heal()
        {
            if (HPAmount != null)
            {
                CurrentHp = Math.Min(Creature.MaxHp, CurrentHp + HPAmount.Value);
                HPAmount = null;
            }
        }

        /// <summary>
        /// Reduces the combatant's current hit points by the given amount, down to a minimum of zero.
        /// </summary>
        [RelayCommand]
        public void Damage()
        {
            if (HPAmount != null)
            {
                CurrentHp = Math.Max(0, CurrentHp - HPAmount.Value);
                HPAmount = null;
            }
        }

        /// <summary>
        /// Adds the given status to the combatant's list of active statuses.
        /// </summary>
        /// <param name="status">The status to apply</param>
        [RelayCommand]
        public void AddStatus(string status) => Statuses.Add(status);
    }

    /// <summary>
    /// Factory for creating CombatantViewModel instances with dependency injection.
    /// </summary>
    public interface ICombatantViewModelFactory
    {
        /// <summary>
        /// Creates an injected CombatantViewModel instance.
        /// </summary>
        /// <param name="combatant">The data model to pass to the CombatViewModel</param>
        /// <returns>An injected CombatantViewModel instance</returns>
        CombatantViewModel Create(Creature creature);
    }

    /// <inheritdoc cref="ICombatantViewModelFactory"/>
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

        public CombatantViewModel Create(Creature creature)
        {
            return ActivatorUtilities.CreateInstance<CombatantViewModel>(_serviceProvider, creature);
        }
    }
}
