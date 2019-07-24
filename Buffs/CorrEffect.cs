using Entropy.Buffs;
using Terraria;

namespace Entropy.Buffs {
    public class CorrEffect : BuffBase{
        public int severity;
        public CorrEffect(NPC npc, int sev) : base(npc){
            severity = sev;
        }
        public override void Update(NPC npc){}
    }
}