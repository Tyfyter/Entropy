using Entropy.Buffs;
using Terraria;
using Terraria.ID;

namespace Entropy.Buffs {
    public class SonarEffect : BuffBase{
        public SonarEffect(NPC npc, int duration, Entity cause = null) : base(npc, cause){
            this.duration = duration;
        }
    }
}