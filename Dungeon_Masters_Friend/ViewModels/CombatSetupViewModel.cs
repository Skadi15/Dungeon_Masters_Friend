using CommunityToolkit.Mvvm.Input;
using DialogHostAvalonia;
using Dungeon_Masters_Friend.Models;
using Dungeon_Masters_Friend.Utilities;
using DynamicData;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Dungeon_Masters_Friend.ViewModels
{
    /// <summary>
    /// View model for the combat setup dialog.
    /// </summary>
    public partial class CombatSetupViewModel : ViewModelBase
    {
        private readonly ICombatantViewModelFactory _combatantViewModelFactory;
        private readonly IDraftCreatureViewModelFactory _draftCreatureViewModelFactory;
        private readonly IDiceRoller _diceRoller;

        /// <summary>
        /// The collection of unfinalized combatants.
        /// </summary>
        public ObservableCollection<CombatantViewModel> DraftCombatants { get; } = [];

        private bool _isInInitiativeMode = false;
        /// <summary>
        /// Indicates if the dialog should show the combatant drafting or initiative view.
        /// </summary>
        public bool IsInInitiativeMode
        {
            get => _isInInitiativeMode;
            set => this.RaiseAndSetIfChanged(ref _isInInitiativeMode, value);
        }

        /// <summary>
        /// The interaction used to add a combatant to or edit a combatant in the list of unfinalized combatants.
        /// </summary>
        public Interaction<DraftCreatureViewModel, Creature?> AddCombatant { get; } = new();
        /// <summary>
        /// The command to show the combatant configuration dialog for a new combatantVm.
        /// </summary>
        public ReactiveCommand<Unit, Unit> AddCombatantCommand { get; }
        /// <summary>
        /// The command to show the combatant configuration dialog for an existing combatant.
        /// </summary>
        public ReactiveCommand<CombatantViewModel, Unit> EditCombatantCommand { get; }

        /// <summary>
        /// A value indicating whether a combat should be started with the drafted combatants.
        /// </summary>
        public bool IsSubmitted { get; private set; } = false;
        /// <summary>
        /// The command that submits the drafted combatants.
        /// </summary>
        public ReactiveCommand<Unit, Unit> SubmitCommand { get; }

        public CombatSetupViewModel(
            ICombatantViewModelFactory combatantViewModelFactory,
            IDraftCreatureViewModelFactory draftCreatureViewModelFactory,
            IDiceRoller diceRoller
        )
        {
            _combatantViewModelFactory = combatantViewModelFactory;
            _draftCreatureViewModelFactory = draftCreatureViewModelFactory;
            _diceRoller = diceRoller;

            AddCombatantCommand = ReactiveCommand.CreateFromTask(AddCombatantAsync);
            EditCombatantCommand = ReactiveCommand.CreateFromTask<CombatantViewModel, Unit>(EditCombatantAsync);
            SubmitCommand = ReactiveCommand.Create(Submit);
        }

        private async Task AddCombatantAsync()
        {
            var result = await AddCombatant.Handle(_draftCreatureViewModelFactory.Create()).FirstAsync();

            if (result != null)
            {
                DraftCombatants.Add(_combatantViewModelFactory.Create(result));
            }
        }

        private async Task<Unit> EditCombatantAsync(CombatantViewModel combatantVm)
        {
            var result = await AddCombatant.Handle(_draftCreatureViewModelFactory.Create(combatantVm.Creature));

            if (result != null)
            {
                DraftCombatants.Replace(combatantVm, _combatantViewModelFactory.Create(result));
            }

            // Return Unit.Default to satisfy Command typing requirements.
            return Unit.Default;
        }

        /// <summary>
        /// Adds a duplicate of the specified combatant to the draft combatants.
        /// </summary>
        /// <remarks>The duplicate has its own underlying model instance, so it can be edited independently.</remarks>
        /// <param name="combatantVm">The combatant to create a duplicate of</param>
        [RelayCommand]
        public void DuplicateCombatant(CombatantViewModel combatantVm) => DraftCombatants.Add(_combatantViewModelFactory.Create(combatantVm.Creature));

        /// <summary>
        /// Removes the specified combatant from the draft combatants.
        /// </summary>
        /// <param name="combatantVm">The combatant to remove</param>
        [RelayCommand]
        public void RemoveCombatant(CombatantViewModel combatantVm) => DraftCombatants.Remove(combatantVm);

        /// <summary>
        /// Removes all draft combatants.
        /// </summary>
        [RelayCommand]
        public void ClearCombatants() => DraftCombatants.Clear();

        /// <summary>
        /// Transitions the dialog to the initiative stage and rolls initiative for non-player combatants.
        /// </summary>
        [RelayCommand]
        public void ProceedToInitiative()
        {
            foreach (var combatantVm in DraftCombatants.Where(combatantVm => !combatantVm.IsPlayer))
            {
                combatantVm.Initiative = _diceRoller.RollDice(1, 20, combatantVm.Creature.InitiativeModifier);
            }

            IsInInitiativeMode = true;
        }

        /// <summary>
        /// Transitions the dialog to the combatant drafting stage.
        /// </summary>
        [RelayCommand]
        public void BackToCombatantDrafting() => IsInInitiativeMode = false;

        /// <summary>
        /// Finalizes the list of combatants by ordering them in descending order according to their initiatives.
        /// </summary>
        /// <returns>The finalized combatants in the correct initiative order</returns>
        public List<CombatantViewModel> FinalizeCombatants() => [.. DraftCombatants.OrderByDescending(combatantVm => combatantVm.Initiative)];

        private void Submit()
        {
            IsSubmitted = true;
            DialogHost.Close(null);
        }
    }

    /// <summary>
    /// Factory for creating CombatSetupViewModel instances with dependency injection.
    /// </summary>
    public interface ICombatSetupViewModelFactory
    {
        /// <summary>
        /// Creates an injected CombatSetupViewModel instance.
        /// </summary>
        /// <returns>An injected CombatSetupViewModel instance</returns>
        CombatSetupViewModel Create();
    }

    /// <inheritdoc cref="ICombatSetupViewModelFactory"/>
    /// <summary>
    /// Basic constructor
    /// </summary>
    /// <param name="serviceProvider">Service provider that provides CombatantViewModel instances</param>
    public class CombatSetupViewModelFactory(IServiceProvider serviceProvider) : ICombatSetupViewModelFactory
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public CombatSetupViewModel Create()
        {
            return _serviceProvider.GetRequiredService<CombatSetupViewModel>();
        }
    }
}
