using Entropy.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace Entropy.Buffs {
    public class ColdEffect : BuffBase{
        int rate = 1;
        public override Color? color => new Color(0,235,255);
        //new string[15] {"Slash", "Impact", "Puncture", "Cold", "Electric", "Heat", "Toxic", "Blast", "Corrosive", "Gas", "Magnetic", "Radiation", "Viral", "True", "Void"}
        public ColdEffect(NPC npc, int duration, int rate = 3) : base(npc){
            this.duration = duration;
            this.rate = rate;
        }
        public override bool PreUpdate(NPC npc, bool canceled){
            if(canceled&&duration%rate==0)duration++;
            return duration%rate==0;
        }
    }
}