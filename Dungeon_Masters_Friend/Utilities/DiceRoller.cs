using System;

namespace Dungeon_Masters_Friend.Utilities
{
    public interface IDiceRoller
    {
        int RollDice(int numberOfDice, int sidesPerDie, int bonus);

        int RollDice(int numberOfDice, int sidesPerDie);

        int RollWithAdvantage(int bonus);

        int RollWithDisadvantage(int bonus);
    }

    /// <summary>
    /// Utility emulating rolling dice.
    /// </summary>
    /// <param name="_random">The Random instance to use for RNG</param>
    public class DiceRoller(Random _random) : IDiceRoller
    {
        /// <summary>
        /// Default constructor that uses a new Random instance for RNG.
        /// </summary>
        public DiceRoller() : this(new()) { }

        /// <summary>
        /// Generates and sums numberOfDice random numbers ranging from 1 to sidesPerDie and adds bonus.
        /// </summary>
        /// <param name="numberOfDice">The number of dice to roll</param>
        /// <param name="sidesPerDie">The maximum value of the dice</param>
        /// <param name="bonus">The extra term added to the summed dice result</param>
        /// <returns>The sum of the rolled dice plus the bonus</returns>
        /// <exception cref="ArgumentException"></exception>
        public int RollDice(int numberOfDice, int sidesPerDie, int bonus)
        {
            if (numberOfDice <= 0 || sidesPerDie <= 0)
                throw new ArgumentException("Number of dice and sides per die must be positive integers");
            int total = 0;
            for (int i = 0; i < numberOfDice; i++)
            {
                total += _random.Next(1, sidesPerDie + 1);
            }
            return total + bonus;
        }

        /// <summary>
        /// Generates and sums numberOfDice random numbers ranging from 1 to sidesPerDie and adds bonus.
        /// </summary>
        /// <param name="numberOfDice">The number of dice to roll</param>
        /// <param name="sidesPerDie">The maximum value of the dice</param>
        /// <returns>The sum of the rolled dice</returns>
        public int RollDice(int numberOfDice, int sidesPerDie)
        {
            return RollDice(numberOfDice, sidesPerDie, 0);
        }

        /// <summary>
        /// Rolls two twenty-sided dice and returns the higher value plus the bonus.
        /// </summary>
        /// <param name="bonus">The amount to add to the rolled dice result</param>
        /// <returns>The higher dice result plus the bonus</returns>
        public int RollWithAdvantage(int bonus)
        {
            return Math.Max(RollDice(1, 20, bonus), RollDice(1, 20, bonus));
        }

        /// <summary>
        /// Rolls two twenty-sided dice and returns the lower value plus the bonus.
        /// </summary>
        /// <param name="bonus">The amount to add to the rolled dice result</param>
        /// <returns>The lower dice result plus the bonus</returns>
        public int RollWithDisadvantage(int bonus)
        {
            return Math.Min(RollDice(1, 20, bonus), RollDice(1, 20, bonus));
        }
    }
}
