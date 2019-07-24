using Entropy.Buffs;
using Terraria;

namespace Entropy.Buffs {
    public class BlastEffect : BuffBase{
        int baseduration = 1;
        float kbr = 1;
        bool kb = false;
        public BlastEffect(NPC npc, int duration) : base(npc){
            this.duration = duration;
            if(npc.knockBackResist>=0.9f)return;
            kb = true;
            kbr = npc.knockBackResist;
            npc.knockBackResist = 0.9f;
        }
        public override void Update(NPC npc){
            if(npc.boss||npc.collideX||npc.collideY||npc.noTileCollide||npc.wet||npc.noGravity){
                base.Update(npc);
            }else if(duration < baseduration){
                duration++;
            }
            if(!isActive&&kb){
                npc.knockBackResist = kbr;
            }
        }
        public override bool PreUpdate(NPC npc){
            return false;
        }
    }
}