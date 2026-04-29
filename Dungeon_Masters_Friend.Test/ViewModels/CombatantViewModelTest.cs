using Dungeon_Masters_Friend.Models;
using Dungeon_Masters_Friend.ViewModels;

namespace Dungeon_Masters_Friend.Test.ViewModels
{
    public class CombatantViewModelTest
    {
        [Fact]
        public void Heal()
        {
            var creature = new Creature()
            {
                MaxHp = 10
            };
            var combatantVm = new CombatantViewModel(creature)
            {
                CurrentHp = 5,
                HPAmount = 3
            };

            combatantVm.HealCommand.Execute(null);

            Assert.Equal(8, combatantVm.CurrentHp);
            Assert.Null(combatantVm.HPAmount);
        }

        [Fact]
        public void Heal_PastMaxHp()
        {
            var creature = new Creature()
            {
                MaxHp = 10
            };
            var combatantVm = new CombatantViewModel(creature)
            {
                CurrentHp = 5,
                HPAmount = 8
            };

            combatantVm.HealCommand.Execute(null);

            Assert.Equal(10, combatantVm.CurrentHp);
            Assert.Null(combatantVm.HPAmount);
        }

        [Fact]
        public void Heal_NullHpAmount()
        {
            var creature = new Creature()
            {
                MaxHp = 10
            };
            var combatantVm = new CombatantViewModel(creature)
            {
                CurrentHp = 5
            };

            combatantVm.HealCommand.Execute(null);

            Assert.Equal(5, combatantVm.CurrentHp);
            Assert.Null(combatantVm.HPAmount);
        }

        [Fact]
        public void Damage()
        {
            var creature = new Creature()
            {
                MaxHp = 10
            };
            var combatantVm = new CombatantViewModel(creature)
            {
                HPAmount = 3
            };

            combatantVm.DamageCommand.Execute(null);

            Assert.Equal(7, combatantVm.CurrentHp);
            Assert.Null(combatantVm.HPAmount);
        }

        [Fact]
        public void Damage_PastZero()
        {
            var creature = new Creature()
            {
                MaxHp = 10
            };
            var combatantVm = new CombatantViewModel(creature)
            {
                HPAmount = 13
            };

            combatantVm.DamageCommand.Execute(null);

            Assert.Equal(0, combatantVm.CurrentHp);
            Assert.Null(combatantVm.HPAmount);
        }

        [Fact]
        public void Damage_NullHpAmount()
        {
            var creature = new Creature()
            {
                MaxHp = 10
            };
            var combatantVm = new CombatantViewModel(creature);

            combatantVm.DamageCommand.Execute(null);

            Assert.Equal(10, combatantVm.CurrentHp);
            Assert.Null(combatantVm.HPAmount);
        }

        [Fact]
        public void AddStatus()
        {
            var combatantVm = new CombatantViewModel();

            combatantVm.AddStatus("status1");
            combatantVm.AddStatus("status2");

            Assert.Equal(["status1", "status2"], combatantVm.Statuses);
        }
    }
}
