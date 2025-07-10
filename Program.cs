using System;

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
            public int Xp { get; set; } = 0;
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
                Console.WriteLine($"XP: {Xp} / {100 * Level}");
                Console.WriteLine("\n0. 나가기\n");
                Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");
            }

            public void CheckLevelUp()
            {
                while (Xp >= 100 * Level)
                {
                    Xp -= 100 * Level;
                    Level++;
                    Attack += 2;
                    Defense += 2;
                    Health += 10;
                    Speed += 1;
                    CurrentHealth = TotalHealth;

                    Console.Clear();
                    Console.WriteLine($"레벨업! 현재 레벨: {Level}");
                    Console.WriteLine("능력치가 상승했습니다!");
                    Console.WriteLine($"+ 공격력: {Attack}, 방어력: {Defense}, 체력: {Health}, 속도: {Speed}");
                    Thread.Sleep(2000);
                }
            }
        }
        public class Item //Items that will be both displayed on player and shop
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public int Value { get; set; }
            public string Description { get; set; }
            public int Price { get; set; }
            public int SellPrice { get; set; }
            public bool Equipped { get; set; }
            public bool IsPurchased { get; set; }
            public Item(string name, string type, int value, string description, int price)
            {
                Name = name;
                Type = type;
                Value = value;
                Description = description;
                Price = price;
                SellPrice = (int)(price * 0.85);
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
            public string SellDisplayPanel()
            {
                string priceDisplay = IsPurchased ? "구매완료" : $"{Price}G";

                string namePadded = PadDisplay(Name, 20);
                string typeValue = PadDisplay($"{Type} +{Value}", 12);
                string descPadded = PadDisplay(Description, 50);
                string pricePadded = PadDisplay(priceDisplay, 10);

                return $"{namePadded}| {typeValue}| {descPadded}| {Price * 0.85}G";
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

        public class Monster
        {
            public string Name { get; set; }
            public int MonsterHealth { get; set; }
            public int MonsterAttack { get; set; }
            public int MonsterDefense { get; set; }
            public int MonsterSpeed { get; set; }
            public int MonsterDropXP { get; set; }
            public int MonsterDropGold { get; set; }
            public Monster(string name, int hp, int attack, int defense, int speed, int xp, int gold)
            {
                Name = name;
                MonsterHealth = hp;
                MonsterAttack = attack;
                MonsterDefense = defense;
                MonsterSpeed = speed;
                MonsterDropXP = xp;
                MonsterDropGold = gold;
            }
        }

        public static class Dungeon
        {
            static List<Monster> beginnerMonsters = new List<Monster>
            {
                new Monster("슬라임", 30, 5, 2, 1, 5, 100),
                new Monster("고블린", 40, 7, 3, 2, 9, 150),
                new Monster("야생 멧돼지", 50, 8, 4, 20, 20, 100)
            };
            static List<Monster> MiddleMonsters = new List<Monster>
            {
                new Monster("골드 슬라임", 100, 1, 10, 1, 100, 1000),
                new Monster("스켈레톤", 60, 12, 3, 8, 20, 300),
                new Monster("좀피", 70, 10, 5, 3, 30, 300)
            };
            static List<Monster> EndMonsters = new List<Monster>
            {
                new Monster("르탄이", 200, 50, 30, 10, 300, 2000)
            };

            public static Monster EncoutnerMonster(Player player, int dungeonNumber)
            {
                Random rng = new Random();
                Monster selected = null;
                switch (dungeonNumber)
                {
                    case 1:
                        selected = beginnerMonsters[rng.Next(beginnerMonsters.Count)];
                        break;
                    case 2:
                        selected = MiddleMonsters[rng.Next(MiddleMonsters.Count)];
                        break;
                    case 3:
                        selected = EndMonsters[rng.Next(EndMonsters.Count)];
                        break;
                    default:
                        Console.WriteLine("잘못된 던전 번호입니다.");
                        Thread.Sleep(1000);
                        break;
                }
                Console.Clear();
                Console.WriteLine("몬스터를 만났습니다!\n");
                Console.WriteLine($"이름: {selected.Name}");
                Console.WriteLine($"체력: {selected.MonsterHealth}");
                Console.WriteLine($"공격력: {selected.MonsterAttack}");
                Console.WriteLine($"방어력: {selected.MonsterDefense}");
                Console.WriteLine($"속도: {selected.MonsterSpeed}");
                Console.WriteLine("\n\n전투 준비");
                Thread.Sleep(3000);
                return selected;
            }

            public static bool CombatSystem(Player player, Monster monster)
            {
                int monsterHP = monster.MonsterHealth;

                Console.Clear();
                while (player.CurrentHealth > 0 && monsterHP > 0)
                {
                    Console.WriteLine("=========전투 시작========\n\n\n\n");
                    Console.WriteLine($"\n{player.PlayerName} HP: {player.CurrentHealth}/{player.TotalHealth}\n");
                    Console.WriteLine("VS\n");
                    Console.WriteLine($"{monster.Name} HP: {monsterHP}\n\n");
                    Console.WriteLine("1.공격하기\n");
                    Console.WriteLine("원하시는 행동을 입력해주세요.\n>>");
                    string? input = Console.ReadLine();

                    if (input == "1")
                    {
                        Console.Clear();

                        bool pirority = false;
                        if (player.TotalSpeed >= monster.MonsterSpeed)
                        {
                            pirority = true;
                        }
                        else if (player.TotalSpeed < monster.MonsterSpeed)
                        {
                            pirority = false;
                        }

                        if (pirority)
                        {
                            int playerDamage = Math.Max(player.TotalAttack - monster.MonsterDefense, 1);
                            monsterHP -= playerDamage;
                            Console.WriteLine($"{player.PlayerName}이(가) 먼저 공격하여 {playerDamage}의 피해를 입혔습니다!");
                            Thread.Sleep(2000);
                            if (monsterHP <= 0)
                            {
                                Console.WriteLine($"{monster.Name}이 쓰려졌습니다.");
                                Console.WriteLine($"==========보상========");
                                Console.WriteLine($"경험치: {monster.MonsterDropXP}");
                                Console.WriteLine($"골드: {monster.MonsterDropGold}");
                                player.Xp += monster.MonsterDropXP;
                                player.Gold += monster.MonsterDropGold;
                                Thread.Sleep(2000);
                                player.CheckLevelUp();
                                Thread.Sleep(2000);
                                return true;
                            }
                            int monsterDamage = Math.Max(monster.MonsterAttack - player.TotalDefense, 1);
                            player.CurrentHealth -= monsterDamage;
                            Console.WriteLine($"{monster.Name}이(가) 반격하여 {monsterDamage}의 피해를 입혔습니다!");
                            Thread.Sleep(1000);
                            if (player.CurrentHealth <= 0)
                            {
                                player.CurrentHealth = 0;
                                Console.WriteLine("당신은 쓰러졌습니다... 게임 오버!");
                                Thread.Sleep(2000);
                                return false;
                            }
                        }
                        else
                        {
                            int monsterDamage = Math.Max(monster.MonsterAttack - player.TotalDefense, 1);
                            player.CurrentHealth -= monsterDamage;
                            Console.WriteLine($"{monster.Name}이(가) 먼저 공격하여 {monsterDamage}의 피해를 입혔습니다!");
                            Thread.Sleep(1000);
                            if (player.CurrentHealth <= 0)
                            {
                                player.CurrentHealth = 0;
                                Console.WriteLine("당신은 쓰러졌습니다... 게임 오버!");
                                Thread.Sleep(2000);
                                return false;
                            }
                            int playerDamage = Math.Max(player.TotalAttack - monster.MonsterDefense, 1);
                            monsterHP -= playerDamage;
                            Console.WriteLine($"{player.PlayerName}이(가) 반격하여 {playerDamage}의 피해를 입혔습니다!");
                            Thread.Sleep(2000);
                            if (monsterHP <= 0)
                            {
                                Console.WriteLine($"{monster.Name}이 쓰려졌습니다.");
                                Console.WriteLine($"==========보상========");
                                Console.WriteLine($"경험치: {monster.MonsterDropXP}");
                                Console.WriteLine($"골드: {monster.MonsterDropGold}");
                                player.Xp += monster.MonsterDropXP;
                                player.Gold += monster.MonsterDropGold;
                                Thread.Sleep(2000);
                                player.CheckLevelUp();
                                Thread.Sleep(2000);
                                return true;
                            }
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("잘못된 입력입니다");
                        Thread.Sleep(1000);
                    }
                }
                return player.CurrentHealth > 0;
            }
            public static void EnterDungeon(int dungeonNumber)
            {
                int currentRoom = 0;
                int maxRooms = 3;

                while (currentRoom < maxRooms)
                {
                    Console.Clear();
                    Console.WriteLine(" __________   __________   __________");
                    Console.WriteLine("|          | |          | |          |");

                    for (int i = 0; i < maxRooms; i++)
                    {
                        if (i == currentRoom)
                            Console.Write("|    O     |=");
                        else
                            Console.Write("|          |=");
                    }
                    Console.WriteLine();
                    Console.WriteLine("|__________|=|__________|=|__________|");
                    Console.WriteLine("\n\n던전에 오신걸 환영합니다");
                    Console.WriteLine("1.다음 방으로 이동하기\n2.상태보기");
                    Console.WriteLine("원하시는 행동을 입력해주세요.\n>>");

                    string? input = Console.ReadLine();

                    if (input == "1")
                    {
                        Console.Clear();
                        Console.WriteLine("다음 방으로 이동중.");
                        Thread.Sleep(2000);
                        Console.Clear();
                        Console.WriteLine("다음 방으로 이동중..");
                        Thread.Sleep(2000);
                        Console.Clear();
                        Console.WriteLine("다음 방으로 이동중...");
                        Thread.Sleep(2000);

                        Monster monster = EncoutnerMonster(GameLoop.player, dungeonNumber);
                        bool survived = CombatSystem(GameLoop.player, monster);

                        if (!survived)
                        {
                            Console.WriteLine("던전에서 쓰러졌습니다. 마을로 돌아갑니다...");
                            Thread.Sleep(2000);
                            return;
                        }

                        currentRoom++;
                    }
                    else if (input == "2")
                    {
                        Console.Clear();
                        GameLoop.player.ShowStatus();
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다");
                        Thread.Sleep(1000);
                    }
                }

                Console.Clear();
                Console.WriteLine("던전 클리어! 마을로 돌아갑니다...");
                Thread.Sleep(2000);
            }


            public static void ShowDungeonEntrance()
            {
                Console.WriteLine(@"
                 ______   __   __  __    _  _______  _______  _______  __    _ 
                |  _    ||  | |  ||   |_| ||    ___||    ___||   _   ||   |_| |
                | | |   ||  |_|  ||       ||   | __ |   |___ |  | |  ||       |
                | |_|   ||       ||  _    ||   ||  ||    ___||  |_|  ||  _    |
                |       ||       || | |   ||   |_| ||   |___ |       || | |   |
                |______| |_______||_|  |__||_______||_______||_______||_|  |__|
                            ⠀⠀⠀⠀⠀⠀⠀⢠⣴⣾⣷⠈⣿⣿⣿⣿⣿⡟⢀⣿⣶⣤⠀⠀⠀⠀⠀⠀⠀⠀
                            ⠀⠀⠀⠀⢠⣾⣷⡄⠻⣿⣿⣧⠘⣿⣿⣿⡿⠀⣾⣿⣿⠃⣰⣿⣶⣄⠀⠀⠀⠀
                            ⠀⠀⠀⣴⣿⣿⣿⡿⠆⠉⠉⠁⠀⠈⠉⠉⠁⠀⠙⠛⠃⢰⣿⣿⣿⣿⣷⡀⠀⠀
                            ⠀⠀⣼⣿⣿⣿⠏⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀  ⠉⢿⣿⣿⣿⣷⠀⠀
                            ⠀⠘⠛⠛⠛⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀  ⠻⠛⠛⠛⠃⠀
                            ⠀⢸⣿⣿⣿⡇⠀⠀⣀⣰⡄⠀⠀⠀⠀⠀⠀⠀⠀  ⢠⣶⣠⠀⠀ ⢰⣾⣿⣿⡇⠀
                            ⠀⢸⡿⠿⠿⠇⠀⢟⠉⠁⣳⠀⠀⠀⠀⠀⠀⠀⠀ ⣿⠈⠈⡿⠀ ⢸⣿⣿⣿⡇⠀
                            ⠀⣤⣤⣴⣶⡆⠀⢠⣀⡀⠁⠀⠀⠀⠀⠀⠀⠀⠀⠈ ⢀⣤⡀⠀ ⠘⠿⠿⠿⠇⠀
                            ⠀⣿⣿⣿⣿⣷⠀⢸⡿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀ ⢿⡇⠀  ⣶⣶⣶⣶⣶⠀
                            ⠀⣿⣿⣿⣿⣿⠀⠚⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀ ⠘⠃⠀ ⣿⣿⣿⣿⣿⠀
                            ⠀⠛⠛⠛⠛⢛⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀  ⠙⠛⠛⠋⣁⠀
                            ⠀⣿⣿⣿⣿⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀  ⣿⣿⣿⣿⣿⠀
                ");
                Console.WriteLine("\n\n던전 입장하시겠습니까?\n1. 초보자의 더전\n2. 중급자의\n3. 르탄이의 던전");
                Console.WriteLine("0. 나가기\n\n");
                Console.WriteLine("원하시는 행동을 입력해주세요.\n>>");
            }

        }

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
                                    Dungeon.EnterDungeon(1);
                                }
                                else if (userinput == "2")
                                {
                                    Dungeon.EnterDungeon(2);
                                }
                                else if (userinput == "3")
                                {
                                    Dungeon.EnterDungeon(3);
                                }
                            }
                            break;
                    }
                }
            }
        }
    }
}
