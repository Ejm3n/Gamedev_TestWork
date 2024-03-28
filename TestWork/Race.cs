using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
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
            Console.WriteLine("The race has started!");
            
            while (Competitor1.DistanceCovered < TrackLength && Competitor2.DistanceCovered < TrackLength)
            {
                DisplayStatus();
                UpdateRace();
                CheckRandomEvents();
                CheckRandomUniqueAbilities();              
                Thread.Sleep(1000); // таймер перед следующим кадром
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
            Car affectedCar = new Random().Next(2) == 0 ? Competitor1 : Competitor2;
            // Событие для Competitor1
            if (chance < 10) // 10% шанс на замедление
            {          
                ChangeCarSpeed(affectedCar, -slowAmount);
                Console.WriteLine($"{affectedCar.Name} ускорен!");
            }
            else if(chance < 20)
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
            Console.WriteLine($"The winner is {winner.Name}!");
        }
    }
}
