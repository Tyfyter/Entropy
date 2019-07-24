using Entropy.Buffs;
using Terraria;

namespace Entropy.Buffs {
    public class BleedEffect : BuffBase{
        int rate = 1;
        int damage = 1;
        public BleedEffect(NPC npc, int damage, int duration, int rate = 10) : base(npc){
            this.damage = damage;
            this.duration = duration;
            this.rate = rate;
        }
        public override void Update(NPC npc){
            if(duration%rate==0){
                int[] a = npc.immune;
                npc.StrikeNPC((int)Entropy.DmgCalcNPC(damage, npc, 13), 0, 0, false, true, true);
                npc.immune = a;
            }
            base.Update(npc);
        }
    }
}