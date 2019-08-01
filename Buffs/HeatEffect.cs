using Entropy.Buffs;
using Terraria;
using Terraria.ID;

namespace Entropy.Buffs {
    public class HeatEffect : BuffBase{
        int rate = 1;
        int damage = 1;
        public HeatEffect(NPC npc, int damage, int duration, int rate = 10, Entity cause = null) : base(npc, cause){
            this.damage = damage;
            this.duration = duration;
            this.rate = rate;
        }
        public override void Update(NPC npc){
            if(duration%rate==0){
                int dmg = (int)Entropy.DmgCalcNPC(damage, npc, 5);
                int[] a = npc.immune;
                npc.StrikeNPC(dmg, 1, npc.direction, false, true, true);
                npc.immune = a;
                (cause as Player)?.addDPS(dmg);
                Dust.NewDust(npc.position,npc.width,npc.height,DustID.Fire);
            }
            base.Update(npc);
        }
        public override bool PreUpdate(NPC npc, bool canceled){
            return Main.rand.Next(0,10)!=0;
        }
    }
}