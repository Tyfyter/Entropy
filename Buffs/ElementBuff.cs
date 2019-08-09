using System;
using Entropy.Buffs;
using Entropy.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using static Entropy.NPCs.EntropyGlobalNPC;

namespace Entropy.Buffs {
    public class ElementBuff : PlayerBuffBase{
        public override Color? color {
            get{
                switch (type){
                    case 0:
                    return Color.DarkRed;
                    case 3:
                    return new Color(100,235,255);
                    case 4:
                    return Color.MediumPurple;
                    case 5:
                    return Color.Lerp(Color.Orange,Color.OrangeRed,(float)Math.Sin(Main.time/20)+1);
                    case 6:
                    return Color.DarkGreen;
                    case 11:
                    return Color.Lerp(new Color(225, 0, 255),new Color(0,255,255),(float)Math.Sin(Main.time/20)+1);
                }
                return Color.Black;
            }
        }
        int type = 0;
        float amount = 0;
        //new string[15] {"Slash", "Impact", "Puncture", "Cold", "Electric", "Heat", "Toxic", "Blast", "Corrosive", "Gas", "Magnetic", "Radiation", "Viral", "True", "Void"}
        public ElementBuff(Player player, int duration, int type, float amount = 0.5f) : base(player){
            this.duration = duration;
            this.type = type;
            this.amount = amount;
        }
        public override void ModifyHitItem(Player attacker, EntModItem item, NPC target, ref int damage, ref bool crit, ref float[] dr){
            item.addElement(type, amount);
            if(crit)AddBuff(BuffBase.GetFromIndex(target, type, damage/2, attacker));
        }
    }
}