using Dungeon_Masters_Friend.Models;
using Dungeon_Masters_Friend.Models.Treasure;
using Dungeon_Masters_Friend.Utilities;
using Dungeon_Masters_Friend.ViewModels;
using Moq;
using TreasureType = Dungeon_Masters_Friend.Models.Treasure.Type;

namespace Dungeon_Masters_Friend.Test.ViewModels
{
    public class TreasureGeneratorViewModelTest
    {
        private readonly Mock<IDiceRoller> _diceRollerMock = new();

        private readonly TreasureGeneratorViewModel _treasureGeneratorVm;

        public TreasureGeneratorViewModelTest()
        {
            _treasureGeneratorVm = new(_diceRollerMock.Object);
        }

        [Fact]
        public void GenerateTreasure_Defaults()
        {
            _diceRollerMock.SetupSequence(r => r.RollDice(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(2)   // Coins
                .Returns(3)   // Num items
                .Returns(1)   // Item type 1/3
                .Returns(4)   // Item rarity 1/3
                .Returns(8)   // Item quality 1/3
                .Returns(6)   // Item type 2/3
                .Returns(6)   // Item rarity 2/3
                .Returns(1)   // Item quality 2/3
                .Returns(4)   // Item type 3/3
                .Returns(1)   // Item rarity 3/3
                .Returns(12); // Item quality 3/3

            _treasureGeneratorVm.GenerateTreasure();

            Assert.Equal(50, _treasureGeneratorVm.Coins);

            var expectedItems = new List<Item>()
            {
                { new Item(TreasureType.TradeGood, Rarity.Common, Quality.Standard) },
                { new Item(TreasureType.ArtObject, Rarity.Uncommon, Quality.Inferior) },
                { new Item(TreasureType.Gemstone, Rarity.Common, Quality.Superior) }
            };
            Assert.Equal(expectedItems, _treasureGeneratorVm.Items);

            _diceRollerMock.Verify(r => r.RollDice(1, 6), Times.Exactly(8));
            _diceRollerMock.Verify(r => r.RollDice(1, 12), Times.Exactly(3));
            _diceRollerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void GenerateTreasure_AllCoins()
        {
            _diceRollerMock.SetupSequence(r => r.RollDice(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(11);  // Coins

            _treasureGeneratorVm.SelectedPlayTier = PlayTier.Adventurer;
            _treasureGeneratorVm.DiceForCoins = _treasureGeneratorVm.TotalDice;
            _treasureGeneratorVm.Items.Add(new Item(TreasureType.TradeGood, Rarity.Common, Quality.Standard));

            _treasureGeneratorVm.GenerateTreasure();

            Assert.Equal(1100, _treasureGeneratorVm.Coins);
            Assert.Empty(_treasureGeneratorVm.Items);

            _diceRollerMock.Verify(r => r.RollDice(3, 6), Times.Once());
        }

        [Fact]
        public void GenerateTreasure_AllItems()
        {
            _diceRollerMock.SetupSequence(r => r.RollDice(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(3)  // Num items
                .Returns(1)   // Item type 1/3
                .Returns(4)   // Item rarity 1/3
                .Returns(8)   // Item quality 1/3
                .Returns(6)   // Item type 2/3
                .Returns(6)   // Item rarity 2/3
                .Returns(1)   // Item quality 2/3
                .Returns(4)   // Item type 3/3
                .Returns(1)   // Item rarity 3/3
                .Returns(12); // Item quality 3/3

            _treasureGeneratorVm.Coins = 200;
            _treasureGeneratorVm.DiceForCoins = 0;
            _treasureGeneratorVm.Items.Add(new Item(TreasureType.TradeGood, Rarity.Common, Quality.Standard));

            _treasureGeneratorVm.GenerateTreasure();

            Assert.Equal(0, _treasureGeneratorVm.Coins);

            var expectedItems = new List<Item>()
            {
                { new Item(TreasureType.TradeGood, Rarity.Common, Quality.Standard) },
                { new Item(TreasureType.ArtObject, Rarity.Uncommon, Quality.Inferior) },
                { new Item(TreasureType.Gemstone, Rarity.Common, Quality.Superior) }
            };
            Assert.Equal(expectedItems, _treasureGeneratorVm.Items);

            _diceRollerMock.Verify(r => r.RollDice(2, 6), Times.Once());
            _diceRollerMock.Verify(r => r.RollDice(1, 6), Times.Exactly(6));
            _diceRollerMock.Verify(r => r.RollDice(1, 12), Times.Exactly(3));
            _diceRollerMock.VerifyNoOtherCalls();
        }
    }
}
