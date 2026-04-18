using Dungeon_Masters_Friend.Models;
using Dungeon_Masters_Friend.ViewModels;
using System.Reactive.Linq;

namespace Dungeon_Masters_Friend.Test.ViewModels
{
    public class DraftCreatureViewModelTest
    {
        private readonly Creature _creature;

        private readonly DraftCreatureViewModel _draftCreatureVm;

        public DraftCreatureViewModelTest()
        {
            _creature = new();

            _draftCreatureVm = new(_creature);
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
