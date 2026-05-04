using Dungeon_Masters_Friend.ViewModels;
using Moq;
using System.Collections.ObjectModel;

namespace Dungeon_Masters_Friend.Test.ViewModels
{
    public class CombatViewModelTest
    {
        private readonly Mock<ICombatSetupViewModelFactory> _combatSetupViewModelFactory;

        private readonly CombatViewModel _combatViewModel;

        public CombatViewModelTest()
        {
            _combatSetupViewModelFactory = new();
            _combatSetupViewModelFactory.Setup(factory => factory.Create(It.IsAny<IEnumerable<CombatantViewModel>>()))
               .Returns((IEnumerable<CombatantViewModel> combatants) =>
               {
                   // The actual values of the CombatSetupViewModel are not relevant for these tests, so we can return a dummy instance.
                   return new CombatSetupViewModel(null!, null!, null!, combatants);
               });

            _combatViewModel = new CombatViewModel(_combatSetupViewModelFactory.Object);
        }

        [Fact]
        public void SetupCombat()
        {
            var combatants = new List<CombatantViewModel>
            {
                new(new() { Name = "Combatant 1"}),
                new(new() { Name = "Combatant 2"}),
                new(new() { Name = "Combatant 3"}),
            };
            _combatViewModel.SetupCombat.RegisterHandler(interaction => interaction.SetOutput(combatants));

            _combatViewModel.SetupCombatCommand.Execute();

            Assert.Equal(combatants, _combatViewModel.Combatants);
            Assert.Equal(0, _combatViewModel.CurrentTurnIndex);
            Assert.True(_combatViewModel.Combatants[0].IsCurrentTurn);
            Assert.False(_combatViewModel.Combatants[1].IsCurrentTurn);
            Assert.False(_combatViewModel.Combatants[2].IsCurrentTurn);

            _combatSetupViewModelFactory.Verify(factory => factory.Create(It.Is<IEnumerable<CombatantViewModel>>(list => list.Count() == 0)));
            _combatSetupViewModelFactory.VerifyNoOtherCalls();
        }

        [Fact]
        public void SetupCombat_NullResponse()
        {
            _combatViewModel.SetupCombat.RegisterHandler(interaction => interaction.SetOutput(null));

            var oldCombatants = new List<CombatantViewModel>
            {
                new(new() { Name = "Old Combatant 1"}),
                new(new() { Name = "Old Combatant 2"}) { IsCurrentTurn = true },
            };
            oldCombatants.ForEach(_combatViewModel.Combatants.Add);
            _combatViewModel.NextTurn();

            _combatViewModel.SetupCombatCommand.Execute();

            Assert.Equal(oldCombatants, _combatViewModel.Combatants);
            Assert.Equal(1, _combatViewModel.CurrentTurnIndex);
            Assert.False(_combatViewModel.Combatants[0].IsCurrentTurn);
            Assert.True(_combatViewModel.Combatants[1].IsCurrentTurn);

            _combatSetupViewModelFactory.Verify(factory => factory.Create(It.Is<IEnumerable<CombatantViewModel>>(list => list.Count() == 2)));
            _combatSetupViewModelFactory.VerifyNoOtherCalls();
        }

        [Fact]
        public void NextTurn()
        {
            var combatants = new List<CombatantViewModel>
            {
                new(new() { Name = "Combatant 1"}) { IsCurrentTurn = true },
                new(new() { Name = "Combatant 2"}),
                new(new() { Name = "Combatant 3"}),
            };
            combatants.ForEach(_combatViewModel.Combatants.Add);

            Assert.Equal(0, _combatViewModel.CurrentTurnIndex);

            _combatViewModel.NextTurn();

            Assert.Equal(1, _combatViewModel.CurrentTurnIndex);
            Assert.False(combatants[0].IsCurrentTurn);
            Assert.True(combatants[1].IsCurrentTurn);
            Assert.False(combatants[2].IsCurrentTurn);

            _combatViewModel.NextTurn();

            Assert.Equal(2, _combatViewModel.CurrentTurnIndex);
            Assert.False(combatants[0].IsCurrentTurn);
            Assert.False(combatants[1].IsCurrentTurn);
            Assert.True(combatants[2].IsCurrentTurn);

            _combatViewModel.NextTurn();

            Assert.Equal(0, _combatViewModel.CurrentTurnIndex);
            Assert.True(combatants[0].IsCurrentTurn);
            Assert.False(combatants[1].IsCurrentTurn);
            Assert.False(combatants[2].IsCurrentTurn);

            _combatSetupViewModelFactory.VerifyNoOtherCalls();
        }

        [Fact]
        public void NextTurn_NoCombatants()
        {
            Assert.Equal(0, _combatViewModel.CurrentTurnIndex);
            _combatViewModel.NextTurn();
            Assert.Equal(0, _combatViewModel.CurrentTurnIndex);

            _combatSetupViewModelFactory.VerifyNoOtherCalls();
        }
    }
}
