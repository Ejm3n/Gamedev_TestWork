using System;

namespace TestWork
{

    abstract class Car
    {
        public string Name { get; set; }
        public float MaxSpeed { get; set; }
        public float CurrentSpeed { get; set; }

        public int HP { get; set; }
        public int Attack { get; set; }
        public float AttackSpeed { get; set; }

        private DateTime lastAttackTime;
        public float DistanceCovered { get; set; }

        public int Mana { get; set; } // Текущее значение маны
        public int MaxMana { get; set; } // Максимальное значение маны
        public int ManaRegenerationRate { get; set; } // Величина восстановления маны в единицу времени
        public int ManaCost { get; set; }
        public int AbilityCoolDown { get; set; }
        public int AbilityDuration { get; set; }
        public bool IsAbilityBlocked { get; private set; }

        protected DateTime lastAbilityTime;

        protected Car(string name, float maxSpeed, int hp, int attack, float attackSpeed, int maxMana, int manaRegenRate,  int manaCost, int abilityCoolDown, int abilityDuration)
        {
            Name = name;
            MaxSpeed = maxSpeed;
            CurrentSpeed = maxSpeed;
            HP = hp;
            Attack = attack;
            AttackSpeed = attackSpeed;
            lastAttackTime = DateTime.MinValue;
            lastAbilityTime = DateTime.MinValue;
            DistanceCovered = 0f;

            MaxMana = maxMana;
            Mana = MaxMana;
            ManaRegenerationRate = manaRegenRate;
            ManaCost = manaCost;
            AbilityCoolDown = abilityCoolDown;
            AbilityDuration = abilityDuration;
        }

        public virtual void UniqueAbility(Car opponent, Race race)
        {
            lastAbilityTime = DateTime.Now;
            Mana -= ManaCost;
        }

        public virtual void Move(float timeFrame)
        {
            if(HP>0)
            {
                DistanceCovered += CurrentSpeed * timeFrame;
                RegenerateMana(timeFrame);
            }     
            else
            {
                Console.WriteLine($"{Name} уничтожен...");
            }
        }

        public bool IsAlive()
        {
            return HP > 0;
        }

        private void RegenerateMana(float timeFrame)
        {
            Mana = Math.Min(Mana + (int)(ManaRegenerationRate * timeFrame), MaxMana);
        }

        public bool CanAttack()
        {
            return (DateTime.Now - lastAttackTime).TotalSeconds >= AttackSpeed && IsAlive();
        }

        public bool CanPerformAbility()
        {
            return (DateTime.Now - lastAbilityTime).TotalSeconds >= AbilityCoolDown && Mana>ManaCost && !IsAbilityBlocked && IsAlive();
        }
        public void BlockAbility(int durationSeconds)
        {
            if (!IsAbilityBlocked)
            {
                IsAbilityBlocked = true;
                Console.WriteLine($"{Name} не может использовать способности в течение {durationSeconds} секунд.");

                // Устанавливаем таймер, который снимет блокировку через durationSeconds
                var timer = new Timer(_ =>
                {
                    IsAbilityBlocked = false;
                    Console.WriteLine($"{Name} снова может использовать способности.");
                }, null, durationSeconds * 1000, Timeout.Infinite);
            }
        }

        public virtual void AttackTarget(Car target)
        {
            if (CanAttack())
            {
                Random rnd = new Random();
                int randomAttack = rnd.Next(Attack - 5, Attack + 5);
                if(randomAttack<=0)
                {
                    randomAttack = 0;
                    Console.WriteLine($"{Name} промазал!");
                }
                Console.WriteLine($"{Name} атакует {target.Name} на {randomAttack} урона.");
                target.HP -= randomAttack;
                lastAttackTime = DateTime.Now;
            }
        }

        public override string ToString()
        {
            return $"Имя: {Name}, Текущая скорость: {CurrentSpeed} м/с, Здоровье: {HP}, Атака: {Attack}, Скорость атаки: {AttackSpeed} с, Мана: {Mana}/{MaxMana}, Пройденное расстояние: {DistanceCovered:F2} м";
        }
    }
}