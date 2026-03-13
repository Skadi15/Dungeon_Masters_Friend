using CommunityToolkit.Mvvm.Input;
using ReactiveUI;

namespace Dungeon_Masters_Friend.ViewModels
{
    /// <summary>
    /// Main view model for the application. Responsible for managing navigation.
    /// </summary>
    public partial class MainViewModel : ViewModelBase
    {
        private ReactiveObject? _currentPage;
        /// <summary>
        /// The currently displayed page.
        /// </summary>
        public ReactiveObject? CurrentPage
        {
            get => _currentPage;
            set => this.RaiseAndSetIfChanged(ref _currentPage, value);
        }

        private bool _isWide;
        /// <summary>
        /// Whether the window is currently wide enough to show the navigation pane permanently.
        /// </summary>
        public bool IsWide
        {
            get => _isWide;
            set => this.RaiseAndSetIfChanged(ref _isWide, value);
        }

        private bool _isPaneOpen;
        /// <summary>
        /// Whether the navigation pane is currently open. Only relevant when IsWide is false.
        /// </summary>
        public bool IsPaneOpen
        {
            get => _isPaneOpen;
            set => this.RaiseAndSetIfChanged(ref _isPaneOpen, value);

        }

        // References to different tool ViewModels
        private readonly CombatViewModel _combatVm;
        private readonly TreasureGeneratorViewModel _treasureGeneratorVm;

        /// <summary>
        /// Accepts instances of the different tool view models.
        /// </summary>
        /// <param name="combatVm"></param>
        /// <param name="treasureGeneratorVm"></param>
        public MainViewModel(
            CombatViewModel combatVm,
            TreasureGeneratorViewModel treasureGeneratorVm
        )
        {
            _combatVm = combatVm;
            _treasureGeneratorVm = treasureGeneratorVm;

            _currentPage = _combatVm;
        }

        /// <summary>
        /// Navigates to the combat page. If the window is not wide, also closes the navigation pane.
        /// </summary>
        [RelayCommand]
        public void NavigateToCombat()
        {
            CurrentPage = _combatVm;
            if (!IsWide)
            {
                TogglePane();
            }
        }

        /// <summary>
        /// Navigates to the treasure generator page. If the window is not wide, also closes the navigation pane.
        /// </summary>
        [RelayCommand]
        public void NavigateToTreasureGenerator()
        {
            CurrentPage = _treasureGeneratorVm;
            if (!IsWide)
            {
                TogglePane();
            }
        }

        /// <summary>
        /// Toggles the open/close state of the navigation pane.
        /// </summary>
        [RelayCommand]
        public void TogglePane() => IsPaneOpen = !IsPaneOpen;
    }
}
