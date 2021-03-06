using Entropy.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace Entropy.Buffs {
    public class ToxicEffect : BuffBase{
        int rate = 1;
        int damage = 1;
        public override Color? color => Color.DarkGreen;
        public override int value => (int)(damage*(duration/(float)rate));
        public ToxicEffect(NPC npc, int damage, int duration, int rate = 12, Entity cause = null) : base(npc, cause){
            this.damage = damage;
            this.duration = duration;
            this.rate = rate;
        }
        public override void Update(NPC npc){
            if(duration%rate==0){
                int[] a = npc.immune;
                int dmg = (int)Entropy.DmgCalcNPC(damage, npc, 6);
                npc.StrikeNPC(dmg, 0, 0, false, true, true);
                npc.immune = a;
                Dust.NewDust(npc.position,npc.width,npc.height,DustID.Fire, newColor:color.Value*3);
                (cause as Player)?.addDPS(dmg);
            }
            base.Update(npc);
        }
    }
}