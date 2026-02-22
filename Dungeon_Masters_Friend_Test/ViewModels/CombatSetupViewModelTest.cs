using Dungeon_Masters_Friend.Models;
using Dungeon_Masters_Friend.Utilities;
using Dungeon_Masters_Friend.ViewModels;
using Moq;

namespace Dungeon_Masters_Friend_Test.ViewModels
{
    public class CombatSetupViewModelTest
    {
        private readonly Mock<ICombatantViewModelFactory> _combatantViewModelFactoryMock = new();
        private readonly Mock<IDraftCombatantViewModelFactory> _draftCombatantViewModelFactoryMock = new();
        private readonly Mock<IDiceRoller> _diceRollerMock = new();

        private readonly CombatSetupViewModel _combatSetupVm;

        public CombatSetupViewModelTest()
        {
            _combatSetupVm = new(
                _combatantViewModelFactoryMock.Object,
                _draftCombatantViewModelFactoryMock.Object,
                _diceRollerMock.Object
            );
        }


        [Fact]
        public void AddCombatant()
        {
            _draftCombatantViewModelFactoryMock.Setup(factory => factory.Create())
                .Returns(new DraftCombatantViewModel(new()));

            var newCombatantVm = new CombatantViewModel(new() { Name = "Combatant1" });
            _combatSetupVm.AddCombatant.RegisterHandler(interaction => interaction.SetOutput(newCombatantVm));

            _combatSetupVm.AddCombatantCommand.Execute();

            Assert.Single(_combatSetupVm.DraftCombatants);
            Assert.Equal(newCombatantVm, _combatSetupVm.DraftCombatants.First());

            _combatantViewModelFactoryMock.VerifyNoOtherCalls();

            _draftCombatantViewModelFactoryMock.Verify(factory => factory.Create(), Times.Once());
            _draftCombatantViewModelFactoryMock.VerifyNoOtherCalls();

            _diceRollerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void AddCombatant_NullResponse()
        {
            _draftCombatantViewModelFactoryMock.Setup(factory => factory.Create())
                .Returns(new DraftCombatantViewModel(new()));

            _combatSetupVm.AddCombatant.RegisterHandler(interaction => interaction.SetOutput(null));

            _combatSetupVm.AddCombatantCommand.Execute();

            Assert.Empty(_combatSetupVm.DraftCombatants);

            _combatantViewModelFactoryMock.VerifyNoOtherCalls();

            _draftCombatantViewModelFactoryMock.Verify(factory => factory.Create(), Times.Once());
            _draftCombatantViewModelFactoryMock.VerifyNoOtherCalls();

            _diceRollerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void EditCombatant()
        {
            _combatantViewModelFactoryMock.Setup(factory => factory.Create(It.IsAny<Entity>()))
                .Returns((Entity entity) => new CombatantViewModel(entity));
            _draftCombatantViewModelFactoryMock.Setup(factory => factory.Create(It.IsAny<CombatantViewModel>()))
                .Returns((CombatantViewModel combatantVm) => new DraftCombatantViewModel(combatantVm));

            var existingCombatantVm = new CombatantViewModel(new() { Name = "Combatant1" });
            _combatSetupVm.DraftCombatants.Add(existingCombatantVm);

            var existingCombatantVm2 = new CombatantViewModel(new() { Name = "Combatant2" });
            _combatSetupVm.DraftCombatants.Add(existingCombatantVm2);

            var newCombatantVm = new CombatantViewModel(new() { Name = "NewCombatant" });
            _combatSetupVm.AddCombatant.RegisterHandler(interaction => interaction.SetOutput(newCombatantVm));

            _combatSetupVm.EditCombatantCommand.Execute(existingCombatantVm);

            Assert.Equal(2, _combatSetupVm.DraftCombatants.Count);
            Assert.Equal(newCombatantVm.Entity.Name, _combatSetupVm.DraftCombatants.First().Entity.Name);
            Assert.Equal(existingCombatantVm2, _combatSetupVm.DraftCombatants[1]);

            _combatantViewModelFactoryMock.Verify(factory => factory.Create(existingCombatantVm.Entity));
            _combatantViewModelFactoryMock.VerifyNoOtherCalls();

            _draftCombatantViewModelFactoryMock.Verify(factory => factory.Create(It.IsAny<CombatantViewModel>()), Times.Once());
            _draftCombatantViewModelFactoryMock.VerifyNoOtherCalls();

            _diceRollerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void EditCombatant_NullResponse()
        {
            _combatantViewModelFactoryMock.Setup(factory => factory.Create(It.IsAny<Entity>()))
                .Returns((Entity entity) => new CombatantViewModel(entity));
            _draftCombatantViewModelFactoryMock.Setup(factory => factory.Create(It.IsAny<CombatantViewModel>()))
                .Returns((CombatantViewModel combatantVm) => new DraftCombatantViewModel(combatantVm));

            var existingCombatantVm = new CombatantViewModel(new() { Name = "Combatant1" });
            _combatSetupVm.DraftCombatants.Add(existingCombatantVm);

            var existingCombatantVm2 = new CombatantViewModel(new() { Name = "Combatant2" });
            _combatSetupVm.DraftCombatants.Add(existingCombatantVm2);

            _combatSetupVm.AddCombatant.RegisterHandler(interaction => interaction.SetOutput(null));

            _combatSetupVm.EditCombatantCommand.Execute(existingCombatantVm);

            Assert.Equal(2, _combatSetupVm.DraftCombatants.Count);
            Assert.Equal(existingCombatantVm, _combatSetupVm.DraftCombatants[0]);
            Assert.Equal(existingCombatantVm2, _combatSetupVm.DraftCombatants[1]);

            _combatantViewModelFactoryMock.Verify(factory => factory.Create(existingCombatantVm.Entity));
            _combatantViewModelFactoryMock.VerifyNoOtherCalls();

            _draftCombatantViewModelFactoryMock.Verify(factory => factory.Create(It.IsAny<CombatantViewModel>()), Times.Once());
            _draftCombatantViewModelFactoryMock.VerifyNoOtherCalls();

            _diceRollerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void DuplicateCombatant()
        {
            _combatantViewModelFactoryMock.Setup(factory => factory.Create(It.IsAny<Entity>()))
                .Returns((Entity entity) => new CombatantViewModel(entity));

            var existingCombatantVm1 = new CombatantViewModel(new() { Name = "Combatant1" });
            _combatSetupVm.DraftCombatants.Add(existingCombatantVm1);

            var existingCombatantVm2 = new CombatantViewModel(new() { Name = "Combatant2" });
            _combatSetupVm.DraftCombatants.Add(existingCombatantVm2);

            _combatSetupVm.DuplicateCombatant(existingCombatantVm1);

            Assert.Equal(3, _combatSetupVm.DraftCombatants.Count);
            Assert.Equal(existingCombatantVm1, _combatSetupVm.DraftCombatants[0]);
            Assert.Equal(existingCombatantVm2, _combatSetupVm.DraftCombatants[1]);
            Assert.NotSame(existingCombatantVm1, _combatSetupVm.DraftCombatants[2]);
            Assert.Equal(existingCombatantVm1.Entity.Name, _combatSetupVm.DraftCombatants[2].Entity.Name);

            _combatantViewModelFactoryMock.Verify(factory => factory.Create(existingCombatantVm1.Entity));
            _combatantViewModelFactoryMock.VerifyNoOtherCalls();

            _draftCombatantViewModelFactoryMock.VerifyNoOtherCalls();

            _diceRollerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void RemoveCombatant()
        {
            var existingCombatantVm1 = new CombatantViewModel(new() { Name = "Combatant1" });
            _combatSetupVm.DraftCombatants.Add(existingCombatantVm1);

            var existingCombatantVm2 = new CombatantViewModel(new() { Name = "Combatant2" });
            _combatSetupVm.DraftCombatants.Add(existingCombatantVm2);

            _combatSetupVm.RemoveCombatant(existingCombatantVm1);

            Assert.Single(_combatSetupVm.DraftCombatants);
            Assert.Equal(existingCombatantVm2, _combatSetupVm.DraftCombatants.First());

            _combatantViewModelFactoryMock.VerifyNoOtherCalls();

            _draftCombatantViewModelFactoryMock.VerifyNoOtherCalls();

            _diceRollerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ClearCombatants()
        {
            var existingCombatantVm1 = new CombatantViewModel(new() { Name = "Combatant1" });
            _combatSetupVm.DraftCombatants.Add(existingCombatantVm1);

            var existingCombatantVm2 = new CombatantViewModel(new() { Name = "Combatant2" });
            _combatSetupVm.DraftCombatants.Add(existingCombatantVm2);

            _combatSetupVm.ClearCombatants();

            Assert.Empty(_combatSetupVm.DraftCombatants);

            _combatantViewModelFactoryMock.VerifyNoOtherCalls();

            _draftCombatantViewModelFactoryMock.VerifyNoOtherCalls();

            _diceRollerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ProceedToInitiative()
        {
            _diceRollerMock.SetupSequence(roller => roller.RollDice(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(5)
                .Returns(3);

            var existingCombatantVm1 = new CombatantViewModel(new()
            {
                Name = "Combatant1",
                AbilityBonuses = new Dictionary<Ability, int> { [Ability.Dexterity] = 2 }
            });
            _combatSetupVm.DraftCombatants.Add(existingCombatantVm1);

            var existingCombatantVm2 = new CombatantViewModel(new()
            {
                Name = "Combatant2",
                AbilityBonuses = new Dictionary<Ability, int> { [Ability.Dexterity] = 5 }
            });
            _combatSetupVm.DraftCombatants.Add(existingCombatantVm2);

            var existingCombatantVm3 = new CombatantViewModel(new() { Name = "Player" }) { IsPlayer = true };
            _combatSetupVm.DraftCombatants.Add(existingCombatantVm3);

            _combatSetupVm.ProceedToInitiative();

            Assert.True(_combatSetupVm.IsInInitiativeMode);
            Assert.Equal(5, _combatSetupVm.DraftCombatants[0].Initiative);
            Assert.Equal(3, _combatSetupVm.DraftCombatants[1].Initiative);
            Assert.Equal(0, _combatSetupVm.DraftCombatants[2].Initiative);

            _combatantViewModelFactoryMock.VerifyNoOtherCalls();

            _draftCombatantViewModelFactoryMock.VerifyNoOtherCalls();

            _diceRollerMock.Verify(roller => roller.RollDice(1, 20, 2));
            _diceRollerMock.Verify(roller => roller.RollDice(1, 20, 5));
            _diceRollerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void BackToCombatantDrafting()
        {
            var existingCombatantVm1 = new CombatantViewModel(new() { Name = "Combatant1" });
            _combatSetupVm.DraftCombatants.Add(existingCombatantVm1);

            var existingCombatantVm2 = new CombatantViewModel(new() { Name = "Combatant2" });
            _combatSetupVm.DraftCombatants.Add(existingCombatantVm2);

            _combatSetupVm.IsInInitiativeMode = true;

            _combatSetupVm.BackToCombatantDrafting();

            Assert.False(_combatSetupVm.IsInInitiativeMode);
            Assert.Equal(2, _combatSetupVm.DraftCombatants.Count);

            _combatantViewModelFactoryMock.VerifyNoOtherCalls();

            _draftCombatantViewModelFactoryMock.VerifyNoOtherCalls();

            _diceRollerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void FinalizeCombatants()
        {
            var existingCombatantVm1 = new CombatantViewModel(new() { Name = "Combatant1" })
            {
                Initiative = 5
            };
            _combatSetupVm.DraftCombatants.Add(existingCombatantVm1);

            var existingCombatantVm2 = new CombatantViewModel(new() { Name = "Combatant2" })
            {
                Initiative = 7
            };
            _combatSetupVm.DraftCombatants.Add(existingCombatantVm2);

            var existingCombatantVm3 = new CombatantViewModel(new() { Name = "Combatant3" })
            {
                Initiative = 3
            };
            _combatSetupVm.DraftCombatants.Add(existingCombatantVm3);

            var finalizedCombatants = _combatSetupVm.FinalizeCombatants();

            var expectedCombatants = new[]
            {
                existingCombatantVm2,
                existingCombatantVm1,
                existingCombatantVm3
            };
            Assert.Equal(expectedCombatants, finalizedCombatants);

            _combatantViewModelFactoryMock.VerifyNoOtherCalls();

            _draftCombatantViewModelFactoryMock.VerifyNoOtherCalls();

            _diceRollerMock.VerifyNoOtherCalls();
        }
    }
}
