using Dungeon_Masters_Friend.Utilities;
using Dungeon_Masters_Friend.ViewModels;
using Moq;

namespace Dungeon_Masters_Friend.Test.ViewModels
{
    public class MainViewModelTest
    {
        private readonly CombatViewModel _combatViewModel = new(new Mock<ICombatSetupViewModelFactory>().Object);
        private readonly TreasureGeneratorViewModel _treasureGeneratorViewModel = new(new Mock<IDiceRoller>().Object);

        private readonly MainViewModel _mainWindowVm;

        public MainViewModelTest()
        {
            _mainWindowVm = new(
                _combatViewModel,
                _treasureGeneratorViewModel
            );
        }

        [Fact]
        public void NavigateToCombat()
        {
            _mainWindowVm.NavigateToCombat();

            Assert.Equal(_combatViewModel, _mainWindowVm.CurrentPage);
        }

        [Fact]
        public void NavigateToTreasureGenerator()
        {
            _mainWindowVm.NavigateToTreasureGenerator();

            Assert.Equal(_treasureGeneratorViewModel, _mainWindowVm.CurrentPage);
        }

        [Fact]
        public void TogglePane()
        {
            _mainWindowVm.IsPaneOpen = false;
            _mainWindowVm.TogglePane();
            Assert.True(_mainWindowVm.IsPaneOpen);

            _mainWindowVm.TogglePane();
            Assert.False(_mainWindowVm.IsPaneOpen);
        }
    }
}
