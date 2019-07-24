using Entropy.Buffs;
using Terraria;
using Terraria.ID;

namespace Entropy.Buffs {
    public class HeatEffect : BuffBase{
        int rate = 1;
        int damage = 1;
        public HeatEffect(NPC npc, int damage, int duration, int rate = 10) : base(npc){
            this.damage = damage;
            this.duration = duration;
            this.rate = rate;
        }
        public override void Update(NPC npc){
            if(duration%rate==0){
                int[] a = npc.immune;
                npc.StrikeNPC((int)Entropy.DmgCalcNPC(damage, npc, 5), 1, npc.direction, false, true, true);
                npc.immune = a;
                Dust.NewDust(npc.position,npc.width,npc.height,DustID.Fire);
            }
            base.Update(npc);
        }
        public override bool PreUpdate(NPC npc){
            return Main.rand.Next(0,10)!=0;
        }
    }
}