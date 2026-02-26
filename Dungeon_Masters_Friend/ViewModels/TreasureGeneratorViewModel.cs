using Avalonia.Data;
using Avalonia.Data.Converters;
using CommunityToolkit.Mvvm.Input;
using Dungeon_Masters_Friend.Models;
using Dungeon_Masters_Friend.Models.Treasure;
using Dungeon_Masters_Friend.Utilities;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using TreasureType = Dungeon_Masters_Friend.Models.Treasure.Type;

namespace Dungeon_Masters_Friend.ViewModels
{
    /// <summary>
    /// View model for the treasure generator panel.
    /// </summary>
    public partial class TreasureGeneratorViewModel : ViewModelBase
    {
        /// <summary>
        /// The available play tiers that can be selected when generating treasure.
        /// </summary>
        /// <remarks>
        /// The play tier determines the total number of dice rolled for treasure generation as well as the rarity
        /// distribution of generated items.
        /// </remarks>
        public static IEnumerable<PlayTier> PlayTiers { get; } = Enum.GetValues<PlayTier>();

        /// <summary>
        /// The available trove profiles that can be selected when generating treasure.
        /// </summary>
        /// <remarks>
        /// The trove profile determines the distribution of item types (trade good, gemstone, art object) for
        /// generated items.
        /// </remarks>
        public static IEnumerable<TroveProfile> TroveProfiles { get; } = Enum.GetValues<TroveProfile>();

        private static readonly Dictionary<PlayTier, int> _totalDiceByTier = new()
        {
            { PlayTier.Apprentice, 2 },
            { PlayTier.Journeyman, 3 },
            { PlayTier.Adventurer, 3 },
            { PlayTier.Veteran, 4 },
            { PlayTier.Champion, 4 },
            { PlayTier.Hero, 5 },
            { PlayTier.Legend, 5 }
        };

        private static readonly Dictionary<PlayTier, int> _coinMultiplierByTier = new()
        {
            { PlayTier.Apprentice, 25  },
            { PlayTier.Journeyman, 50 },
            { PlayTier.Adventurer, 100 },
            { PlayTier.Veteran, 250 },
            { PlayTier.Champion, 1_000 },
            { PlayTier.Hero, 2_500 },
            { PlayTier.Legend, 5_000 }
        };

        private static readonly Dictionary<PlayTier, Dictionary<int, Rarity>> _itemRarityDistributionByTier = new()
        {
            {
                PlayTier.Apprentice,
                new()
                {
                    { 1, Rarity.Common },
                    { 2, Rarity.Common },
                    { 3, Rarity.Common },
                    { 4, Rarity.Common },
                    { 5, Rarity.Common },
                    { 6, Rarity.Uncommon }
                }
            },
            {
                PlayTier.Journeyman,
                new()
                {
                    { 1, Rarity.Common },
                    { 2, Rarity.Common },
                    { 3, Rarity.Uncommon },
                    { 4, Rarity.Uncommon },
                    { 5, Rarity.Uncommon },
                    { 6, Rarity.Rare }

                }
            },
            {
                PlayTier.Adventurer,
                new()
                {
                    { 1, Rarity.Common },
                    { 2, Rarity.Common },
                    { 3, Rarity.Uncommon },
                    { 4, Rarity.Uncommon },
                    { 5, Rarity.Rare },
                    { 6, Rarity.Rare }
                }
            },
            {
                PlayTier.Veteran,
                new()
                {
                    { 1, Rarity.Common },
                    { 2, Rarity.Uncommon },
                    { 3, Rarity.Rare },
                    { 4, Rarity.Rare },
                    { 5, Rarity.Rare },
                    { 6, Rarity.VeryRare }
                }
            },
            {
                PlayTier.Champion,
                new()
                {
                    { 1, Rarity.Uncommon },
                    { 2, Rarity.Rare },
                    { 3, Rarity.Rare },
                    { 4, Rarity.VeryRare },
                    { 5, Rarity.VeryRare },
                    { 6, Rarity.VeryRare }
                }
            },
            {
                PlayTier.Hero,
                new()
                {
                    { 1, Rarity.Rare },
                    { 2, Rarity.Rare },
                    { 3, Rarity.VeryRare },
                    { 4, Rarity.VeryRare },
                    { 5, Rarity.VeryRare },
                    { 6, Rarity.Legendary }
                }
            },
            {
                PlayTier.Legend,
                new()
                {
                    { 1, Rarity.Rare },
                    { 2, Rarity.VeryRare },
                    { 3, Rarity.VeryRare },
                    { 4, Rarity.Legendary },
                    { 5, Rarity.Legendary },
                    { 6, Rarity.Legendary }

                }
            }
        };

