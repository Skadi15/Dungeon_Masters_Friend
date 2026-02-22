using Dungeon_Masters_Friend.ViewModels;
using Moq;

namespace Dungeon_Masters_Friend_Test.ViewModels
{
    public class MainWindowViewModelTest
    {
        private readonly Mock<ICombatSetupViewModelFactory> _combatSetupVmFactory = new();

        private readonly MainWindowViewModel _mainWindowVm;

        public MainWindowViewModelTest()
        {
            _mainWindowVm = new(new(_combatSetupVmFactory.Object));
        }

        [Fact]
        public void NavigateToCombat()
        {
            _mainWindowVm.NavigateToCombat();

            Assert.Equal(_mainWindowVm._combatVm, _mainWindowVm.CurrentPage);
        }
    }
}
