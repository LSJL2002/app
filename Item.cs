using System;

namespace TextRpg
{
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
}