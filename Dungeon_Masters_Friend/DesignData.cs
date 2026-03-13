using Dungeon_Masters_Friend.Utilities;
using Dungeon_Masters_Friend.ViewModels;

namespace Dungeon_Masters_Friend
{
    /// <summary>
    /// Test data for use during design view design.
    /// </summary>
    public static class DesignData
    {
        public static CombatViewModel CombatViewModel
        {
            get
            {
                var model = new CombatViewModel(null);
                model.Combatants.Add(
                    new(
                        new()
                        {
                            Name = "Combatant 1",
                            MaxHp = 25,
                            ArmorClass = 12
                        }
                    )
                );
                model.Combatants.Add(
                    new(
                        new()
                        {
                            Name = "Combatant 2",
                            MaxHp = 100,
                            ArmorClass = 20
                        }
                    )
                );
                model.Combatants.Add(
                    new(
                        new()
                        {
                            Name = "Combatant 3",
                            MaxHp = 1,
                            ArmorClass = 5
                        }
                    )
                );
                return model;
            }
        }

        public static TreasureGeneratorViewModel TreasureGeneratorViewModel
        {
            get
            {
                var model = new TreasureGeneratorViewModel(new DiceRoller())
                {
                     SelectedPlayTier = Models.PlayTier.Adventurer,
                     SelectedTroveProfile = TroveProfile.LessTradeGoods,
                     DiceForCoins = 1,
                     Coins = 1000
                };
                model.Items.Add(new(Models.Treasure.Type.ArtObject, Models.Treasure.Rarity.Uncommon, Models.Treasure.Quality.Standard));
                model.Items.Add(new(Models.Treasure.Type.TradeGood, Models.Treasure.Rarity.Legendary, Models.Treasure.Quality.Inferior));
                model.Items.Add(new(Models.Treasure.Type.Gemstone, Models.Treasure.Rarity.Rare, Models.Treasure.Quality.Superior));
                return model;
            }
        }
    }
}
