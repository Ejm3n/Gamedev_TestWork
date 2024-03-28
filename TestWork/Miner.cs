namespace TestWork
{
    class Miner : Car
    {
        private float opponentSlowDownMultiplier= .5f;
        public Miner(string name, float maxSpeed, int hp, int attack, float attackSpeed, int maxMana, int manaRegenRate, int manaCost,int abilityCoolDown, int abilityDuration)
            :base(name,  maxSpeed,  hp,  attack,  attackSpeed,  maxMana,  manaRegenRate, manaCost, abilityCoolDown, abilityDuration) {}

        public override void UniqueAbility(Car opponent, Race race)
        {
            if (CanPerformAbility() && DistanceCovered > opponent.DistanceCovered)
            {
                Console.WriteLine($"{Name} устанавливает мины, замедляя {opponent.Name}!");
                base.UniqueAbility(opponent, race);
                opponent.CurrentSpeed *= opponentSlowDownMultiplier;
                race.AddEffect(() => opponent.CurrentSpeed = opponent.MaxSpeed, AbilityDuration); // Восстановление скорости
            }
            else
            {
                Console.WriteLine($"{Name} не может использовать уникальную способность.");
            }
        }
    }

}
