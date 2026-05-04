using Dungeon_Masters_Friend.Models;
using Dungeon_Masters_Friend.Repositories;
using Dungeon_Masters_Friend.Utilities;
using Dungeon_Masters_Friend.ViewModels;
using Moq;

namespace Dungeon_Masters_Friend.Test.ViewModels
{
    public class CombatSetupViewModelTest
    {
        private readonly Mock<IBestiaryRepository> _bestiaryRepositoryMock = new();
        private readonly Mock<ICombatantViewModelFactory> _combatantViewModelFactoryMock = new();
        private readonly Mock<IDraftCreatureViewModelFactory> _draftCreatureViewModelFactoryMock = new();
        private readonly Mock<IDiceRoller> _diceRollerMock = new();

        private readonly CombatSetupViewModel _combatSetupVm;

        public CombatSetupViewModelTest()
        {
            _combatSetupVm = new(
                _combatantViewModelFactoryMock.Object,
                _draftCreatureViewModelFactoryMock.Object,
                _diceRollerMock.Object,
                []
            );
        }

        [Fact]
        public void AddCombatant()
        {
            _draftCreatureViewModelFactoryMock.Setup(factory => factory.Create())
                .Returns(new DraftCreatureViewModel(new(), _bestiaryRepositoryMock.Object));

            _combatantViewModelFactoryMock.Setup(factory => factory.Create(It.IsAny<Creature>()))
                .Returns((Creature creature) => new CombatantViewModel(creature));

            var newCreature = new Creature() { Name = "Combatant 1" };
            _combatSetupVm.AddCombatant.RegisterHandler(interaction => interaction.SetOutput(newCreature));

            _combatSetupVm.AddCombatantCommand.Execute();

            Assert.Single(_combatSetupVm.DraftCombatants);
            Assert.Equal(newCreature, _combatSetupVm.DraftCombatants.First().Creature);

            _combatantViewModelFactoryMock.Verify(factory => factory.Create(newCreature), Times.Once());
            _combatantViewModelFactoryMock.VerifyNoOtherCalls();

            _draftCreatureViewModelFactoryMock.Verify(factory => factory.Create(), Times.Once());
            _draftCreatureViewModelFactoryMock.VerifyNoOtherCalls();

            _diceRollerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void AddCombatant_NullResponse()
        {
            _draftCreatureViewModelFactoryMock.Setup(factory => factory.Create())
                .Returns(new DraftCreatureViewModel(new(), _bestiaryRepositoryMock.Object));

            _combatSetupVm.AddCombatant.RegisterHandler(interaction => interaction.SetOutput(null));

            _combatSetupVm.AddCombatantCommand.Execute();

            Assert.Empty(_combatSetupVm.DraftCombatants);

            _combatantViewModelFactoryMock.VerifyNoOtherCalls();

            _draftCreatureViewModelFactoryMock.Verify(factory => factory.Create(), Times.Once());
            _draftCreatureViewModelFactoryMock.VerifyNoOtherCalls();

            _diceRollerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void EditCombatant()
        {
            _combatantViewModelFactoryMock.Setup(factory => factory.Create(It.IsAny<Creature>()))
                .Returns((Creature creature) => new CombatantViewModel(creature));
            _draftCreatureViewModelFactoryMock.Setup(factory => factory.Create(It.IsAny<Creature>()))
                .Returns((Creature creature) => new DraftCreatureViewModel(creature, _bestiaryRepositoryMock.Object));

            var existingCombatantVm = new CombatantViewModel(new() { Name = "Combatant1" });
            _combatSetupVm.DraftCombatants.Add(existingCombatantVm);

            var existingCombatantVm2 = new CombatantViewModel(new() { Name = "Combatant2" });
            _combatSetupVm.DraftCombatants.Add(existingCombatantVm2);

            var newCreature = new Creature() { Name = "NewCombatant" };
            _combatSetupVm.AddCombatant.RegisterHandler(interaction => interaction.SetOutput(newCreature));

            _combatSetupVm.EditCombatantCommand.Execute(existingCombatantVm);

            Assert.Equal(2, _combatSetupVm.DraftCombatants.Count);
            Assert.Equal(newCreature.Name, _combatSetupVm.DraftCombatants.First().Creature.Name);
            Assert.Equal(existingCombatantVm2, _combatSetupVm.DraftCombatants[1]);

            _combatantViewModelFactoryMock.Verify(factory => factory.Create(newCreature));
            _combatantViewModelFactoryMock.VerifyNoOtherCalls();

            _draftCreatureViewModelFactoryMock.Verify(factory => factory.Create(existingCombatantVm.Creature), Times.Once());
            _draftCreatureViewModelFactoryMock.VerifyNoOtherCalls();

            _diceRollerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void EditCombatant_NullResponse()
        {
            _combatantViewModelFactoryMock.Setup(factory => factory.Create(It.IsAny<Creature>()))
                .Returns((Creature creature) => new CombatantViewModel(creature));
            _draftCreatureViewModelFactoryMock.Setup(factory => factory.Create(It.IsAny<Creature>()))
                .Returns((Creature creature) => new DraftCreatureViewModel(creature, _bestiaryRepositoryMock.Object));

            var existingCombatantVm = new CombatantViewModel(new() { Name = "Combatant1" });
            _combatSetupVm.DraftCombatants.Add(existingCombatantVm);

            var existingCombatantVm2 = new CombatantViewModel(new() { Name = "Combatant2" });
            _combatSetupVm.DraftCombatants.Add(existingCombatantVm2);

            _combatSetupVm.AddCombatant.RegisterHandler(interaction => interaction.SetOutput(null));

            _combatSetupVm.EditCombatantCommand.Execute(existingCombatantVm);

            Assert.Equal(2, _combatSetupVm.DraftCombatants.Count);
            Assert.Equal(existingCombatantVm, _combatSetupVm.DraftCombatants[0]);
            Assert.Equal(existingCombatantVm2, _combatSetupVm.DraftCombatants[1]);

            _combatantViewModelFactoryMock.VerifyNoOtherCalls();

            _draftCreatureViewModelFactoryMock.Verify(factory => factory.Create(existingCombatantVm.Creature), Times.Once());
            _draftCreatureViewModelFactoryMock.VerifyNoOtherCalls();

            _diceRollerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void DuplicateCombatant()
        {
            _combatantViewModelFactoryMock.Setup(factory => factory.Create(It.IsAny<Creature>()))
                .Returns((Creature creature) => new CombatantViewModel(creature));

            var existingCombatantVm1 = new CombatantViewModel(new() { Name = "Combatant1" });
            _combatSetupVm.DraftCombatants.Add(existingCombatantVm1);

            var existingCombatantVm2 = new CombatantViewModel(new() { Name = "Combatant2" });
            _combatSetupVm.DraftCombatants.Add(existingCombatantVm2);

            _combatSetupVm.DuplicateCombatant(existingCombatantVm1);

            Assert.Equal(3, _combatSetupVm.DraftCombatants.Count);
            Assert.Equal(existingCombatantVm1, _combatSetupVm.DraftCombatants[0]);
            Assert.Equal(existingCombatantVm2, _combatSetupVm.DraftCombatants[1]);
            Assert.NotSame(existingCombatantVm1, _combatSetupVm.DraftCombatants[2]);
            Assert.Equal(existingCombatantVm1.Creature.Name, _combatSetupVm.DraftCombatants[2].Creature.Name);

            _combatantViewModelFactoryMock.Verify(factory => factory.Create(existingCombatantVm1.Creature));
            _combatantViewModelFactoryMock.VerifyNoOtherCalls();

            _draftCreatureViewModelFactoryMock.VerifyNoOtherCalls();

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

            _draftCreatureViewModelFactoryMock.VerifyNoOtherCalls();

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

            _draftCreatureViewModelFactoryMock.VerifyNoOtherCalls();

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

            _draftCreatureViewModelFactoryMock.VerifyNoOtherCalls();

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

            _draftCreatureViewModelFactoryMock.VerifyNoOtherCalls();

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

            _draftCreatureViewModelFactoryMock.VerifyNoOtherCalls();

            _diceRollerMock.VerifyNoOtherCalls();
        }
    }
}
