namespace TestWork
{
    class Ghost : Car
    {
        private bool isEphemeral = false; // Флаг эфемерного состояния

        public Ghost(string name, float maxSpeed, int hp, int attack, float attackSpeed, int maxMana, int manaRegenRate, int manaCost, int abilityCoolDown, int abilityDuration)
            : base(name, maxSpeed, hp, attack, attackSpeed, maxMana, manaRegenRate, manaCost, abilityCoolDown, abilityDuration) 
        {
        }

        public override void UniqueAbility(Car opponent, Race race)
        {
            if (CanPerformAbility() && !isEphemeral) 
            {
                Console.WriteLine($"{Name} становится эфемерным и невосприимчивым к атакам!");
                isEphemeral = true;
                base.UniqueAbility(opponent, race);
                race.AddEffect(() => RestoreEphermal(), AbilityDuration);
            }
            else
            {
                Console.WriteLine($"{Name} не может использовать способность");
            }
        }

        public void RestoreEphermal()
        {
            isEphemeral = false;
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
                DistanceCovered += MaxSpeed * timeFrame;
                Console.WriteLine($"{Name} неуязвим!");
            }
        }
    }
}
