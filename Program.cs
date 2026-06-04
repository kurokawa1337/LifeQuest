using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace LifeQuest
{
    class Program
    {
        static List<TaskItem> tasks = new List<TaskItem>();
        static UserProfile player = new UserProfile();

        static readonly string tasksFile = "tasks.json";
        static readonly string playerFile = "player.json";

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            LoadData();

            while (true)
            {
                Console.Clear();
                PrintHeader();
                PrintMenu();

                Console.Write("\nОбери дію (0-9): ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ViewTasks(); break;
                    case "2": AddTask(); break;
                    case "3": CompleteTask(); break;
                    case "4": DeleteTask(); break;
                    case "5": ShowInstructions(); break;
                    case "6": PrestigeMenu(); break;
                    case "7": HardReset(); break;
                    case "9": DevMode(); break;
                    case "0":
                        SaveData();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("Дані збережено. До зустрічі в LifeQuest!");
                        Console.ResetColor();
                        return;
                    default:
                        PrintError("Невідома команда. Спробуй ще раз.");
                        break;
                }
            }
        }

        static void PrintHeader()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==================================================");
            Console.WriteLine("                 L I F E Q U E S T                ");
            Console.WriteLine("==================================================");

            Console.ForegroundColor = ConsoleColor.Yellow;
            string prestigeText = player.PrestigeLevel > 0 ? $"[Престиж: {player.PrestigeLevel} | Множник XP: x{player.XPMultiplier}]" : "";
            Console.WriteLine($" Звання: {player.Rank} {prestigeText}");

            int percent = (int)((double)player.CurrentXP / player.XPToNextLevel * 10);
            string bar = new string('█', percent) + new string('-', 10 - percent);

            if (player.Level >= 100)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($" Рівень: MAX (100)  |  Досвід: MAX  |  [██████████]");
                Console.WriteLine(" Час зробити Переродження (Пункт 6)!");
            }
            else
            {
                Console.WriteLine($" Рівень: {player.Level}  |  Досвід: {player.CurrentXP}/{player.XPToNextLevel} XP  |  [{bar}]");
            }

            Console.ResetColor();
            Console.WriteLine("==================================================\n");
        }

        static void PrintMenu()
        {
            Console.WriteLine("1. [📖] Переглянути всі квести");
            Console.WriteLine("2. [➕] Додати новий квест");
            Console.WriteLine("3. [✔] Виконати квест");
            Console.WriteLine("4. [❌] Видалити квест");
            Console.WriteLine("5. [ℹ️] Інструкція (Як грати)");

            if (player.Level >= 100) Console.ForegroundColor = ConsoleColor.Magenta;
            else Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("6. [🌟] Переродження (Престиж) - Доступно з 100 рівня");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("7. [🔥] Видалити всі дані (Hard Reset)");
            Console.ResetColor();

            Console.WriteLine("9. [🛠] Режим розробника (Чіти для тесту)");
            Console.WriteLine("0. [🚪] Вихід та збереження");
        }

        static void ShowInstructions()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("--- ІНСТРУКЦІЯ ТА МЕХАНІКИ LifeQuest ---\n");
            Console.ResetColor();

            Console.WriteLine("🎯 ОСНОВНА ІДЕЯ:");
            Console.WriteLine(" LifeQuest перетворює твої нудні повсякденні справи на RPG-гру.");
            Console.WriteLine(" Додавай реальні завдання (квести), виконуй їх і отримуй Досвід (XP).\n");

            Console.WriteLine("🏆 ЗВАННЯ ТА РІВНІ:");
            Console.WriteLine(" Кожні 10 рівнів ти отримуєш нове звання (Новачок -> Воїн -> Легенда).\n");

            Console.WriteLine("🌟 ПЕРЕРОДЖЕННЯ (ПРЕСТИЖ):");
            Console.WriteLine(" Коли ти досягнеш 100-го рівня, ти не зможеш отримувати XP далі.");
            Console.WriteLine(" Тобі відкриється функція 'Переродження' (Престиж).");
            Console.WriteLine(" Скинувши свій рівень до 1-го, ти отримаєш множник досвіду (х2, х3...).");
            Console.WriteLine(" Це дозволить прокачуватись набагато швидше!\n");

            WaitForKey();
        }

        static void PrestigeMenu()
        {
            Console.Clear();
            if (player.Level < 100)
            {
                PrintError("Ти ще не готовий! Потрібен 100 рівень.");
                return;
            }

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("=== ВІВТАР ПЕРЕРОДЖЕННЯ ===");
            Console.WriteLine("Ти досяг вершини. Але це ще не кінець.");
            Console.WriteLine("Ти можеш скинути свій рівень до 1, але натомість отримаєш:");
            Console.WriteLine($"-> Назавжди збільшений множник досвіду: x{player.XPMultiplier + 1}");
            Console.ResetColor();

            Console.Write("\nЗробити Переродження? (Так - 'y', Ні - 'n'): ");
            if (Console.ReadLine().ToLower() == "y")
            {
                player.DoPrestige();
                SaveData();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n[МАГІЯ] Ти переродився! Твій прогрес скинуто, але ти став сильнішим.");
                Console.ResetColor();
            }
            WaitForKey();
        }

        static void HardReset()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("=== ⚠️ НЕБЕЗПЕЧНА ЗОНА: ПОВНЕ СКИДАННЯ ⚠️ ===");
            Console.WriteLine("Ця дія видалить УСІ твої квести, рівні, досвід та престиж.");
            Console.WriteLine("Відновити дані буде НЕМОЖЛИВО!");
            Console.ResetColor();

            Console.Write("\nЯкщо ти дійсно хочеш це зробити, введи слово 'RESET' (великими літерами): ");
            string confirmation = Console.ReadLine();

            if (confirmation == "RESET")
            {
                tasks.Clear();
                player = new UserProfile();

                if (File.Exists(tasksFile)) File.Delete(tasksFile);
                if (File.Exists(playerFile)) File.Delete(playerFile);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n[ПОРОЖНЕЧА] Всі дані успішно видалено. Життя починається з чистого аркуша.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nСкидання скасовано. Твій прогрес у безпеці.");
                Console.ResetColor();
            }
            WaitForKey();
        }

        static void DevMode()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("--- РЕЖИМ РОЗРОБНИКА (ЧІТИ) ---\n");
            Console.ResetColor();

            Console.WriteLine("1. Додати +1000 XP");
            Console.WriteLine("2. Встановити одразу 100 рівень (Для тесту Престижу)");
            Console.WriteLine("3. Скасувати");

            Console.Write("\nВибір: ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                player.AddXP(1000 / player.XPMultiplier);
                Console.WriteLine("Додано 1000 XP!");
            }
            else if (choice == "2")
            {
                player.Level = 100;
                player.CurrentXP = player.XPToNextLevel;
                Console.WriteLine("Ти отримав 100 рівень!");
            }

            SaveData();
            WaitForKey();
        }

        static void ViewTasks()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("--- ТВОЇ АКТИВНІ КВЕСТИ ---\n");
            Console.ResetColor();

            if (tasks.Count == 0)
            {
                Console.WriteLine("Список квестів порожній. Час додати нові виклики!");
            }
            else
            {
                for (int i = 0; i < tasks.Count; i++)
                {
                    var task = tasks[i];
                    string status = task.IsCompleted ? "[✔]" : "[ ]";

                    if (task.IsCompleted) Console.ForegroundColor = ConsoleColor.DarkGray;
                    else if (task.Difficulty == "Складне") Console.ForegroundColor = ConsoleColor.Red;
                    else if (task.Difficulty == "Середнє") Console.ForegroundColor = ConsoleColor.Yellow;
                    else Console.ForegroundColor = ConsoleColor.White;

                    Console.WriteLine($"{i + 1}. {status} {task.Title} ({task.Difficulty} | +{task.RewardXP * player.XPMultiplier} XP)");
                    Console.ResetColor();
                }
            }
            WaitForKey();
        }

        static void AddTask()
        {
            Console.Write("\nВведи назву квесту: ");
            string title = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(title))
            {
                PrintError("Назва не може бути порожньою!");
                return;
            }

            Console.WriteLine("Обери складність:");
            Console.WriteLine("1. Легке (Базово +20 XP)");
            Console.WriteLine("2. Середнє (Базово +50 XP)");
            Console.WriteLine("3. Складне (Базово +100 XP)");
            Console.Write("Твій вибір: ");
            string diffChoice = Console.ReadLine();

            string difficulty = "Легке";
            int xp = 20;

            if (diffChoice == "2") { difficulty = "Середнє"; xp = 50; }
            else if (diffChoice == "3") { difficulty = "Складне"; xp = 100; }

            tasks.Add(new TaskItem { Title = title, Difficulty = difficulty, RewardXP = xp, IsCompleted = false });
            SaveData();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n[+] Новий квест успішно додано!");
            Console.ResetColor();
            WaitForKey();
        }

        static void CompleteTask()
        {
            ViewTasks();
            if (tasks.Count == 0) return;

            Console.Write("\nВведи номер квесту, який ти виконав: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= tasks.Count)
            {
                var task = tasks[index - 1];

                if (task.IsCompleted)
                {
                    PrintError("Цей квест вже виконано!");
                }
                else
                {
                    task.IsCompleted = true;
                    int earnedXP = task.RewardXP * player.XPMultiplier;

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n[КВЕСТ ВИКОНАНО] Ти отримав +{earnedXP} XP!");
                    Console.ResetColor();

                    player.AddXP(task.RewardXP);
                    SaveData();
                }
            }
            else
            {
                PrintError("Невірний номер квесту!");
            }
            WaitForKey();
        }

        static void DeleteTask()
        {
            ViewTasks();
            if (tasks.Count == 0) return;

            Console.Write("\nВведи номер квесту для видалення: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= tasks.Count)
            {
                tasks.RemoveAt(index - 1);
                SaveData();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n[-] Квест видалено.");
                Console.ResetColor();
            }
            else
            {
                PrintError("Невірний номер!");
            }
            WaitForKey();
        }

        static void SaveData()
        {
            File.WriteAllText(tasksFile, JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true }));
            File.WriteAllText(playerFile, JsonSerializer.Serialize(player, new JsonSerializerOptions { WriteIndented = true }));
        }

        static void LoadData()
        {
            if (File.Exists(tasksFile))
            {
                string json = File.ReadAllText(tasksFile);
                tasks = JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
            }

            if (File.Exists(playerFile))
            {
                string json = File.ReadAllText(playerFile);
                player = JsonSerializer.Deserialize<UserProfile>(json) ?? new UserProfile();
            }
        }

        static void PrintError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n[ПОМИЛКА] {message}");
            Console.ResetColor();
            WaitForKey();
        }

        static void WaitForKey()
        {
            Console.WriteLine("\nНатисни Enter для продовження...");
            Console.ReadLine();
        }
    }
}