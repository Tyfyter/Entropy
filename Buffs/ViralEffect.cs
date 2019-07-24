using Entropy.Buffs;
using Terraria;

namespace Entropy.Buffs {
    public class ViralEffect : BuffBase{
        public ViralEffect(NPC npc, int duration) : base(npc){
            this.duration = duration;
            npc.life/=2;
            npc.lifeMax/=2;
        }
        public override void Update(NPC npc){
            base.Update(npc);
            if(!isActive){
                npc.lifeMax*=2;
                npc.life*=2;
            }
        }
    }
}