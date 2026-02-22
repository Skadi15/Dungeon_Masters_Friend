using Avalonia.Input;
using Dungeon_Masters_Friend.ViewModels;
using ReactiveUI;
using ReactiveUI.Avalonia;
using System.Reactive.Disposables.Fluent;
using System.Threading.Tasks;

namespace Dungeon_Masters_Friend.Views
{
    public partial class CombatSetupView : ReactiveUserControl<CombatSetupViewModel>
    {
        public CombatSetupView()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                // Bind the AddCombatant interaction to show the combatant drafting dialog
                this.BindInteraction(
                    ViewModel,
                    vm => vm.AddCombatant,
                    ShowDraftCombatantDialogAsync
                ).DisposeWith(disposables);
            });
        }

        private async Task ShowDraftCombatantDialogAsync(IInteractionContext<DraftCombatantViewModel, CombatantViewModel?> context)
        {
            var draftCombatantViewModel = context.Input;
            var draftCombatantView = new DraftCombatantView
            {
                DataContext = draftCombatantViewModel
            };

            await DialogHostAvalonia.DialogHost.Show(draftCombatantView);

            // If the dialog closed and the combatant was not submitted, return null to signal that a combatant should not be added or edited.
            var combatantVm = draftCombatantViewModel.IsSubmitted ? draftCombatantViewModel.ViewModel : null;
            context.SetOutput(combatantVm);
        }

        private void TextBox_GotFocus(object? sender, GotFocusEventArgs e) => CodeBehindUtils.TextBox_SelectContents(sender);
    }
}
