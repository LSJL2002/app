using System;

namespace TextRpg
{
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
                        bool typeEquipped = false;
                        foreach (var item in player.Inventory)
                        {
                            if (item.Equipped && item.Type == choosenEquip.Type)
                            {
                                typeEquipped = true;
                                break;
                            }
                        }
                        if (choosenEquip.Equipped)
                        {
                            Console.Clear();
                            choosenEquip.Equipped = false;
                            Console.WriteLine($"{choosenEquip.Name}을(를) 장착을 해제했습니다");
                            Thread.Sleep(1000);
                        }
                        else if (typeEquipped)
                        {
                            Console.Clear();
                            Console.WriteLine($"이미 같은 종류의 아이템을 장착하고 있습니다. {choosenEquip.Type}");
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
}