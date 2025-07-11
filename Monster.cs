using System;

namespace TextRpg
{
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
}