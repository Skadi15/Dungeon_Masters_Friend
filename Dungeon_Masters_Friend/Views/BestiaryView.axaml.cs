using Dungeon_Masters_Friend.ViewModels;
using ReactiveUI;
using ReactiveUI.Avalonia;
using System.Threading.Tasks;

namespace Dungeon_Masters_Friend.Views;

public partial class BestiaryView : ReactiveUserControl<BestiaryViewModel>
{
    public BestiaryView()
    {
        InitializeComponent();

        this.WhenActivated(disposables =>
        {
            // Bind the AddCreature interaction to show the creature drafting dialog
            this.BindInteraction(
                ViewModel,
                vm => vm.AddCreature,
                ShowDraftCreatureDialogAsync
            );
        });
    }

    private async Task ShowDraftCreatureDialogAsync(IInteractionContext<DraftCreatureViewModel, CreatureViewModel?> context)
    {
        var draftCreatureViewModel = context.Input;
        var draftCreatureView = new DraftCreatureView
        {
            DataContext = draftCreatureViewModel
        };
        await DialogHostAvalonia.DialogHost.Show(draftCreatureView);

        // If the dialog closed and the creature was not submitted, return null to signal that a creatureVm should not be added or edited.
        var creatureVm = draftCreatureViewModel.IsSubmitted ? new CreatureViewModel(draftCreatureViewModel.Creature) : null;
        context.SetOutput(creatureVm);
    }
}