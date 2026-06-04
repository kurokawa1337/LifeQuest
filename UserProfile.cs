using System;

namespace LifeQuest
{
    public class UserProfile
    {
        public int Level { get; set; } = 1;
        public int CurrentXP { get; set; } = 0;
        public int PrestigeLevel { get; set; } = 0;

        public int XPMultiplier => 1 + PrestigeLevel;
        public int XPToNextLevel => Level * 100;

        public string Rank
        {
            get
            {
                if (Level < 10) return "Новачок";
                if (Level < 20) return "Учень";
                if (Level < 30) return "Шукач пригод";
                if (Level < 40) return "Воїн";
                if (Level < 50) return "Ветеран";
                if (Level < 60) return "Майстер";
                if (Level < 70) return "Герой";
                if (Level < 80) return "Елітний Герой";
                if (Level < 90) return "Чемпіон";
                if (Level < 100) return "Напівбог";
                return "Легенда";
            }
        }

        public void AddXP(int baseAmount)
        {
            if (Level >= 100) return;

            int finalAmount = baseAmount * XPMultiplier;
            CurrentXP += finalAmount;

            while (CurrentXP >= XPToNextLevel && Level < 100)
            {
                CurrentXP -= XPToNextLevel;
                Level++;

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"\n[LEVEL UP!] Вітаємо! Ти досяг {Level} рівня! Твоє звання: {Rank}");
                Console.ResetColor();
            }

            if (Level >= 100)
            {
                Level = 100;
                CurrentXP = XPToNextLevel;
            }
        }

        public void DoPrestige()
        {
            if (Level >= 100)
            {
                PrestigeLevel++;
                Level = 1;
                CurrentXP = 0;
            }
        }
    }
}