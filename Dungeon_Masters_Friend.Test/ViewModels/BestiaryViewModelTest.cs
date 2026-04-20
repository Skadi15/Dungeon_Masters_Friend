using Dungeon_Masters_Friend.Models;
using Dungeon_Masters_Friend.Repositories;
using Dungeon_Masters_Friend.ViewModels;
using Moq;

namespace Dungeon_Masters_Friend.Test.ViewModels
{
    public class BestiaryViewModelTest
    {
        private readonly Mock<IDraftCreatureViewModelFactory> _draftCreatureViewModelFactoryMock = new();
        private readonly Mock<IBestiaryRepository> _bestiaryRepositoryMock = new();

        private readonly BestiaryViewModel _bestiaryVm;

        public BestiaryViewModelTest()
        {
            // Default repository load returns an empty list so initialization doesn't populate creatures
            _bestiaryRepositoryMock.Setup(repo => repo.LoadAsync()).ReturnsAsync([]);

            _bestiaryVm = new(_draftCreatureViewModelFactoryMock.Object, _bestiaryRepositoryMock.Object);
        }

        [Fact]
        public async Task LoadsCreaturesFromRepositoryOnInitialization()
        {
            var repoMock = new Mock<IBestiaryRepository>();
            var models = new List<Creature>
            {
                new() { Name = "Creature1" },
                new() { Name = "Creature2" }
            };

            repoMock.Setup(r => r.LoadAsync()).ReturnsAsync(models);

            var vm = new BestiaryViewModel(_draftCreatureViewModelFactoryMock.Object, repoMock.Object);

            // Initialization runs asynchronously. Wait briefly for the load to complete.
            var attempts = 0;
            while (vm.Creatures.Count != models.Count && attempts++ < 50)
            {
                await Task.Delay(10);
            }

            Assert.Equal(models.Count, vm.Creatures.Count);
            Assert.Equal([.. models.Select(m => m.Name)], vm.Creatures.Select(cv => cv.Creature.Name).ToList());

            repoMock.Verify(r => r.LoadAsync(), Times.Once());
            repoMock.VerifyNoOtherCalls();
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

            _bestiaryRepositoryMock.Verify(repo => repo.LoadAsync(), Times.Once());
            _bestiaryRepositoryMock.Verify(repo => repo.SaveAsync(It.IsAny<IEnumerable<Creature>>()), Times.Once());
            _bestiaryRepositoryMock.VerifyNoOtherCalls();
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

            _bestiaryRepositoryMock.Verify(repo => repo.LoadAsync(), Times.Once());
            _bestiaryRepositoryMock.VerifyNoOtherCalls();
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

            _bestiaryRepositoryMock.Verify(repo => repo.LoadAsync(), Times.Once());
            _bestiaryRepositoryMock.Verify(repo => repo.SaveAsync(It.IsAny<IEnumerable<Creature>>()), Times.Once());
            _bestiaryRepositoryMock.VerifyNoOtherCalls();
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

            _bestiaryRepositoryMock.Verify(repo => repo.LoadAsync(), Times.Once());
            _bestiaryRepositoryMock.VerifyNoOtherCalls();
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

            _bestiaryRepositoryMock.Verify(repo => repo.LoadAsync(), Times.Once());
            _bestiaryRepositoryMock.Verify(repo => repo.SaveAsync(It.IsAny<IEnumerable<Creature>>()), Times.Once());
            _bestiaryRepositoryMock.VerifyNoOtherCalls();
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

            _bestiaryRepositoryMock.Verify(repo => repo.LoadAsync(), Times.Once());
            _bestiaryRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void RemoveCreature()
        {
            var existingCreatureVm1 = new CreatureViewModel(new() { Name = "Creature1" });
            _bestiaryVm.Creatures.Add(existingCreatureVm1);

            var existingCreatureVm2 = new CreatureViewModel(new() { Name = "Creature2" });
            _bestiaryVm.Creatures.Add(existingCreatureVm2);

            _bestiaryVm.RemoveCreatureCommand.Execute(existingCreatureVm1);

            Assert.Single(_bestiaryVm.Creatures);
            Assert.Equal(existingCreatureVm2, _bestiaryVm.Creatures.First());

            _draftCreatureViewModelFactoryMock.VerifyNoOtherCalls();

            _bestiaryRepositoryMock.Verify(repo => repo.LoadAsync(), Times.Once());
            _bestiaryRepositoryMock.Verify(repo => repo.SaveAsync(It.IsAny<IEnumerable<Creature>>()), Times.Once());
            _bestiaryRepositoryMock.VerifyNoOtherCalls();
        }
    }
}
