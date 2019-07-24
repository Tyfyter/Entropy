using Entropy.Buffs;
using Terraria;

namespace Entropy.Buffs {
    public class ImpactEffect : BuffBase{
        float kbr = 1;
        bool kb = false;
        public ImpactEffect(NPC npc, int duration) : base(npc){
            this.duration = duration;
            if(npc.knockBackResist>=0.4f)return;
            kb = true;
            kbr = npc.knockBackResist;
            npc.knockBackResist = 0.4f;
        }
        public override void Update(NPC npc){
            npc.knockBackResist = 0.05f;
            if(npc.boss||npc.collideX||npc.collideY||npc.noTileCollide||npc.wet||npc.noGravity)base.Update(npc);
            if(!isActive&&kb){
                npc.knockBackResist = kbr;
            }
        }
        public override bool PreUpdate(NPC npc){
            return false;
        }
    }
}