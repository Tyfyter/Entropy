using Terraria;
using Terraria.ID;
using static Entropy.NPCs.EntropyGlobalNPC;
using Microsoft.Xna.Framework;
using static Entropy.EntropyExt;

namespace Entropy.Buffs {
    public class GasEffect : BuffBase{
        int damage = 1;
        //int basedamage = 1;
        public override bool isActive => false;
        public GasEffect(NPC npc, int damage, int duration, Entity cause = null) : base(npc, cause){
            this.damage = damage;
            foreach (NPC npc2 in Main.npc){
                Vector2 va = constrain(npc.Center, npc2.TopLeft, npc2.BottomRight);
                Vector2 vb = constrain(npc2.Center, npc.TopLeft, npc.BottomRight);
                if((va-vb).Length()>96)continue;
                AddBuff(new ToxicEffect(npc2, damage, duration, cause:cause));
            }
            duration = 2;
        }
    }
}