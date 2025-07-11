using System;

namespace TextRpg
{
        public class Player //Player Info
    {
        public string PlayerName { get; set; }
        public int Level { get; set; } = 1;
        public string Job { get; set; } = "무직";
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
}