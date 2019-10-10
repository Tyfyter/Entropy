using System;
using Entropy.Buffs;
using Entropy.NPCs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace Entropy.Buffs {
    public class RadEffect : BuffBase{
        public override Color? color {get{
            return Color.Lerp(new Color(225, 0, 255),new Color(0,255,255),(float)Math.Sin(Main.time/20)+1);
        }}
        //new string[15] {"Slash", "Impact", "Puncture", "Cold", "Electric", "Heat", "Toxic", "Blast", "Corrosive", "Gas", "Magnetic", "Radiation", "Viral", "True", "Void"}
        public RadEffect(NPC npc, int duration) : base(npc){
            this.duration = duration;
        }
        public override void Update(NPC npc){
            base.Update(npc);
            npc.GetGlobalNPC<EntropyGlobalNPC>().rad = true;
        }
    }
}