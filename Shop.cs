using System;

namespace TextRpg
{
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
            Console.WriteLine("\n1. 아이템 구매\n2. 아이템 판매\n0. 나가기");
            Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");
        }

        public static void SellItem(Player player)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"상점 - 아이템 판매\n필요없는 아이템을 판매할 수 있는 상점입니다.\n\n[보유 골드]\n{player.Gold}G");
                Console.WriteLine("[아이템 목록]");
                if (player.Inventory.Count == 0)
                {
                    Console.WriteLine("인벤토리에 아무것도 없습니다");
                }
                else
                {
                    for (int i = 0; i < player.Inventory.Count; i++)
                    {
                        var item = player.Inventory[i];
                        Console.WriteLine($"{i + 1}. {item.SellDisplayPanel()}");
                    }
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
                if (int.TryParse(input, out int choice) && choice >= 1 && choice <= player.Inventory.Count)
                {
                    var choosenItem = ShopItem[choice - 1];
                    if (choosenItem != null)
                    {
                        Console.Clear();
                        Console.WriteLine($"Selling Item Debug Giving {choosenItem.SellPrice}G");
                        player.Inventory.Remove(choosenItem);
                        choosenItem.IsPurchased = false;
                        player.Gold += choosenItem.SellPrice;
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
}
