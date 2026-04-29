using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Media;
using DialogHostAvalonia;
using Dungeon_Masters_Friend.ViewModels;
using ReactiveUI;
using ReactiveUI.Avalonia;
using System.Collections.Generic;
using System.ComponentModel;
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

                ViewModel?.PropertyChanged += OnViewModelPropertyChanged;
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

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            // When the current turn index changes, scroll the combatants list to bring the active combatant into view.
            if (ViewModel != null && e.PropertyName == nameof(ViewModel.CurrentTurnIndex))
            {
                CombatantsList.ContainerFromIndex(ViewModel.CurrentTurnIndex)?.BringIntoView();
            }
        }
    }

    internal class IsCurrentTurnConverter : IValueConverter
    {
        public object Convert(object? value, System.Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool isCurrentTurn)
            {
                return isCurrentTurn ? Brushes.DimGray : Brushes.Transparent;
            }
            return false;
        }

        public object ConvertBack(object? value, System.Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}
