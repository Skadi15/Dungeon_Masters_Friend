using Dungeon_Masters_Friend.Models;
using Dungeon_Masters_Friend.Repositories;
using Dungeon_Masters_Friend.ViewModels;
using Moq;
using System.Reactive.Linq;

namespace Dungeon_Masters_Friend.Test.ViewModels
{
    public class DraftCreatureViewModelTest
    {
        private readonly Mock<IBestiaryRepository> _bestiaryRepository = new();
        private readonly Creature _creature = new();

        private readonly DraftCreatureViewModel _draftCreatureVm;

        public DraftCreatureViewModelTest()
        {
            _draftCreatureVm = new(_creature, _bestiaryRepository.Object);
        }

        [Fact]
        public async Task BestiaryLoadsAtStartup()
        {
            var bestiary = new List<Creature>
            {
                new() { Name = "Creature1" },
                new() { Name = "Creature2" }
            };
            var bestiaryRepository = new Mock<IBestiaryRepository>();
            bestiaryRepository.Setup(repo => repo.LoadAsync()).ReturnsAsync(bestiary);

            var vm = new DraftCreatureViewModel(new Creature(), bestiaryRepository.Object);

            // Initialization runs asynchronously. Wait briefly for the load to complete.
            var attempts = 0;
            while (vm.Bestiary.Count != bestiary.Count && attempts++ < 50)
            {
                await Task.Delay(10);
            }

            Assert.Equal(bestiary.Count, vm.Bestiary.Count);
            Assert.Equal([.. bestiary.Select(m => m.Name)], vm.Bestiary.Select(cv => cv.Name).ToList());
            bestiaryRepository.Verify(repo => repo.LoadAsync(), Times.Once());
            bestiaryRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public void AddTrait()
        {
            Assert.Empty(_draftCreatureVm.Traits);

            _draftCreatureVm.AddTraitCommand.Execute().Subscribe();

            Assert.Single(_draftCreatureVm.Traits);
            Assert.Empty(_creature.Traits);
        }

        [Fact]
        public void RemoveTrait()
        {
            var traitToKeep = new ObservableString("keep");
            var traitToRemove = new ObservableString("remove");
            _draftCreatureVm.Traits.Add(traitToKeep);
            _draftCreatureVm.Traits.Add(traitToRemove);

            _draftCreatureVm.RemoveTraitCommand.Execute(traitToRemove).Subscribe();

            Assert.Single(_draftCreatureVm.Traits);
            Assert.Equal(traitToKeep, _draftCreatureVm.Traits.First());
            Assert.Empty(_creature.Traits);
        }

        [Fact]
        public void AddAction()
        {
            Assert.Empty(_draftCreatureVm.Actions);

            _draftCreatureVm.AddActionCommand.Execute().Subscribe();

            Assert.Single(_draftCreatureVm.Actions);
            Assert.Empty(_creature.Actions);
        }

        [Fact]
        public void RemoveAction()
        {
            var actionToKeep = new ObservableString("keep");
            var actionToRemove = new ObservableString("remove");
            _draftCreatureVm.Actions.Add(actionToKeep);
            _draftCreatureVm.Actions.Add(actionToRemove);

            _draftCreatureVm.RemoveActionCommand.Execute(actionToRemove).Subscribe();

            Assert.Single(_draftCreatureVm.Actions);
            Assert.Equal(actionToKeep, _draftCreatureVm.Actions.First());
            Assert.Empty(_creature.Actions);
        }
    }
}
