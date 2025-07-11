using System;
using System.ComponentModel;

namespace TextRpg
{
    public static class GameLoop
    {
        public static Player player;
        public static void ErrorMessage()
        {
            Console.Clear();
            Console.WriteLine("잘못된 입력입니다");
            Thread.Sleep(1000);
        }

        public static void Start()
        {
            Console.WriteLine("==========TextRPG========");
            Console.WriteLine("당신의 이름을 입력하세요");
            string? PlayerName = Console.ReadLine();
            if (PlayerName != null)
            {
                player = new Player(PlayerName);
            }
            string[] jobs = { "용사", "팔라딘", "개발자" };

            while (true)
            {
                Console.WriteLine("당신의 직업은 무엇인가요?");
                for (int i = 0; i < jobs.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {jobs[i]}");
                }
                string? selectedJob = Console.ReadLine();
                if (int.TryParse(selectedJob, out int jobIndex) && jobIndex > 0 && jobIndex <= jobs.Length)
                {
                    player.Job = jobs[jobIndex - 1];
                    break;
                }
                else
                {
                    Console.WriteLine("잘못된 입력 다시 선택하십시오");
                }
            }
            Console.Clear();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n스파르타 마을에 오신 여러분 환영합니다.");
                Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.\n");
                Console.WriteLine("1. 상태 보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                Console.WriteLine("4. 휴식취하기 (500G)");
                Console.WriteLine("5. 던전 입장하기");
                Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");
                string? input = Console.ReadLine();
                switch (input)
                {
                    case "1":

                        while (true)
                        {
                            Console.Clear();
                            player.ShowStatus();
                            string? userinput = Console.ReadLine();
                            if (userinput != "0")
                            {
                                ErrorMessage();
                            }
                            else
                            {
                                break;
                            }
                        }
                        break;
                    case "2":
                        while (true)
                        {
                            Console.Clear();
                            Inventory.ShowInventory(player);
                            string? userinput = Console.ReadLine();
                            if (userinput == "1")
                            {
                                Console.Clear();
                                Inventory.EquipItems(player);
                            }
                            else if (userinput == "0")
                            {
                                break;
                            }
                            else
                            {
                                ErrorMessage();
                            }
                        }
                        break;
                    case "3":
                        while (true)
                        {
                            Console.Clear();
                            Shop.ShopDisplay(player);
                            string? userinput = Console.ReadLine();
                            if (userinput == "0")
                                break;
                            else if (userinput == "1")
                                Shop.PurchaseItem(player);
                            else if (userinput == "2")
                                Shop.SellItem(player);
                            else
                            {
                                ErrorMessage();
                            }

                        }
                        break;
                    case "4":
                        while (true)
                        {
                            Console.Clear();
                            Rest.Resting(player);
                            string? userinput = Console.ReadLine();
                            if (userinput == "0")
                                break;
                            else if (userinput == "1")
                            {
                                Console.Clear();
                                Console.WriteLine($"휴식취하는 중 (-500G)\n보유 골드 ({player.Gold})");
                                player.CurrentHealth += 100;
                                player.Gold -= 500;
                                if (player.CurrentHealth > player.TotalHealth)
                                {
                                    player.CurrentHealth = player.TotalHealth;
                                }
                                Thread.Sleep(2000);
                                Console.Clear();
                                Console.WriteLine($"휴식 완료했씁니다 현재 체력 {player.Health}");
                                Thread.Sleep(2000);
                                break;
                            }
                            else
                            {
                                ErrorMessage();
                            }
                        }
                        break;
                    case "5":
                        while (true)
                        {
                            Console.Clear();
                            Dungeon.ShowDungeonEntrance();
                            string? userinput = Console.ReadLine();
                            if (userinput == "0")
                            {
                                break;
                            }
                            else if (userinput == "1")
                            {
                                Dungeon.EnterDungeon(1, player);
                            }
                            else if (userinput == "2")
                            {
                                Dungeon.EnterDungeon(2, player);
                            }
                            else if (userinput == "3")
                            {
                                Dungeon.EnterDungeon(3, player);
                            }
                        }
                        break;
                    default:
                        Console.Clear();
                        ErrorMessage();
                        Thread.Sleep(1000);
                        break;
                }
            }
        }
    }
}