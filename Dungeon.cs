using System;

namespace TextRpg
{
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
        public static void EnterDungeon(int dungeonNumber, Player player)
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
                    Thread.Sleep(500);
                    Console.Clear();
                    Console.WriteLine("다음 방으로 이동중..");
                    Thread.Sleep(500);
                    Console.Clear();
                    Console.WriteLine("다음 방으로 이동중...");
                    Thread.Sleep(500);

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
            if (dungeonNumber == 1)
            {
                Console.WriteLine("던전 클리어! 마을로 돌아갑니다...");
                Console.WriteLine("=========보상========");
                Console.WriteLine("Gold: +1000G\nExp: +30Xp");
                player.Gold += 1700;
                player.Xp += 30;
            }
            else if (dungeonNumber == 2)
            {
                Console.WriteLine("던전 클리어! 마을로 돌아갑니다...");
                Console.WriteLine("=========보상========");
                Console.WriteLine("Gold: +1700G\nExp: +50Xp");
                player.Gold += 1700;
                player.Xp += 50;
            }
            else
            {
                Console.WriteLine("던전 클리어! 마을로 돌아갑니다...");
                Console.WriteLine("=========보상========");
                Console.WriteLine("Gold: +2500G\nExp: +100Xp");
                player.Gold += 2500;
                player.Xp += 100;
            }
            Thread.Sleep(3000);
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
}