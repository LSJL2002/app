using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Formats.Asn1;
using System.Globalization;
using System.Numerics;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualBasic;

namespace TextRpg
{
    class Program
    {
        public static void Main(string[] args)
        {
            GameLoop.Start();
        }
        public class Player //Player Info
        {
            public string PlayerName { get; set; }
            public int Level { get; set; } = 1;
            public string Job { get; set; }
            public int Attack { get; set; } = 10;
            public int Defense { get; set; } = 5;
            public int Health { get; set; } = 100;
            public int CurrentHealth { get; set; }
            public int Speed { get; set; } = 5;
            public int Gold { get; set; } = 1500;
            public List<Item> Inventory { get; set; } = new List<Item>();

            public int TotalAttack
            {
                get
                {
                    int total = Attack;
                    foreach (var item in Inventory)
                    {
                        if (item.Equipped && item.Type == "공격력")
                        {
                            total += item.Value;
                        }
                    }
                    return total;
                }
            }

            public int TotalDefense
            {
                get
                {
                    int total = Defense;
                    foreach (var item in Inventory)
                    {
                        if (item.Equipped && item.Type == "방어력")
                        {
                            total += item.Value;
                        }
                    }
                    return total;
                }
            }

            public int TotalHealth
            {
                get
                {
                    int total = Health;
                    foreach (var item in Inventory)
                    {
                        if (item.Equipped && item.Type == "체력")
                        {
                            total += item.Value;
                        }
                    }
                    return total;
                }
            }

            public int TotalSpeed
            {
                get
                {
                    int total = Speed;
                    foreach (var item in Inventory)
                    {
                        if (item.Equipped && item.Type == "속도")
                        {
                            total += item.Value;
                        }
                    }
                    return total;
                }
            }


            public Player(string name)
            {
                PlayerName = name;
                CurrentHealth = Health; 
            }



            public void ShowStatus()
            {
                int equipAttack = 0;
                int equipDefense = 0;
                int equipHealth = 0;
                int equipSpeed = 0;
                foreach (var item in Inventory)
                {
                    if (!item.Equipped)
                        continue;
                    if (item.Type == "공격력")
                        equipAttack += item.Value;
                    else if (item.Type == "방어력")
                        equipDefense += item.Value;
                    else if (item.Type == "체력")
                        equipHealth += item.Value;
                    else if (item.Type == "속도")
                        equipSpeed += item.Value;
                }

                string attackDisplay = equipAttack > 0 ? $"{Attack + equipAttack} (+{equipAttack})" : $"{Attack}";
                string defenseDisplay = equipDefense > 0 ? $"{Defense + equipDefense} (+{equipDefense})" : $"{Defense}";
                string healthDisplay = equipHealth > 0 ? $"{Health + equipHealth} (+{equipHealth})" : $"{Health}";
                string speedDisplay = equipSpeed > 0 ? $"{Speed + equipSpeed} (+{equipSpeed})" : $"{Speed}";

                Console.WriteLine("\n상태 보기\n캐릭터의 정보가 표시됩니다.\n");
                Console.WriteLine($"Lv. {Level}\n{PlayerName} ( {Job} )");
                Console.WriteLine($"현재 체력 : {CurrentHealth}");
                Console.WriteLine($"체력 (스탯) : {healthDisplay}");
                Console.WriteLine($"공격: {attackDisplay}");
                Console.WriteLine($"방어: {defenseDisplay}");
                Console.WriteLine($"속도: {speedDisplay}");
                Console.WriteLine($"Gold : {Gold} G");
                Console.WriteLine("\n0. 나가기\n");
                Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");
            }
        }
        public class Item //Items that will be both displayed on player and shop
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public int Value { get; set; }
            public string Description { get; set; }
            public int Price { get; set; }
            public bool Equipped { get; set; }
            public bool IsPurchased { get; set; }
            public Item(string name, string type, int value, string description, int price)
            {
                Name = name;
                Type = type;
                Value = value;
                Description = description;
                Price = price;
            }
            public string GetInventoryDisplay()
            {
                string equippedMark = Equipped ? "[E]" : "   ";
                return $"- {equippedMark}{Name} | {Type} +{Value} | {Description}";
            }

