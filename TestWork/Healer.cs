namespace TestWork
{
    class Healer : Car
    {
        public int HealingAmount = 10; // Количество восстанавливаемого здоровья
        private int MaxHP;

        public Healer(string name, float maxSpeed, int hp, int attack, float attackSpeed, int maxMana, int manaRegenRate, int manaCost, int abilityCoolDown,int abilityDuration)
            : base(name, maxSpeed, hp, attack, attackSpeed, maxMana, manaRegenRate, manaCost, abilityCoolDown, abilityDuration)
        {
            MaxHP = hp;
        }

        public override void UniqueAbility(Car opponent, Race race)
        {
            if (CanPerformAbility() && HP<MaxHP)
            {
                HP += HealingAmount;
                base.UniqueAbility(opponent, race);
                Console.WriteLine($"{Name} использует способность и восстанавливает {HealingAmount} здоровья.");
                if(HP>MaxHP)
                    HP = MaxHP;
            }
            else
            {
                Console.WriteLine($"{Name} не может использовать способность");
            }
        }
    }

}
