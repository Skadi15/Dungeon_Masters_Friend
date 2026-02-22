using DialogHostAvalonia;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System;
using System.Reactive;

namespace Dungeon_Masters_Friend.ViewModels
{
    /// <summary>
    /// View model for drafting or editing a combatant.
    /// </summary>
    public partial class DraftCombatantViewModel : ViewModelBase
    {
        /// <summary>
        /// The underlying combatant model.
        /// </summary>
        public CombatantViewModel ViewModel { get; } = new(new());
        /// <summary>
        /// A value indicating whether the item has been submitted to be added.
        /// </summary>
        public bool IsSubmitted { get; private set; } = false;
        /// <summary>
        /// The command that submits the drafted combatant.
        /// </summary>
        public ReactiveCommand<Unit, Unit> SubmitCommand { get; }
        /// <summary>
        /// The command that cancels the drafting process.
        /// </summary>
        public ReactiveCommand<Unit, Unit> CancelCommand { get; }

        /// <summary>
        /// Constructor for editing an existing combatant.
        /// </summary>
        /// <param name="combatant">The combatant to edit.</param>
        public DraftCombatantViewModel(CombatantViewModel combatant)
        {
            ViewModel = combatant;
            SubmitCommand = ReactiveCommand.Create(Submit);
            CancelCommand = ReactiveCommand.Create(Cancel);
        }

        private void Submit()
        {
            IsSubmitted = true;
            DialogHost.Close(null);
        }

        private void Cancel()
        {
            DialogHost.Close(null);
        }
    }

    /// <summary>
    /// Factory for creating DraftCombatantViewModel instances with dependency injection.
    /// </summary>
    public interface IDraftCombatantViewModelFactory
    {
        /// <summary>
        /// Creates an injected DraftCombatantViewModel instance.
        /// </summary>
        /// <returns>An injected DraftCombatantViewModel instance</returns>
        public DraftCombatantViewModel Create();

        /// <summary>
        /// Creates an injected DraftCombatantViewModel instance from an existing combatant.
        /// </summary>
        /// <param name="combatantVm">The data model to pass to the DraftCombatantViewModel</param>
        /// <returns>An injected DraftCombatantViewModel instance</returns>
        public DraftCombatantViewModel Create(CombatantViewModel combatantVm);
    }

    /// <inheritdoc/>
    public class DraftCombatantViewModelFactory : IDraftCombatantViewModelFactory
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="serviceProvider">Service provider that provides DraftCombatantViewModel instances</param>
        public DraftCombatantViewModelFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public DraftCombatantViewModel Create()
        {
            return _serviceProvider.GetRequiredService<DraftCombatantViewModel>();
        }

        public DraftCombatantViewModel Create(CombatantViewModel combatantVm)
        {
            return ActivatorUtilities.CreateInstance<DraftCombatantViewModel>(_serviceProvider, combatantVm);
        }
    }
}
