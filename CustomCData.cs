/*
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI;
using Terraria.ModLoader;

namespace Entropy{
    public class CustomCData : CustomCurrencySingleCoin
    {
        public Color CustomCurrencytextcolor = Color.White; //this defines the Custom Currency Buy Price color when shown in the shoop
        int coinType;
        public CustomCData(int coinItemID, long currencyCap) : base(coinItemID, currencyCap){
            coinType = coinItemID;
        }

        public override void GetPriceText(string[] lines, ref int currentLine, int price)
        {
            Color color = CustomCurrencytextcolor * ((float)Main.mouseTextColor / 255f);
            lines[currentLine++] = string.Format("[c/{0:X2}{1:X2}{2:X2}:{3} {4} {5}]", new object[]
                {
                    color.R,
                    color.G,
                    color.B,
                    Lang.tip[50],
                    price,
                    Lang.GetItemNameValue(coinType)
                });
        }
    }
}
//*/