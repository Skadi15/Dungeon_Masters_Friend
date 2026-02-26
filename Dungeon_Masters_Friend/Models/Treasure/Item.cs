namespace Dungeon_Masters_Friend.Models.Treasure
{
    /// <summary>
    /// A single item within a treasure trove
    /// </summary>
    /// <param name="Type">The type of the item</param>
    /// <param name="Rarity">The rarity of the item</param>
    /// <param name="Quality">The quality of the item</param>
    public record Item(Type Type, Rarity Rarity, Quality Quality);
}
