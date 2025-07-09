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
            public int Gold { get; set; } = 1500;
            public List<Item> Inventory { get; set; } = new List<Item>();

            public Player(string name)
            {
                PlayerName = name;
            }

            

            public void ShowStatus()
            {
                Console.WriteLine("\n상태 보기\n캐릭터의 정보가 표시됩니다.\n");
                Console.WriteLine($"Lv. {Level}\n{PlayerName} ( {Job} )");
                //Attack
                //Defense
                Console.WriteLine($"체 력 : {Health}");
                Console.WriteLine($"Gold : {Gold} G");
                Console.WriteLine("\n0. 나가기\n");
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
            public string ShopDisplayPanel()
            {
                string priceDisplay = IsPurchased ? "구매완료" : $"{Price}G";
                return $"{Name} | {Type} + {Value} | {Description} | {priceDisplay}";
            }
        }

        public class Shop
        {
            public static List<Item> ShopItem = new List<Item>
            {
                new Item ("철검", "공격력",10,"쉽게 볼 수 있는 낡은 검 입니다.",300)
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
            }

            public static void PurchaseItem(Player player)
            {
                while (true)
                {
                    Console.WriteLine($"상점 - 아이템 구매\n필요한 아이템을 얻을 수 있는 상점입니다.\n\n[보유 골드]\n{player.Gold}G");
                    Console.WriteLine("[아이템 목록]");
                    for (int i = 0; i < ShopItem.Count; i++)
                    {
                        var item = ShopItem[i];
                        Console.WriteLine($"{i + 1}. {item.ShopDisplayPanel()}");
                    }
                    Console.WriteLine("\n0. 상점으로 돌아가기");
                    Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");
                    string? input = Console.ReadLine();
                    if (input == "0")
                    {
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
        }

        public static class GameLoop
        {
            static Player player;
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
                    Console.WriteLine("\n스파르타 마을에 오신 여러분 환영합니다.");
                    Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.\n");
                    Console.WriteLine("1. 상태 보기");
                    Console.WriteLine("2. 인벤토리");
                    Console.WriteLine("3. 상점");
                    Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");
                    string? input = Console.ReadLine();
                    switch (input)
                    {
                        case "1":
                            Console.Clear();
                            player.ShowStatus();
                            while (true)
                            {
                                string? userinput = Console.ReadLine();
                                if (userinput != "0")
                                {
                                    Console.WriteLine("잘못된 입력입니다.");
                                }
                                else
                                {
                                    break;
                                }
                            }
                            break;
                        case "2":
                            Console.Clear();
                            Inventory.ShowInventory(player);
                            while (true)
                            {
                                string? userinput = Console.ReadLine();
                                if (userinput != "0")
                                {
                                    Console.WriteLine("잘못된 입력입니다.");
                                }
                                else
                                {
                                    break;
                                }
                            }
                            break;
                        case "3":
                            Console.Clear();
                            Shop.ShopDisplay(player);
                            while (true)
                            {
                                Shop.ShopDisplay(player);
                                string? userinput = Console.ReadLine();
                                if (userinput == "0")
                                    break;
                                else if (userinput == "1")
                                    Shop.PurchaseItem(player);
                                else
                                    Console.WriteLine("잘못된 입력입니다.");
                            }
                            break;
                    }
                }
            }
        }
    }
}
