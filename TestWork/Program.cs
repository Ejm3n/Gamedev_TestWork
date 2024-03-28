namespace TestWork
{
    class Program
    {
        static List<Car> availableCars = new List<Car>
    {
        new Miner("Miner", 100, 100, 5, 5, 100, 10, 30,6,3),
        new Ghost("Ghost", 120, 80, 4, 10, 100, 10, 40, 12,4),
        new Kabbalist("Kabbalist",95,95,3,6,100,15,35,8,0),
        new Jet("Jet",115,85,6,5,90,12,30,5,6),
        new Healer("Healer",110,70,5,5,70,10,25,10,0)
    };

        static void Main(string[] args)
        {
            Console.WriteLine("Добро пожаловать на гонки!");
            Car competitor1 = ChooseCar("Выберите машину 1:");
            Car competitor2 = ChooseCar("Выберете машину 2:");

            Race race = new Race(competitor1, competitor2, 1000);
            race.Start();
        }

        static Car ChooseCar(string prompt)
        {
            Console.WriteLine(prompt);
            for (int i = 0; i < availableCars.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {availableCars[i].Name}");
            }

            Car selectedCar = null;
            while (selectedCar == null)
            {
                Console.Write("Введите номер машины: ");
                if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= availableCars.Count)
                {
                    // Создаем новый экземпляр выбранной машины
                    var carPrototype = availableCars[choice - 1];
                    selectedCar = (Car)Activator.CreateInstance(carPrototype.GetType(), carPrototype.Name, carPrototype.MaxSpeed, carPrototype.HP, carPrototype.Attack, carPrototype.AttackSpeed, carPrototype.MaxMana, carPrototype.ManaRegenerationRate, carPrototype.ManaCost, carPrototype.AbilityCoolDown, carPrototype.AbilityDuration);
                }
                else
                {
                    Console.WriteLine("Что-то пошло не так. Попробуйте снова.");
                }
            }
            return selectedCar;
        }
    }
}