using Entropy.Buffs;
using Terraria;

namespace Entropy.Buffs {
    public class PuncEffect : BuffBase{
        public PuncEffect(NPC npc, int duration) : base(npc){
            this.duration = duration;
        }
        public override void ModifyHit(NPC npc, Player target, ref int damage, ref bool crit){
            damage=(int)(damage*0.7f);
            crit = false;
        }
    }
}