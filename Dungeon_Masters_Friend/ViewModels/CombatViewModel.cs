using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Dungeon_Masters_Friend.ViewModels
{
    /// <summary>
    /// View model for the combat panel.
    /// </summary>
    public partial class CombatViewModel : ViewModelBase
    {
        private readonly ICombatSetupViewModelFactory _combatSetupViewModelFactory;

        /// <summary>
        /// The collection of combatants participating in the current combat.
        /// </summary>
        public ObservableCollection<CombatantViewModel> Combatants { get; } = [];
        /// <summary>
        /// The interaction used to request the user to configure a combat.
        /// </summary>
        public Interaction<CombatSetupViewModel, List<CombatantViewModel>?> SetupCombat { get; } = new();
        /// <summary>
        /// The command to initiate combat configuration.
        /// </summary>
        public ReactiveCommand<Unit, Unit> SetupCombatCommand { get; }

        private int _currentTurnIndex = 0;
        /// <summary>
        /// Gets the zero-based index of the entity whose turn it currently is.
        /// </summary>
        public int CurrentTurnIndex
        {
            get => _currentTurnIndex;
            private set => this.RaiseAndSetIfChanged(ref _currentTurnIndex, value);
        }

        /// <summary>
        /// Accepts factories for creating view models.
        /// </summary>
        /// <param name="combatSetupViewModelFactory">Factory for creating CombatSetupViewModel instances</param>
        public CombatViewModel(
            ICombatSetupViewModelFactory combatSetupViewModelFactory
        )
        {
            _combatSetupViewModelFactory = combatSetupViewModelFactory;

            SetupCombatCommand = ReactiveCommand.CreateFromTask(SetupCombatAsync);
        }

        private async Task SetupCombatAsync()
        {
            var result = await SetupCombat.Handle(_combatSetupViewModelFactory.Create()).FirstAsync();

            if (result != null)
            {
                // Remove any combatants from previous combats.
                Combatants.Clear();

                foreach (var combatantVm in result)
                {
                    Combatants.Add(combatantVm);
                }

                // Reset the active combatant to the first one.
                CurrentTurnIndex = 0;
            }
        }

        /// <summary>
        /// Advances to the turn of the next combatant in the initiative order.
        /// </summary>
        [RelayCommand]
        public void NextTurn()
        {
            if (Combatants.Count != 0)
            {
                CurrentTurnIndex = (CurrentTurnIndex + 1) % Combatants.Count;
            }
        }
    }
}
