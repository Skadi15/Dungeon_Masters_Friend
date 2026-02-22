using System;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon_Masters_Friend.Models
{
    /// <summary>
    /// Template stats for an entity.
    /// </summary>
    public class Entity
    {
        /// <summary>
        /// The entity's name.
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// The entity's size category.
        /// </summary>
        public Size Size { get; set; }
        /// <summary>
        /// The entity's maximum hit points.
        /// </summary>
        public int MaxHp { get; set; }
        /// <summary>
        /// The entity's armor class.
        /// </summary>
        public int ArmorClass { get; set; }
        /// <summary>
        /// The entity's walking speed in feet/round.
        /// </summary>
        public int WalkingSpeed { get; set; }
        /// <summary>
        /// The entity's flying speed in feet/round.
        /// </summary>
        public int? FlyingSpeed { get; set; }
        /// <summary>
        /// The entity's swimming speed in feet/round.
        /// </summary>
        public int? SwimmingSpeed { get; set; }
        /// <summary>
        /// A dictionary of the entity's ability score bonuses.
        /// </summary>
        public Dictionary<Ability, int> AbilityBonuses { get; set; } = Enum.GetValues<Ability>()
            .Cast<Ability>()
            .ToDictionary(keySelector: value => value, elementSelector: _ => 0);
        /// <summary>
        /// The entity's initiative modifier by deriving it from their Dexterity ability bonus.
        /// </summary>
        public int InitiativeModifier { get => AbilityBonuses.GetValueOrDefault(Ability.Dexterity, 0); }
        /// <summary>
        /// A list of the entity's traits.
        /// </summary>
        public List<string> Traits { get; set; } = [];
        /// <summary>
        /// A list of the entity's in-combat actions.
        /// </summary>
        public List<string> Actions { get; set; } = [];

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Entity() { }

        /// <summary>
        /// Copy constructor. Performs a deep copy.
        /// </summary>
        /// <param name="other">The entity to copy</param>
        public Entity(Entity other)
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
