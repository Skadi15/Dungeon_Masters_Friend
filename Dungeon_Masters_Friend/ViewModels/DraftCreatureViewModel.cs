using DialogHostAvalonia;
using Dungeon_Masters_Friend.Models;
using Dungeon_Masters_Friend.Repositories;
using DynamicData;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

namespace Dungeon_Masters_Friend.ViewModels
{
    /// <summary>
    /// View model for drafting or editing a creature.
    /// </summary>
    public class DraftCreatureViewModel : ViewModelBase
    {
        /// <summary>
        /// Gets a collection containing all defined values of the <see cref="Size"/> enumeration.
        /// </summary>
        public static IEnumerable<Size> Sizes { get; } = Enum.GetValues<Size>();

        private readonly IBestiaryRepository _bestiaryRepository;

        /// <summary>
        /// The collection of creatures in the bestiary, used for selecting a template to draft from.
        /// </summary>
        public ObservableCollection<Creature> Bestiary { get; } = [];
        private Creature? _selectedCreatureTemplate;
        /// <summary>
        /// The currently selected creature template to draft from. When set, the drafting fields will be populated with the template's values.
        /// </summary>
        public Creature? SelectedCreatureTemplate
        {
            get => _selectedCreatureTemplate;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedCreatureTemplate, value);
                OnSelectedCreatureTemplateChanged(value);
            }
        }
        private Creature _creature;
        /// <summary>
        /// The underlying creature model.
        /// </summary>
        public Creature Creature
        {
            get => _creature;
            private set
            {
                this.RaiseAndSetIfChanged(ref _creature, value);
            }
        }
        /// <summary>
        /// An observable collection of the creature's traits.
        /// </summary>
        public ObservableCollection<ObservableString> Traits { get; }
        /// <summary>
        /// An observable collection of the creature's actions.
        /// </summary>
        public ObservableCollection<ObservableString> Actions { get; }
        /// <summary>
        /// A value indicating whether the item has been submitted to be added.
        /// </summary>
        public bool IsSubmitted { get; private set; } = false;
        /// <summary>
        /// The command that adds another trait to the creature.
        /// </summary>
        public ReactiveCommand<Unit, Unit> AddTraitCommand { get; }
        /// <summary>
        /// The command that removes a trait from the creature.
        /// </summary>
        public ReactiveCommand<ObservableString, Unit> RemoveTraitCommand { get; }
        /// <summary>
        /// The command that adds another action to the creature.
        /// </summary>
        public ReactiveCommand<Unit, Unit> AddActionCommand { get; }
        /// <summary>
        /// The command that removes an action from the creature.
        /// </summary>
        public ReactiveCommand<ObservableString, Unit> RemoveActionCommand { get; }
        /// <summary>
        /// The command that submits the drafted creature.
        /// </summary>
        public ReactiveCommand<Unit, Unit> SubmitCommand { get; }
        /// <summary>
        /// The command that cancels the drafting process.
        /// </summary>
        public ReactiveCommand<Unit, Unit> CancelCommand { get; }

        public DraftCreatureViewModel(Creature creature, IBestiaryRepository bestiaryRepository)
        {
            Creature = creature;
            _bestiaryRepository = bestiaryRepository;

            Traits = new (Creature.Traits.ConvertAll(trait => new ObservableString(trait)));
            Actions = new(Creature.Actions.ConvertAll(action => new ObservableString(action)));

            AddTraitCommand = ReactiveCommand.Create(AddTrait);
            RemoveTraitCommand = ReactiveCommand.Create<ObservableString>(RemoveTrait);
            AddActionCommand = ReactiveCommand.Create(AddAction);
            RemoveActionCommand = ReactiveCommand.Create<ObservableString>(RemoveAction);
            SubmitCommand = ReactiveCommand.Create(Submit);
            CancelCommand = ReactiveCommand.Create(Cancel);

            _ = LoadBestiaryAsync(creature);
        }

        private async Task LoadBestiaryAsync(Creature sourceCreature)
        {
            var bestiary = await _bestiaryRepository.LoadAsync();
            Bestiary.Clear();
            Bestiary.AddRange(bestiary);

            if (Bestiary.Contains(sourceCreature))
            {
                SelectedCreatureTemplate = Bestiary.First(creature => creature.Equals(sourceCreature));
            }
        }

        private void OnSelectedCreatureTemplateChanged(Creature? selected)
        {
            if (selected != null)
            {
                Creature = new(selected);
                Traits.Clear();
                Traits.AddRange(Creature.Traits.Select(trait => new ObservableString(trait)));
                Actions.Clear();
                Actions.AddRange(Creature.Actions.Select(action => new ObservableString(action)));
            }
        }

        private void AddTrait()
        {
            Traits.Add(new());
        }

        private void RemoveTrait(ObservableString trait) => Traits.Remove(trait);

        private void AddAction() => Actions.Add(new());

        private void RemoveAction(ObservableString action) => Actions.Remove(action);

        private void Submit()
        {
            Creature.Traits = [.. Traits.Select(trait => trait.Value)];
            Creature.Actions = [.. Actions.Select(trait => trait.Value)];
            IsSubmitted = true;
            DialogHost.Close(null);
        }

        private void Cancel()
        {
            DialogHost.Close(null);
        }
    }

    /// <summary>
    /// Factory for creating instances of <see cref="DraftCreatureViewModel"/> with dependency injection.
    /// </summary>
    public interface IDraftCreatureViewModelFactory
    {
        /// <summary>
        /// Creates an injected instance of <see cref="DraftCreatureViewModel"/> with a new <see cref="Creature"/>.
        /// </summary>
        /// <returns>An injected <see cref="DraftCreatureViewModel"/> instance</returns>
        DraftCreatureViewModel Create();

        /// <summary>
        /// Creates an injected instance of <see cref="DraftCreatureViewModel"/> from an existing <see cref="Creature"/>.
        /// </summary>
        /// <returns>An injected <see cref="DraftCreatureViewModel"/> instance</returns>
        DraftCreatureViewModel Create(Creature creature);
    }

    /// <inheritdoc cref="IDraftCreatureViewModelFactory"/>
    /// <summary>
    /// Basic constructor
    /// </summary>
    /// <param name="serviceProvider">Service provider that provides <see cref="DraftCreatureViewModel"/> instances</param>
    public class DraftCreatureViewModelFactory(IServiceProvider serviceProvider) : IDraftCreatureViewModelFactory
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public DraftCreatureViewModel Create()
        {
            return ActivatorUtilities.CreateInstance<DraftCreatureViewModel>(_serviceProvider, new Creature());
        }

        public DraftCreatureViewModel Create(Creature creature)
        {
            return ActivatorUtilities.CreateInstance<DraftCreatureViewModel>(_serviceProvider, creature);
        }
    }

    /// <summary>
    /// Represents a string value that notifies observers when its value changes.
    /// </summary>
    /// <param name="value">The initial value of the observable string. If not specified, the value is initialized to an empty string.</param>
    public class ObservableString(string value = "") : ReactiveObject
    {
        private string _value = value;
        /// <summary>
        /// Gets or sets the current value represented by this instance.
        /// </summary>
        public string Value
        {
            get => _value;
            set => this.RaiseAndSetIfChanged(ref _value, value);
        }

        public override string ToString() => Value;
    }
}
