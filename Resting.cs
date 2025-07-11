using System;

namespace TextRpg
{
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
}