using Entropy.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace Entropy.Buffs {
    public class ColdEffect : BuffBase{
        int rate = 1;
        public override Color? color {
            get{return rcolor;}
            set{rcolor = value.Value;}
        }
        Color rcolor = new Color(100,235,255);
        public override bool isActive{
            get{return base.isActive&&rate>1;}
            set{base.isActive = value;}
        }
        //new string[15] {"Slash", "Impact", "Puncture", "Cold", "Electric", "Heat", "Toxic", "Blast", "Corrosive", "Gas", "Magnetic", "Radiation", "Viral", "True", "Void"}
        public ColdEffect(NPC npc, int duration, int rate = 3) : base(npc){
            this.duration = duration;
            this.rate = rate;
        }
        public override bool PreUpdate(NPC npc, bool canceled){
            if(npc.aiStyle==37||npc.aiStyle==6||(npc.aiStyle>=45&&npc.aiStyle<=49)||npc.type==NPCID.WallofFleshEye){
                isActive = false;
                return true;
            }
            if(canceled&&duration%rate==0)duration++;
            return duration%rate==0;
        }
        public ColdEffect withColor(Color color){
            this.color = color;
            return this;
        }
    }
}