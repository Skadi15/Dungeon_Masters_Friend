using System;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon_Masters_Friend.Models
{
    /// <summary>
    /// Template stats for a creature.
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

        /// <summary>
        /// Determines whether the specified creature is equal to the current creature by comparing all properties.
        /// </summary>
        /// <param name="other">The creature to compare with the current creature</param>
        /// <returns>true if the specified creature is equal to the current creature; otherwise, false</returns>
        public override bool Equals(object? obj) => Equals(obj as Creature);

        /// <summary>
        /// Determines whether the specified creature is equal to the current creature by comparing all properties.
        /// </summary>
        /// <param name="other">The creature to compare with the current creature, or null</param>
        /// <returns>true if the specified creature is equal to the current creature; otherwise, false</returns>
        public bool Equals(Creature? other)
        {
            if (other is null)
                return false;

            return Name == other.Name
                && Size == other.Size
                && MaxHp == other.MaxHp
                && ArmorClass == other.ArmorClass
                && WalkingSpeed == other.WalkingSpeed
                && FlyingSpeed == other.FlyingSpeed
                && SwimmingSpeed == other.SwimmingSpeed
                && AbilityBonuses.SequenceEqual(other.AbilityBonuses)
                && Traits.SequenceEqual(other.Traits)
                && Actions.SequenceEqual(other.Actions);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current creature</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + Name.GetHashCode();
                hash = hash * 31 + Size.GetHashCode();
                hash = hash * 31 + MaxHp.GetHashCode();
                hash = hash * 31 + ArmorClass.GetHashCode();
                hash = hash * 31 + WalkingSpeed.GetHashCode();
                hash = hash * 31 + (FlyingSpeed?.GetHashCode() ?? 0);
                hash = hash * 31 + (SwimmingSpeed?.GetHashCode() ?? 0);

                foreach (var bonus in AbilityBonuses)
                {
                    hash = hash * 31 + bonus.Key.GetHashCode();
                    hash = hash * 31 + bonus.Value.GetHashCode();
                }

                foreach (var trait in Traits)
                {
                    hash = hash * 31 + trait.GetHashCode();
                }

                foreach (var action in Actions)
                {
                    hash = hash * 31 + action.GetHashCode();
                }

                return hash;
            }
        }

        /// <summary>
        /// Returns a value that indicates whether two creatures are equal.
        /// </summary>
        /// <param name="left">The first creature to compare</param>
        /// <param name="right">The second creature to compare</param>
        /// <returns>true if left and right are equal; otherwise, false</returns>
        public static bool operator ==(Creature? left, Creature? right)
        {
            if (left is null)
                return right is null;
            return left.Equals(right);
        }

        /// <summary>
        /// Returns a value that indicates whether two creatures are not equal.
        /// </summary>
        /// <param name="left">The first creature to compare</param>
        /// <param name="right">The second creature to compare</param>
        /// <returns>true if left and right are not equal; otherwise, false</returns>
        public static bool operator !=(Creature? left, Creature? right) => !(left == right);
    }
}
