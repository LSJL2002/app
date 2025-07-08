using System;
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
            public string PlayerName;
            public int Level = 1;
            public string Job;
            public int Attack = 10;
            public int Defense = 5;
            public int Health = 100;
            public int Gold = 1500;
            public List<Item> Inventory = new List<Item>();

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
            public string Name;
            public string Type;
            public int Value;
            public string Description;
            public int Price;
            public bool Equipped;
            public bool IsPurchased;
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
            public string ShopDisplay()
            {
                string priceDisplay = IsPurchased ? "구매완료" : $"{Price}G";
                return $"{Name} | {Type} + {Value} | {Description} | {priceDisplay}";
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
            }
        }

        public class GameLoop()
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
                Console.WriteLine("당신의 직업은 무엇인가요?");
                for (int i = 0; i < jobs.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {jobs[i]}");
                }
                
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
                        }
                    }
            }
        }
    }
    
}
