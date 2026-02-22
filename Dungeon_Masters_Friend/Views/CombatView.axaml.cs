using Avalonia.Controls;
using DialogHostAvalonia;
using Dungeon_Masters_Friend.ViewModels;
using ReactiveUI;
using ReactiveUI.Avalonia;
using System.Collections.Generic;
using System.Reactive.Disposables.Fluent;
using System.Threading.Tasks;

namespace Dungeon_Masters_Friend.Views
{
    public partial class CombatView : ReactiveUserControl<CombatViewModel>
    {
        public CombatView()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                // Bind the SetupCombat interaction to show the combat setup dialog
                this.BindInteraction(
                    ViewModel,
                    vm => vm.SetupCombat,
                    DoShowSetupDialogAsync
                ).DisposeWith(disposables);
            });
        }

        private async Task DoShowSetupDialogAsync(IInteractionContext<CombatSetupViewModel, List<CombatantViewModel>?> context)
        {
            var combatSetupViewModel = context.Input;
            var combatSetupView = new CombatSetupView
            {
                DataContext = combatSetupViewModel
            };

            await DialogHost.Show(combatSetupView);

            // If the setup was not submitted, e.g. the cancel button was pressed,
            // return null to signal that a combat should not be started.
            var combatantVms = combatSetupViewModel.IsSubmitted ? combatSetupViewModel.FinalizeCombatants() : null;
            context.SetOutput(combatantVms);
        }

        private void DataGrid_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (sender is not DataGrid dataGrid)
            {
                return;
            }

            dataGrid.ScrollIntoView(dataGrid.SelectedItem, null);
        }
    }
}
