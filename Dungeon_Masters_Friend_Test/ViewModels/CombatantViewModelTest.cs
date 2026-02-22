using Dungeon_Masters_Friend.Models;
using Dungeon_Masters_Friend.ViewModels;

namespace Dungeon_Masters_Friend_Test.ViewModels
{
    public class CombatantViewModelTest
    {
        [Fact]
        public void Heal()
        {
            var entity = new Entity()
            {
                MaxHp = 10
            };
            var combatantVm = new CombatantViewModel(entity)
            {
                CurrentHp = 5
            };

            combatantVm.Heal(3);

            Assert.Equal(8, combatantVm.CurrentHp);
        }

        [Fact]
        public void Heal_PastMaxHp()
        {
            var entity = new Entity()
            {
                MaxHp = 10
            };
            var combatantVm = new CombatantViewModel(entity)
            {
                CurrentHp = 5
            };

            combatantVm.Heal(8);

            Assert.Equal(10, combatantVm.CurrentHp);
        }

        [Fact]
        public void Damage()
        {
            var entity = new Entity()
            {
                MaxHp = 10
            };
            var combatantVm = new CombatantViewModel(entity);

            combatantVm.Damage(3);

            Assert.Equal(7, combatantVm.CurrentHp);
        }

        [Fact]
        public void Damage_PastZero()
        {
            var entity = new Entity()
            {
                MaxHp = 10
            };
            var combatantVm = new CombatantViewModel(entity);

            combatantVm.Damage(13);

            Assert.Equal(0, combatantVm.CurrentHp);
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
