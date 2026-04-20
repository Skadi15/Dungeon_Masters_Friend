using Avalonia.Input;
using Dungeon_Masters_Friend.Models;
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

        private async Task ShowDraftCombatantDialogAsync(IInteractionContext<DraftCreatureViewModel, Creature?> context)
        {
            var draftCreatureViewModel = context.Input;
            var draftCombatantView = new DraftCreatureView
            {
                DataContext = draftCreatureViewModel
            };

            await DialogHostAvalonia.DialogHost.Show(draftCombatantView);

            // If the dialog closed and the combatant was not submitted, return null to signal that a combatant should not be added or edited.
            var combatantVm = draftCreatureViewModel.IsSubmitted ? draftCreatureViewModel.Creature : null;
            context.SetOutput(combatantVm);
        }

        private void TextBox_GotFocus(object? sender, GotFocusEventArgs e) => CodeBehindUtils.TextBox_SelectContents(sender);
    }
}
