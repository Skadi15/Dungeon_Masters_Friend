using CommunityToolkit.Mvvm.Input;
using Dungeon_Masters_Friend.Repositories;
using DynamicData;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Dungeon_Masters_Friend.ViewModels
{
    /// <summary>
    /// View model for the bestiary page, which allows users to manage their collection of creatures.
    /// </summary>
    public partial class BestiaryViewModel : ViewModelBase
    {
        private readonly IDraftCreatureViewModelFactory _draftCreatureViewModelFactory;
        private readonly IBestiaryRepository _bestiaryRepository;

        /// <summary>
        /// The collection of creatures in the bestiary.
        /// </summary>
        public ObservableCollection<CreatureViewModel> Creatures { get; } = [];

        /// <summary>
        /// The interaction used to add a creature to or edit a creature in the bestiary.
        /// </summary>
        public Interaction<DraftCreatureViewModel, CreatureViewModel?> AddCreature { get; } = new();
        /// <summary>
        /// The command to show the configuration dialog for a new creature.
        /// </summary>
        public ReactiveCommand<Unit, Unit> AddCreatureCommand { get; }
        /// <summary>
        /// The command to show the configuration dialog for an existing creature.
        /// </summary>
        public ReactiveCommand<CreatureViewModel, Unit> EditCreatureCommand { get; }
        /// <summary>
        /// The command to show the configuration dialog for a new creature, using an existing creature as a template.
        /// </summary>
        public ReactiveCommand<CreatureViewModel, Unit> AddCreatureFromExistingCommand { get; }

        public BestiaryViewModel(IDraftCreatureViewModelFactory draftCreatureViewModelFactory, IBestiaryRepository bestiaryRepository)
        {
            _draftCreatureViewModelFactory = draftCreatureViewModelFactory;
            _bestiaryRepository = bestiaryRepository;

            AddCreatureCommand = ReactiveCommand.CreateFromTask(AddCreatureAsync);
            EditCreatureCommand = ReactiveCommand.CreateFromTask<CreatureViewModel, Unit>(EditCreatureAsync);
            AddCreatureFromExistingCommand = ReactiveCommand.CreateFromTask<CreatureViewModel, Unit>(AddCreatureFromExistingAsync);

            // Load the bestiary on initialization
            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            var models = await _bestiaryRepository.LoadAsync();
            Creatures.AddRange(models.Select(m => new CreatureViewModel(m)));
        }

        private async Task AddCreatureAsync()
        {
            var result = await AddCreature.Handle(_draftCreatureViewModelFactory.Create());

            if (result is not null)
            {
                Creatures.Add(result);
                await _bestiaryRepository.SaveAsync(Creatures.Select(c => c.Creature));
            }
        }

        private async Task<Unit> EditCreatureAsync(CreatureViewModel creatureVm)
        {
            var result = await AddCreature.Handle(_draftCreatureViewModelFactory.Create(creatureVm.Creature));

            if (result != null)
            {
                Creatures.Replace(creatureVm, result);
                await _bestiaryRepository.SaveAsync(Creatures.Select(c => c.Creature));
            }

            // Return Unit.Default to satisfy Command typing requirements.
            return Unit.Default;
        }

        private async Task<Unit> AddCreatureFromExistingAsync(CreatureViewModel creatureVm)
        {
            var result = await AddCreature.Handle(_draftCreatureViewModelFactory.Create(creatureVm.Creature));

            if (result is not null)
            {
                Creatures.Add(result);
                await _bestiaryRepository.SaveAsync(Creatures.Select(c => c.Creature));
            }

            // Return Unit.Default to satisfy Command typing requirements.
            return Unit.Default;
        }

        /// <summary>
        /// Removes the specified creature from the bestiary.
        /// </summary>
        /// <param name="creatureVm">The creature to remove</param>
        [RelayCommand]
        public async Task RemoveCreature(CreatureViewModel creatureVm)
        {
            Creatures.Remove(creatureVm);
            await _bestiaryRepository.SaveAsync(Creatures.Select(c => c.Creature));
        }
    }
}
