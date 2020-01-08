using Entropy.Buffs;
using Entropy.NPCs;
using Terraria;

namespace Entropy.Buffs {
    public class YoteEffect : BuffBase{
        float kbr = 1;
        bool kb = false;
        public YoteEffect(NPC npc, int duration) : base(npc){
            this.duration = duration;
            npc.damage+=20;
            if(npc.knockBackResist>=0.4f)return;
            kb = true;
            kbr = npc.knockBackResist;
            npc.knockBackResist = 0.4f;
        }
        public override void Update(NPC npc){
            //npc.knockBackResist = 0.05f;
            npc.GetGlobalNPC<EntropyGlobalNPC>().rad = true;
            if(npc.boss||npc.collideX||npc.collideY||npc.noTileCollide||npc.wet||npc.noGravity){
                base.Update(npc);
            }
            if(!isActive){
                npc.damage-=20;
                if(kb)npc.knockBackResist = kbr;
            }
        }
        public override void ModifyHitNPC(NPC npc, NPC target, ref int damage, ref float knockback, ref bool crit){
            damage+=(int)(npc.velocity.Length()*5);
            damage*=5;
            npc.StrikeNPC(damage/2, knockback, 0, crit);
        }
        public override bool PreUpdate(NPC npc, bool canceled){
            return false;
        }
    }
}