using Dungeon_Masters_Friend.Models;

namespace Dungeon_Masters_Friend.ViewModels
{
    /// <summary>
    /// View model for a creature, which is used to display a creature in the bestiary.
    /// </summary>
    /// <param name="creature">The creature model</param>
    public class CreatureViewModel(Creature creature) : ViewModelBase
    {
        /// <summary>
        /// The underlying model.
        /// </summary>
        public Creature Creature { get; } = creature;
    }
}
