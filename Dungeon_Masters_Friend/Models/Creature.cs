using System;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon_Masters_Friend.Models
{
    /// <summary>
    /// Template stats for an creature.
    /// </summary>
    public class Creature
    {
        /// <summary>
        /// The creature's name.
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// The creature's size category.
        /// </summary>
        public Size Size { get; set; }
        /// <summary>
        /// The creature's maximum hit points.
        /// </summary>
        public int MaxHp { get; set; }
        /// <summary>
        /// The creature's armor class.
        /// </summary>
        public int ArmorClass { get; set; }
        /// <summary>
        /// The creature's walking speed in feet/round.
        /// </summary>
        public int WalkingSpeed { get; set; }
        /// <summary>
        /// The creature's flying speed in feet/round.
        /// </summary>
        public int? FlyingSpeed { get; set; }
        /// <summary>
        /// The creature's swimming speed in feet/round.
        /// </summary>
        public int? SwimmingSpeed { get; set; }
        /// <summary>
        /// A dictionary of the creature's ability score bonuses.
        /// </summary>
        public Dictionary<Ability, int> AbilityBonuses { get; set; } = Enum.GetValues<Ability>()
            .Cast<Ability>()
            .ToDictionary(keySelector: value => value, elementSelector: _ => 0);
        /// <summary>
        /// The creature's initiative modifier by deriving it from their Dexterity ability bonus.
        /// </summary>
        public int InitiativeModifier { get => AbilityBonuses.GetValueOrDefault(Ability.Dexterity, 0); }
        /// <summary>
        /// A list of the creature's traits.
        /// </summary>
        public List<string> Traits { get; set; } = [];
        /// <summary>
        /// A list of the creature's in-combat actions.
        /// </summary>
        public List<string> Actions { get; set; } = [];

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Creature() { }

        /// <summary>
        /// Copy constructor. Performs a deep copy.
        /// </summary>
        /// <param name="other">The creature to copy</param>
        public Creature(Creature other)
        {
            Name = other.Name;
            Size = other.Size;
            MaxHp = other.MaxHp;
            ArmorClass = other.ArmorClass;
            WalkingSpeed = other.WalkingSpeed;
            FlyingSpeed = other.FlyingSpeed;
            SwimmingSpeed = other.SwimmingSpeed;
            AbilityBonuses = new Dictionary<Ability, int>(other.AbilityBonuses);
            Traits = [.. other.Traits];
            Actions = [.. other.Actions];
        }
    }
}