            private string PadDisplay(string input, int totalWidth)
            {
                int displayWidth = 0;
                foreach (char c in input)
                {
                    // 한글, 한자 등은 2칸 차지
                    displayWidth += (c >= 0xAC00 && c <= 0xD7A3) ? 2 : 1;
                }

                int padding = Math.Max(0, totalWidth - displayWidth);
                return input + new string(' ', padding);
            }

            public string ShopDisplayPanel()
            {
                string priceDisplay = IsPurchased ? "구매완료" : $"{Price}G";

                string namePadded = PadDisplay(Name, 20);
                string typeValue = PadDisplay($"{Type} +{Value}", 12);
                string descPadded = PadDisplay(Description, 50);
                string pricePadded = PadDisplay(priceDisplay, 10);

                return $"{namePadded}| {typeValue}| {descPadded}| {pricePadded}";
            }
        }

        public class Shop
        {
            public static List<Item> ShopItem = new List<Item>
            {
                new Item ("수련자 갑옷", "방어력",5,"수련에 도움을 주는 갑옷입니다.",1000),
                new Item ("무쇠 갑옷", "방어력", 9,"무쇠로 만들어져 튼튼한 갑옷입니다.", 1500),
                new Item ("스프라타의 갑옷", "방어력", 15,"스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500),
                new Item ("낡은 검", "공격력", 5, "쉽게 볼 수 있는 낡은 검 입니다.", 1000),
                new Item ("청동 도끼", "공격력",10, "어디선가 사용됐던거 같은 도끼입니다.",1500),
                new Item ("스파르타의 창", "공격력",20,"스파르타의 전사들이 사용했다는 전설의 창입니다.", 3500),
                new Item ("머리띠", "체력", 10, "그냥 머리띠입니다.", 1000),
                new Item ("투구 ", "체력", 30, "쇠로 만들어진 투구입니다.",1500),
                new Item ("스프르타의 투구", "체력", 80, "스파르타의 전사들이 사용했다는 전설의 투구입니다.", 3500),
                new Item ("샌들", "속도", 3, "샌들입니다, 그냥 샌들입니다.", 1000),
                new Item ("쇠 부츠", "속도", 9, "쇠로 만들어진 부츠입니다.", 1500),
                new Item ("스파르타의 부츠", "속도", 15, "스파르타의 전사들이 신었다는 전설의 부츠입니다.", 3500),
                new Item ("키보드", "공격", 90, "이게 왜?", 9999)
            };

            public static void ShopDisplay(Player player)
            {
                Console.WriteLine($"[상점]\n필요한 아이템을 얻을 수 있는 상점입니다\n\n[보유골드]\n{player.Gold}G\n");
                Console.WriteLine("[아이템 목록]");
                for (int i = 0; i < ShopItem.Count; i++)
                {
                    var item = ShopItem[i];
                    Console.WriteLine($"- {item.ShopDisplayPanel()}");
                }
                Console.WriteLine("\n1. 아이템 구매\n0. 나가기");
                Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");
            }

            public static void PurchaseItem(Player player)
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine($"상점 - 아이템 구매\n필요한 아이템을 얻을 수 있는 상점입니다.\n\n[보유 골드]\n{player.Gold}G");
                    Console.WriteLine("[아이템 목록]");
                    for (int i = 0; i < ShopItem.Count; i++)
                    {
                        var item = ShopItem[i];
                        Console.WriteLine($"{(i + 1).ToString().PadLeft(2)}. {item.ShopDisplayPanel()}");
                    }
                    Console.WriteLine("\n0. 상점으로 돌아가기");
                    Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");
                    string? input = Console.ReadLine();
                    if (input == "0")
                    {
                        Console.Clear();
                        ShopDisplay(player);
                        break;
                    }
                    if (int.TryParse(input, out int choice) && choice >= 1 && choice <= ShopItem.Count)
                    {
                        var choosenItem = ShopItem[choice - 1];
                        if (player.Gold < choosenItem.Price)
                        {
                            Console.Clear();
                            Console.WriteLine($"{choosenItem.Price - player.Gold}G 부족합니다.");
                            Thread.Sleep(1000);
                        }
                        else if (choosenItem.IsPurchased)
                        {
                            Console.Clear();
                            Console.WriteLine("이미 소지하고 있는 물건입니다!");
                            Thread.Sleep(1000);
                        }
                        else
                        {
                            Console.Clear();
                            player.Gold -= choosenItem.Price;
                            choosenItem.IsPurchased = true;
                            player.Inventory.Add(choosenItem);
                            Console.WriteLine($"{choosenItem.Name}을 구입했습니다!");
                            Thread.Sleep(1000);
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("잘못된 입력입니다");
                        Thread.Sleep(1000);
                    }
                    
                }

            }
        }
        public static class Inventory
        {
            public static void ShowInventory(Player player)
            {
                Console.WriteLine("\n인벤토리\n보유 중인 아이템을 관리할 수 있습니다.\n[아이템 목록]");
                if (player.Inventory.Count == 0)
                {
                    Console.WriteLine("아이템이 없습니다.");
                }
                else
                {
                    foreach (var item in player.Inventory)
                    {
                        Console.WriteLine(item.GetInventoryDisplay());
                    }
                }
                Console.WriteLine("\n1. 장착 관리");
                Console.WriteLine("0. 나가기");
                Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");
            }
            public static void EquipItems(Player player)
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("인벤토리 - 장착 관리\n보유 중인 아이템을 관리할 수 있습니다.\n");
                    Console.WriteLine("[아이템 목록]");
                    for (int i = 0; i < player.Inventory.Count; i++)
                    {
                        var item = player.Inventory[i];
                        Console.WriteLine($"{i + 1}. {item.GetInventoryDisplay()}");
                    }
                    Console.WriteLine("\n\n0. 나가기");
                    Console.WriteLine("원하시는 행동을 입력해주세요.\n>>");
                    string? input = Console.ReadLine();
                    if (input == "0")
                    {
                        Console.Clear();
                        ShowInventory(player);
                        break;
                    }
                    if (int.TryParse(input, out int choice) && choice > 0 && choice <= player.Inventory.Count)
                    {
                        var choosenEquip = player.Inventory[choice - 1];
                        if (choosenEquip.Equipped)
                        {
                            Console.Clear();
                            choosenEquip.Equipped = false;
                            Console.WriteLine($"{choosenEquip.Name}을(를) 장착을 해제했습니다");
                            Thread.Sleep(1000);
                        }
                        else
                        {
                            Console.Clear();
                            choosenEquip.Equipped = true;
                            Console.WriteLine($"{choosenEquip.Name}을(를) 장착했습니다");
                            Thread.Sleep(1000);
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("잘못된 입력입니다");
                        Thread.Sleep(1000);
                    }
                }
            }
        }

        public static class Rest
        {
            public static void Resting(Player player)
            {
                Console.WriteLine("휴식하기");
                Console.WriteLine($"현재 체력 : {player.CurrentHealth}");
                Console.WriteLine($"500 G 를 내면 체력을 회복할 수 있습니다. (보유 골드 : {player.Gold} G)\n");
                Console.WriteLine("1. 휴식하기");
                Console.WriteLine("0. 나가기\n\n");
                Console.WriteLine("원하시는 행동을 입력해주세요.\n>>");
            }
        }

        public static class GameLoop
        {
            static Player player;

            public static void ErrorMessage()
            {
                Console.Clear();
                Console.WriteLine("잘못된 입력입니다");
                Thread.Sleep(1000);
            }

            public static void Start()
            {
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
                    }
                }
            }
        }
    }
}
