namespace TestWork
{
    internal class Race
    {
        public Car Competitor1 { get; set; }
        public Car Competitor2 { get; set; }
        public float TrackLength { get; set; }
        private List<System.Timers.Timer> effectTimers = new List<System.Timers.Timer>();

        private int randomEffectCooldown = 10000;
        private int slowAmount = 15;
        private int speedIncreaseAmount = 10;

        public Race(Car competitor1, Car competitor2, float trackLength)
        {
            Competitor1 = competitor1;
            Competitor2 = competitor2;
            TrackLength = trackLength;
        }

        public void Start()
        {
            Console.WriteLine("Поехали!");

            while (Competitor1.DistanceCovered < TrackLength && Competitor2.DistanceCovered < TrackLength)
            {
                DisplayStatus();
                UpdateRace();
                CheckRandomEvents();
                CheckUniqueAbilities();
                Thread.Sleep(1000); // таймер перед следующим кадром
            }
            DetermineWinner();
            DisplayStatus();
        }

        public void AddEffect(Action effect, int delayInSeconds)
        {
            System.Timers.Timer timer = new System.Timers.Timer(delayInSeconds * 1000);          
            effectTimers.Add(timer);

            timer.Elapsed += (sender, e) =>
            {
                effect.Invoke(); // Вызываем эффект

                // Удаляем таймер из списка, только если он там есть
                if (effectTimers.Contains(timer))
                {
                    effectTimers.Remove(timer);
                }

                timer.Dispose(); // Освобождаем ресурсы таймера
            };

            timer.AutoReset = false; // Указываем, что таймер не должен перезапускаться автоматически
            timer.Enabled = true; // Включаем таймер
        }


        private void CheckUniqueAbilities()
        {
            Competitor1.UniqueAbility(Competitor2, this);
            Competitor2.UniqueAbility(Competitor1, this);
        }

        private void UpdateRace()
        {
            Competitor1.Move(0.1f); // Двигаем машины
            Competitor2.Move(0.1f);

            // Проверяем возможность атаки и атакуем, если возможно
            if (Competitor1.CanAttack())
            {
                Competitor1.AttackTarget(Competitor2);
            }
            if (Competitor2.CanAttack())
            {
                Competitor2.AttackTarget(Competitor1);
            }
        }

        private void CheckRandomEvents()
        {
            int chance = new Random().Next(100);
            Car affectedCar = new Random().Next(2) == 0 ? Competitor1 : Competitor2;
            if (chance < 10) // 10% шанс на замедление
            {
                ChangeCarSpeed(affectedCar, -slowAmount);
                Console.WriteLine($"{affectedCar.Name} ускорен!");
            }
            else if (chance < 20)
            {
                ChangeCarSpeed(affectedCar, speedIncreaseAmount);
                Console.WriteLine($"{affectedCar.Name} ускорен!");
            }
        }

        private void ChangeCarSpeed(Car car, int speedChange)
        {
            car.CurrentSpeed += slowAmount; // Замедляем

            System.Timers.Timer restoreSpeedTimer = new System.Timers.Timer(randomEffectCooldown)
            {
                AutoReset = false,
                Enabled = true
            };

            // Подписываемся на событие Elapsed таймера
            restoreSpeedTimer.Elapsed += (sender, e) =>
            {
                car.CurrentSpeed -= slowAmount; // Восстанавливаем исходную скорость машины
                Console.WriteLine($"{car.Name} восстанавливает свою скорость.");
                restoreSpeedTimer.Dispose();
            };
        }

        private void DisplayStatus()
        {
            Console.Clear();
            Console.WriteLine(Competitor1.ToString());
            Console.WriteLine(Competitor2.ToString());
        }

        private void DetermineWinner()
        {
            var winner = Competitor1.DistanceCovered >= TrackLength ? Competitor1 : Competitor2;
            Console.WriteLine($"Победитель {winner.Name}!");
        }
    }
}
