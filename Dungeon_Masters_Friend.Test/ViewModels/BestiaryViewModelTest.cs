using Dungeon_Masters_Friend.Models;
using Dungeon_Masters_Friend.ViewModels;
using Moq;

namespace Dungeon_Masters_Friend.Test.ViewModels
{
    public class BestiaryViewModelTest
    {
        private readonly Mock<IDraftCreatureViewModelFactory> _draftCreatureViewModelFactoryMock = new();

        private readonly BestiaryViewModel _bestiaryVm;

        public BestiaryViewModelTest()
        {
            _bestiaryVm = new(_draftCreatureViewModelFactoryMock.Object);
        }

        [Fact]
        public void AddCreature()
        {
            _draftCreatureViewModelFactoryMock.Setup(factory => factory.Create())
                .Returns(new DraftCreatureViewModel(new()));

            var newCreatureVm = new CreatureViewModel(new() { Name = "Creature1" });
            _bestiaryVm.AddCreature.RegisterHandler(interaction => interaction.SetOutput(newCreatureVm));

            _bestiaryVm.AddCreatureCommand.Execute();

            Assert.Single(_bestiaryVm.Creatures);
            Assert.Equal(newCreatureVm, _bestiaryVm.Creatures.First());

            _draftCreatureViewModelFactoryMock.Verify(factory => factory.Create(), Times.Once());
            _draftCreatureViewModelFactoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void AddCreature_NullResponse()
        {
            _draftCreatureViewModelFactoryMock.Setup(factory => factory.Create())
                .Returns(new DraftCreatureViewModel(new()));

            _bestiaryVm.AddCreature.RegisterHandler(interaction => interaction.SetOutput(null));

            _bestiaryVm.AddCreatureCommand.Execute();

            Assert.Empty(_bestiaryVm.Creatures);

            _draftCreatureViewModelFactoryMock.Verify(factory => factory.Create(), Times.Once());
            _draftCreatureViewModelFactoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void EditCreature()
        {
            _draftCreatureViewModelFactoryMock.Setup(factory => factory.Create(It.IsAny<Creature>()))
                .Returns((Creature creature) => new DraftCreatureViewModel(creature));

            var existingCreatureVm = new CreatureViewModel(new() { Name = "Creature1" });
            _bestiaryVm.Creatures.Add(existingCreatureVm);

            var existingCreatureVm2 = new CreatureViewModel(new() { Name = "Creature2" });
            _bestiaryVm.Creatures.Add(existingCreatureVm2);

            var newCreatureVm = new CreatureViewModel(new() { Name = "NewCreature" });
            _bestiaryVm.AddCreature.RegisterHandler(interaction => interaction.SetOutput(newCreatureVm));

            _bestiaryVm.EditCreatureCommand.Execute(existingCreatureVm);

            Assert.Equal(2, _bestiaryVm.Creatures.Count);
            Assert.Equal(newCreatureVm, _bestiaryVm.Creatures.First());
            Assert.Equal(existingCreatureVm2, _bestiaryVm.Creatures[1]);

            _draftCreatureViewModelFactoryMock.Verify(factory => factory.Create(existingCreatureVm.Creature));
            _draftCreatureViewModelFactoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void EditCreature_NullResponse()
        {
            _draftCreatureViewModelFactoryMock.Setup(factory => factory.Create(It.IsAny<Creature>()))
                .Returns((Creature creature) => new DraftCreatureViewModel(creature));

            var existingCreatureVm = new CreatureViewModel(new() { Name = "Creature1" });
            _bestiaryVm.Creatures.Add(existingCreatureVm);

            var existingCreatureVm2 = new CreatureViewModel(new() { Name = "Creature2" });
            _bestiaryVm.Creatures.Add(existingCreatureVm2);

            _bestiaryVm.AddCreature.RegisterHandler(interaction => interaction.SetOutput(null));

            _bestiaryVm.EditCreatureCommand.Execute(existingCreatureVm);

            Assert.Equal(2, _bestiaryVm.Creatures.Count);
            Assert.Equal(existingCreatureVm, _bestiaryVm.Creatures[0]);
            Assert.Equal(existingCreatureVm2, _bestiaryVm.Creatures[1]);

            _draftCreatureViewModelFactoryMock.Verify(factory => factory.Create(existingCreatureVm.Creature));
            _draftCreatureViewModelFactoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void AddCreatureFromExisting()
        {
            _draftCreatureViewModelFactoryMock.Setup(factory => factory.Create(It.IsAny<Creature>()))
                .Returns((Creature creature) => new DraftCreatureViewModel(creature));

            var existingCreatureVm = new CreatureViewModel(new() { Name = "Creature1" });

            var newCreatureVm = new CreatureViewModel(new() { Name = "NewCreature" });
            _bestiaryVm.AddCreature.RegisterHandler(interaction => interaction.SetOutput(newCreatureVm));

            _bestiaryVm.AddCreatureFromExistingCommand.Execute(existingCreatureVm);

            Assert.Single(_bestiaryVm.Creatures);
            Assert.Equal(newCreatureVm, _bestiaryVm.Creatures.First());

            _draftCreatureViewModelFactoryMock.Verify(factory => factory.Create(existingCreatureVm.Creature));
            _draftCreatureViewModelFactoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void AddCreatureFromExisting_NullResponse()
        {
            _draftCreatureViewModelFactoryMock.Setup(factory => factory.Create(It.IsAny<Creature>()))
                .Returns((Creature creature) => new DraftCreatureViewModel(creature));

            var existingCreatureVm = new CreatureViewModel(new() { Name = "Creature1" });

            _bestiaryVm.AddCreature.RegisterHandler(interaction => interaction.SetOutput(null));

            _bestiaryVm.AddCreatureFromExistingCommand.Execute(existingCreatureVm);

            Assert.Empty(_bestiaryVm.Creatures);

            _draftCreatureViewModelFactoryMock.Verify(factory => factory.Create(existingCreatureVm.Creature));
            _draftCreatureViewModelFactoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void RemoveCreature()
        {
            var existingCreatureVm1 = new CreatureViewModel(new() { Name = "Creature1" });
            _bestiaryVm.Creatures.Add(existingCreatureVm1);

            var existingCreatureVm2 = new CreatureViewModel(new() { Name = "Creature2" });
            _bestiaryVm.Creatures.Add(existingCreatureVm2);

            _bestiaryVm.RemoveCreature(existingCreatureVm1);

            Assert.Single(_bestiaryVm.Creatures);
            Assert.Equal(existingCreatureVm2, _bestiaryVm.Creatures.First());

            _draftCreatureViewModelFactoryMock.VerifyNoOtherCalls();
        }
    }
}
