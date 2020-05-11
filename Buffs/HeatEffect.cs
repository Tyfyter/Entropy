using System;
using Entropy.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace Entropy.Buffs {
    public class HeatEffect : BuffBase{
        public int rate = 1;
        public int damage = 1;
        public bool noKb = false;
        public override int value => (int)(damage*(duration/(float)rate));
        public HeatEffect(NPC npc, int damage, int duration, int rate = 15, Entity cause = null, bool Kb = true) : base(npc, cause){
            this.damage = damage;
            this.duration = duration;
            this.rate = rate;
            this.noKb = !Kb;
        }
        public override void Update(NPC npc){
            if(duration%rate==0){
                int dmg = (int)Entropy.DmgCalcNPC(damage, npc, 5);
                int[] a = npc.immune;
                Vector2 oldVel = npc.velocity;
                npc.StrikeNPC(dmg, Math.Max(rate/10f-0.5f, 0), npc.direction, false, true, true);
                if(noKb)npc.velocity = oldVel;
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