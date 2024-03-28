using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWork
{
    class Kabbalist : Car
    {
        private static readonly Random random = new Random();
        private int selfDamage = 5;
        private int abilityDamage = 15;
        private int abilityDuration = 5;
        public Kabbalist(string name, float maxSpeed, int hp, int attack, float attackSpeed, int maxMana, int manaRegenRate, int manaCost, int abilityCoolDown, int abilityDuration)
            : base(name, maxSpeed, hp, attack, attackSpeed, maxMana, manaRegenRate, manaCost, abilityCoolDown, abilityDuration){}

        public override void UniqueAbility(Car opponent, Race race)
        {
            if(CanPerformAbility())
            {
                int chance = random.Next(100);
                base.UniqueAbility(opponent, race);
                // Шанс самоповреждения
                if (chance < 10) // 10% вероятность самоповреждения
                {
                    HP -= selfDamage; // Уменьшение своего HP
                    Console.WriteLine($"{Name} демон случайно наносит себе {selfDamage} урона.");
                }
                else if (chance < 55)
                {
                    Console.WriteLine($"{Name} блокирует способности {opponent.Name}.");
                    opponent.BlockAbility(abilityDuration); // Блокировка на n секунд
                }
                else
                {
                    opponent.HP -= abilityDamage;
                    Console.WriteLine($"{Name} демон наносит {abilityDamage} урона {opponent.Name}.");
                }
            }          
        }
    }

}
