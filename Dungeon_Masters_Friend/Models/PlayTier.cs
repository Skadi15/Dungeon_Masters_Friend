namespace Dungeon_Masters_Friend.Models
{
    /// <summary>
    /// Character level-based tiers of play. Used for determining appropriate challenges, rewards, and content.
    /// </summary>
    public enum PlayTier
    {
        /// <summary>
        /// Levels 1-2
        /// </summary>
        Apprentice,
        /// <summary>
        /// Levels 3-5
        /// </summary>
        Journeyman,
        /// <summary>
        /// Levels 6-8
        /// </summary>
        Adventurer,
        /// <summary>
        /// Levels 9-11
        /// </summary>
        Veteran,
        /// <summary>
        /// Levels 12-14
        /// </summary>
        Champion,
        /// <summary>
        /// Levels 15-17
        /// </summary>
        Hero,
        /// <summary>
        /// Levels 18+
        /// </summary>
        Legend
    }
}