        private static readonly Dictionary<TroveProfile, Dictionary<int, TreasureType>> _itemTypeDistributionByProfile = new()
        {
            {
                TroveProfile.Balanced,
                new()
                {
                    { 1, TreasureType.TradeGood },
                    { 2, TreasureType.TradeGood },
                    { 3, TreasureType.Gemstone },
                    { 4, TreasureType.Gemstone },
                    { 5, TreasureType.ArtObject },
                    { 6, TreasureType.ArtObject }
                }
            },
            {
                TroveProfile.LessArtObjects,
                new()
                {
                    { 1, TreasureType.TradeGood },
                    { 2, TreasureType.TradeGood },
                    { 3, TreasureType.TradeGood },
                    { 4, TreasureType.Gemstone },
                    { 5, TreasureType.Gemstone },
                    { 6, TreasureType.ArtObject }
                }
            },
            {
                TroveProfile.LessTradeGoods,
                new()
                {
                    { 1, TreasureType.TradeGood },
                    { 2, TreasureType.Gemstone },
                    { 3, TreasureType.Gemstone },
                    { 4, TreasureType.ArtObject },
                    { 5, TreasureType.ArtObject },
                    { 6, TreasureType.ArtObject }
                }
            },
            {
                TroveProfile.NoTradeGoods,
                new()
                {
                    { 1, TreasureType.Gemstone },
                    { 2, TreasureType.Gemstone },
                    { 3, TreasureType.Gemstone },
                    { 4, TreasureType.ArtObject },
                    { 5, TreasureType.ArtObject },
                    { 6, TreasureType.ArtObject }
                }
            },
            {
                TroveProfile.NoArtObjects,
                new()
                {
                    { 1, TreasureType.TradeGood },
                    { 2, TreasureType.TradeGood },
                    { 3, TreasureType.TradeGood },
                    { 4, TreasureType.Gemstone },
                    { 5, TreasureType.Gemstone },
                    { 6, TreasureType.Gemstone }

                }
            }
        };

        private PlayTier _selectedPlayTier = PlayTier.Apprentice;
        /// <summary>
        /// The currently selected play tier to use for generating treasure.
        /// </summary>
        public PlayTier SelectedPlayTier
        {
            get => _selectedPlayTier;
            set => this.RaiseAndSetIfChanged(ref _selectedPlayTier, value);
        }

        private TroveProfile _selectedTroveProfile = TroveProfile.Balanced;
        /// <summary>
        /// The currently selected trove profile to use for generating treasure.
        /// </summary>
        public TroveProfile SelectedTroveProfile
        {
            get => _selectedTroveProfile;
            set => this.RaiseAndSetIfChanged(ref _selectedTroveProfile, value);
        }

        private int _diceForCoins = 1;
        /// <summary>
        /// The number of dice out of the total available that should be allocated for generating coins as opposed to items.
        /// </summary>
        public int DiceForCoins
        {
            get => _diceForCoins;
            set => this.RaiseAndSetIfChanged(ref _diceForCoins, value);
        }

        private readonly ObservableAsPropertyHelper<int> _totalDice;
        /// <summary>
        /// The total number of dice available for generating treasure. Determined by the selected play tier.
        /// </summary>
        public int TotalDice => _totalDice.Value;

