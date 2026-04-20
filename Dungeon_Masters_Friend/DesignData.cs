using Dungeon_Masters_Friend.Utilities;
using Dungeon_Masters_Friend.ViewModels;

namespace Dungeon_Masters_Friend
{
    /// <summary>
    /// Test data for use during design view design.
    /// </summary>
    public static class DesignData
    {
        public static CreatureViewModel CreatureViewModel
        {
            get => new(
                new()
                {
                    Name = "Zombie",
                    MaxHp = 22,
                    ArmorClass = 8,
                    WalkingSpeed = 20,
                    AbilityBonuses =
                    {
                        [Models.Ability.Strength] = 1,
                        [Models.Ability.Dexterity] = -2,
                        [Models.Ability.Constitution] = 3,
                        [Models.Ability.Intelligence] = -4,
                        [Models.Ability.Wisdom] = -2,
                        [Models.Ability.Charisma] = -3
                    },
                    Traits = [
                        "UndeadFortitude. If damage reduces the zombie to 0 hit points, it must make a Constitution saving throw with a DC of 5+the damage taken, unless the damage is radiant or from a critical hit. On a success, the zombie drops to 1 hit point instead."
                    ],
                    Actions = [
                        "Slam. Melee Weapon Attack: +3 to hit, reach 5 ft., one target. Hit: (1d6 + 1) bludgeoning damage."
                    ]
                }
            );
        }

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

        public static DraftCreatureViewModel DraftCreatureViewModel
        {
            get => new(
                new()
                {
                    Name = "Goblin",
                    MaxHp = 7,
                    ArmorClass = 15,
                    WalkingSpeed = 30,
                    AbilityBonuses =
                    {
                        [Models.Ability.Strength] = -2,
                        [Models.Ability.Dexterity] = 2,
                        [Models.Ability.Constitution] = 0,
                        [Models.Ability.Intelligence] = 0,
                        [Models.Ability.Wisdom] = -1,
                        [Models.Ability.Charisma] = -1
                    },
                    Traits = [
                        "Nimble Escape. The goblin can take the Disengage or Hide action as a bonus action on each of its turns."
                    ],
                    Actions = [
                        "Scimitar. Melee Weapon Attack: +4 to hit, reach 5 ft., one target. Hit: (1d6 + 2) slashing damage.",
                        "Shortbow. Ranged Weapon Attack: +4 to hit, range 80/320 ft., one target. Hit: (1d6 + 2) piercing damage."
                    ]
                },
                null
            );
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
