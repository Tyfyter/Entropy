using Entropy.Buffs;
using Terraria;

namespace Entropy.Buffs {
    public class BleedEffect : BuffBase{
        int rate = 1;
        int damage = 1;
        public override int value => (int)(damage*(duration/(float)rate));
        public BleedEffect(NPC npc, int damage, int duration, int rate = 12, Entity cause = null) : base(npc, cause){
            this.damage = damage;
            this.duration = duration;
            this.rate = rate;
        }
        public override void Update(NPC npc){
            if(duration%rate==0){
                int[] a = npc.immune;
                int dmg = (int)Entropy.DmgCalcNPC(damage, npc, 13);
                npc.StrikeNPC(dmg, 0, 0, false, true, true);
                npc.immune = a;
                (cause as Player)?.addDPS(dmg);
            }
            base.Update(npc);
        }
    }
}