        /// <summary>
        /// The list of generated treasure items.
        /// </summary>
        public ObservableCollection<Item> Items { get; } = [];

        private int _coins = 0;
        /// <summary>
        /// The number of coins in the generated treasure trove.
        /// </summary>
        public int Coins
        {
            get => _coins;
            set => this.RaiseAndSetIfChanged(ref _coins, value);
        }

        private readonly IDiceRoller _diceRoller;

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="diceRoller">The dice roller to use for generating treasure</param>
        public TreasureGeneratorViewModel(IDiceRoller diceRoller)
        {
            _diceRoller = diceRoller;
            _totalDice = this.WhenAnyValue(vm => vm.SelectedPlayTier)
                .Select(tier => _totalDiceByTier[tier])
                .ToProperty(this, vm => vm.TotalDice);
        }

        /// <summary>
        /// Generates a treasure trove based on the currently selected options.
        /// </summary>
        [RelayCommand]
        public void GenerateTreasure()
        {
            Items.Clear();

            Coins = DiceForCoins == 0 ? 0 : _diceRoller.RollDice(DiceForCoins, 6) * _coinMultiplierByTier[SelectedPlayTier];

            if (TotalDice > DiceForCoins)
            {
                var numItems = _diceRoller.RollDice(TotalDice - DiceForCoins, 6);
                for (int i = 0; i < numItems; i++)
                {
                    Items.Add(GenerateItem());
                }
            }
        }

        private Item GenerateItem()
        {
            var type = _itemTypeDistributionByProfile[SelectedTroveProfile][_diceRoller.RollDice(1, 6)];
            var rarity = _itemRarityDistributionByTier[SelectedPlayTier][_diceRoller.RollDice(1, 6)];
            var quality = _diceRoller.RollDice(1, 12) switch
            {
                1 => Quality.Inferior,
                12 => Quality.Superior,
                _ => Quality.Standard
            };
            return new Item(type, rarity, quality);
        }
    }

    /// <summary>
    /// A distribution of item types for generated treasure items.
    /// </summary>
    public enum TroveProfile
    {
        NoArtObjects,
        LessArtObjects,
        Balanced,
        LessTradeGoods,
        NoTradeGoods
    }

    /// <summary>
    /// Converts TroveProfile values to and from a user-friendly strings for display and selection.
    /// </summary>
    public class TroveProfileToStringConverter : IValueConverter
    {
        private static readonly Dictionary<TroveProfile, string> _profileToDisplayString = new()
        {
            { TroveProfile.NoArtObjects, "No Art Objects" },
            { TroveProfile.LessArtObjects, "Less Art Objects" },
            { TroveProfile.Balanced, "Balanced" },
            { TroveProfile.LessTradeGoods, "Less Trade Goods" },
            { TroveProfile.NoTradeGoods, "No Trade Goods" }
        };
        private static readonly Dictionary<string, TroveProfile> _displayStringToProfile = _profileToDisplayString.ToDictionary(entry => entry.Value, entry => entry.Key);

        public object Convert(object? value, System.Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            if (value is TroveProfile profile)
            {
                return _profileToDisplayString[profile];
            }
            return new BindingNotification(new ArgumentException("Expected an object of type TroveProfile but received " + value?.GetType()));
        }

        public object ConvertBack(object? value, System.Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string str)
            {
                return _displayStringToProfile[str];
            }
            return new BindingNotification(new InvalidEnumArgumentException("Cannot convert object of type " + value?.GetType() + " to TroveProfile"));
        }
    }

    /// <summary>
    /// Converts item Quality values to a boolean value indicating whether the item is of non-standard quality
    /// (i.e. either inferior or superior) for display purposes.
    /// </summary>
    public class ItemQualityToIsNonStandardConverter : IValueConverter
    {
        public object Convert(object? value, System.Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Quality quality)
            {
                return quality != Quality.Standard;
            }
            return false;
        }

        public object ConvertBack(object? value, System.Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}
