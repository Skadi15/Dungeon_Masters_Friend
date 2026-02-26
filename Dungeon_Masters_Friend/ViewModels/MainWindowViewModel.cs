using CommunityToolkit.Mvvm.Input;
using ReactiveUI;

namespace Dungeon_Masters_Friend.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private ReactiveObject? _currentPage;
        public ReactiveObject? CurrentPage
        {
            get => _currentPage;
            set => this.RaiseAndSetIfChanged(ref _currentPage, value);
        }

        private string _windowTitle = "Dungeon Master's Friend";
        public string WindowTitle
        {
            get => _windowTitle;
            set => this.RaiseAndSetIfChanged(ref _windowTitle, value);
        }

        // References to different tool ViewModels
        public CombatViewModel _combatVm;
        public TreasureGeneratorViewModel _treasureGeneratorVm;

        public MainWindowViewModel(
            CombatViewModel combatVm,
            TreasureGeneratorViewModel treasureGeneratorVm
        )
        {
            _combatVm = combatVm;
            _treasureGeneratorVm = treasureGeneratorVm;

            _currentPage = _combatVm;
        }

        [RelayCommand]
        public void NavigateToCombat() => CurrentPage = _combatVm;

        [RelayCommand]
        public void NavigateToTreasureGenerator() => CurrentPage = _treasureGeneratorVm;
    }
}
