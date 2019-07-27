using Entropy.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace Entropy.Buffs {
    public class MagEffect : BuffBase{
        int rate = 1;
        public override Color? color => new Color(0,135,255);
        //new string[15] {"Slash", "Impact", "Puncture", "Cold", "Electric", "Heat", "Toxic", "Blast", "Corrosive", "Gas", "Magnetic", "Radiation", "Viral", "True", "Void"}
        public MagEffect(NPC npc, int duration, int rate = 5) : base(npc){
            this.duration = duration;
            this.rate = rate;
        }
        public override bool PreUpdate(NPC npc, bool canceled){
            return Main.rand.Next(rate)==0;
        }
    }
}