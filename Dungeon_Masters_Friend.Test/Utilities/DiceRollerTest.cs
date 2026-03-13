using Dungeon_Masters_Friend.Utilities;
using Moq;

namespace Dungeon_Masters_Friend.Test.Utilities
{
    public class DiceRollerTest
    {
        [Fact]
        public void RollDice_WithBonus()
        {
            var randomMock = new Mock<Random>();
            randomMock.SetupSequence(r => r.Next(1, 19))
                .Returns(1)
                .Returns(5)
                .Returns(20)
                .Returns(12);

            var diceRoller = new DiceRoller(randomMock.Object);

            var result = diceRoller.RollDice(4, 18, 3);

            Assert.Equal(41, result);

            randomMock.Verify(r => r.Next(1, 19), Times.Exactly(4));
            randomMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void RollDice_InvalidNumberOfDice()
        {
            var diceRoller = new DiceRoller();

            var exception = Assert.Throws<ArgumentException>(() => diceRoller.RollDice(0, 20, 0));

            Assert.Equal("Number of dice and sides per die must be positive integers", exception.Message);
        }

        [Fact]
        public void RollDice_InvalidSidesPerDie()
        {
            var diceRoller = new DiceRoller();

            var exception = Assert.Throws<ArgumentException>(() => diceRoller.RollDice(2, 0, 0));

            Assert.Equal("Number of dice and sides per die must be positive integers", exception.Message);
        }

        [Fact]
        public void RollDice_NoBonus()
        {
            var randomMock = new Mock<Random>();
            randomMock.SetupSequence(r => r.Next(1, 19))
                .Returns(1)
                .Returns(5)
                .Returns(20)
                .Returns(12);

            var diceRoller = new DiceRoller(randomMock.Object);

            var result = diceRoller.RollDice(4, 18);

            Assert.Equal(38, result);

            randomMock.Verify(r => r.Next(1, 19), Times.Exactly(4));
            randomMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void RollWithAdvantage()
        {
            var randomMock = new Mock<Random>();
            randomMock.SetupSequence(r => r.Next(1, 21))
                .Returns(5)
                .Returns(15);

            var diceRoller = new DiceRoller(randomMock.Object);

            var result = diceRoller.RollWithAdvantage(3);

            Assert.Equal(18, result);

            randomMock.Verify(r => r.Next(1, 21), Times.Exactly(2));
            randomMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void RollWithDisadvantage()
        {
            var randomMock = new Mock<Random>();
            randomMock.SetupSequence(r => r.Next(1, 21))
                .Returns(5)
                .Returns(15);

            var diceRoller = new DiceRoller(randomMock.Object);

            var result = diceRoller.RollWithDisadvantage(3);

            Assert.Equal(8, result);

            randomMock.Verify(r => r.Next(1, 21), Times.Exactly(2));
            randomMock.VerifyNoOtherCalls();
        }
    }
}