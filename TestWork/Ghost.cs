using System;
using System.Threading;

namespace TestWork
{
    class Ghost : Car
    {
        private bool isEphemeral = false; // Флаг эфемерного состояния

        public Ghost(string name, float maxSpeed, int hp, int attack, float attackSpeed, int maxMana, int manaRegenRate, int manaCost, int abilityCoolDown, int abilityDuration)
            : base(name, maxSpeed, hp, attack, attackSpeed, maxMana, manaRegenRate, manaCost, abilityCoolDown, abilityDuration) 
        {
            AbilityDuration *= 1000;
        }

        public override void UniqueAbility(Car opponent, Race race)
        {
            if (CanPerformAbility()) // Проверяем, достаточно ли маны для активации способности
            {
                Console.WriteLine($"{Name} становится эфемерным и невосприимчивым к атакам!");
                isEphemeral = true;
                base.UniqueAbility(opponent, race);
                Timer timer = new Timer((e) =>
                {
                    isEphemeral = false;
                    Console.WriteLine($"{Name} больше не эфемерный.");
                }, null, AbilityDuration, Timeout.Infinite); // Эффект длится 5 секунд
            }
            else
            {
                Console.WriteLine($"{Name} не может использовать способность.");
            }
        }

        public override void Move(float timeFrame)
        {
            if (!isEphemeral) // Если машина не в эфемерном состоянии, она двигается как обычно
            {
                base.Move(timeFrame);
            }
            else
            {
                // В эфемерном состоянии машина также может двигаться, но без взаимодействия с препятствиями или атаками
                // Это можно дополнительно настроить в зависимости от правил игры
                DistanceCovered += CurrentSpeed * timeFrame;
            }

            // Дополнительная логика для восстановления маны или других аспектов движения может быть добавлена здесь
        }
    }

}
