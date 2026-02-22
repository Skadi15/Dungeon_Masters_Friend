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

        // References to our different tool ViewModels
        public CombatViewModel _combatVm;

        public MainWindowViewModel(CombatViewModel combatVm)
        {
            _combatVm = combatVm;

            _currentPage = _combatVm;
        }

        [RelayCommand]
        public void NavigateToCombat() => CurrentPage = _combatVm;
    }
}
