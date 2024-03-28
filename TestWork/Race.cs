using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TestWork
{
    internal class Race
    {
        public Car Competitor1 { get; set; }
        public Car Competitor2 { get; set; }
        public float TrackLength { get; set; }
        private List<Action> effects = new List<Action>();
        private List<Timer> effectTimers = new List<Timer>();
        
        private int randomEffectCooldown = 10000;

        public Race(Car competitor1, Car competitor2, float trackLength)
        {
            Competitor1 = competitor1;
            Competitor2 = competitor2;
            TrackLength = trackLength;
        }

        public void Start()
        {
            Console.WriteLine("The race has started!");
            
            while (Competitor1.DistanceCovered < TrackLength && Competitor2.DistanceCovered < TrackLength)
            {
                DisplayStatus();
                UpdateRace();
                CheckRandomEvents();
                CheckRandomUniqueAbilities();              
                Thread.Sleep(1000); // Wait for a bit before next frame
            }
            DetermineWinner();
            DisplayStatus();
        }
        public void AddEffect(Action effect, int delayInSeconds)
        {
            Timer timer = null;
            timer = new Timer(_ =>
            {
                effect.Invoke();
                effectTimers.Remove(timer);
                timer.Dispose();
            }, null, delayInSeconds * 1000, Timeout.Infinite);

            effectTimers.Add(timer);
        }


        private void CheckRandomUniqueAbilities()
        {
            Competitor1.UniqueAbility(Competitor2,this);
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
            Random rnd = new Random();
            int chance = rnd.Next(100);
            // Событие для Competitor1
            if (chance < 10) // 10% шанс на замедление
            {
                var affectedCar = new Random().Next(2) == 0 ? Competitor1 : Competitor2;
                affectedCar.CurrentSpeed -=15f; // Замедляем на 15
                Console.WriteLine($"{affectedCar.Name} замедлен!");

                System.Timers.Timer restoreSpeedTimer = new System.Timers.Timer(randomEffectCooldown)
                {
                    AutoReset = false, // Указываем, что таймер не должен перезапускаться автоматически
                    Enabled = true     // Включаем таймер сразу
                };

                // Подписываемся на событие Elapsed таймера
                restoreSpeedTimer.Elapsed += (sender, e) =>
                {
                    affectedCar.CurrentSpeed += 15f; // Восстанавливаем исходную скорость машины
                    Console.WriteLine($"{affectedCar.Name} восстанавливает свою скорость.");
                    restoreSpeedTimer.Dispose(); // Освобождаем ресурсы таймера
                };
            }
            else if(chance < 20)
            {
                var affectedCar = new Random().Next(2) == 0 ? Competitor1 : Competitor2;
                affectedCar.CurrentSpeed += 10f; // ускоряем на 10%
                Console.WriteLine($"{affectedCar.Name} ускорен!");

                System.Timers.Timer restoreSpeedTimer = new System.Timers.Timer(randomEffectCooldown)
                {
                    AutoReset = false, // Указываем, что таймер не должен перезапускаться автоматически
                    Enabled = true     // Включаем таймер сразу
                };

                // Подписываемся на событие Elapsed таймера
                restoreSpeedTimer.Elapsed += (sender, e) =>
                {
                    affectedCar.CurrentSpeed -= 10f; // Восстанавливаем исходную скорость машины
                    Console.WriteLine($"{affectedCar.Name} восстанавливает свою скорость.");
                    restoreSpeedTimer.Dispose(); // Освобождаем ресурсы таймера
                };
            }            
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
            Console.WriteLine($"The winner is {winner.Name}!");
        }
    }
}
