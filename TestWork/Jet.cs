namespace TestWork
{
    class Jet : Car
    {
        private int speedBoost = 10;
        public Jet(string name, float maxSpeed, int hp, int attack, float attackSpeed, int maxMana, int manaRegenRate, int manaCost, int abilityCoolDown, int abilityDuration)
            : base(name, maxSpeed, hp, attack, attackSpeed, maxMana, manaRegenRate, manaCost, abilityCoolDown, abilityDuration){}

        public override void UniqueAbility(Car opponent, Race race)
        {
            if (CanPerformAbility())
            {
                Console.WriteLine($"{Name} увеличивает свою мощность и максимальную скорость.");               
                CurrentSpeed += speedBoost;
                base.UniqueAbility(opponent, race);
                race.AddEffect(() => CurrentSpeed -= speedBoost, AbilityDuration);
            }
            else
            {
                Console.WriteLine($"{Name} не может использовать способность");
            }
        }
    }

}
