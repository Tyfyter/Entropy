using Entropy.Buffs;
using Microsoft.Xna.Framework;
using Terraria;

namespace Entropy.Buffs {
    public class ViralEffect : BuffBase{
        int i = 0;
        public override Color? color => Color.DarkOliveGreen;
        public ViralEffect(NPC npc, int duration) : base(npc){
            this.duration = duration;
            i = npc.lifeMax%2;
            npc.life/=2;
            npc.lifeMax/=2;
        }
        public override void Update(NPC npc){
            base.Update(npc);
            if(!isActive){
                npc.lifeMax=(npc.lifeMax*2)+i;
                npc.life*=2;
            }
        }
    }
}