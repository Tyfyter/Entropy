using Entropy.Buffs;
using Microsoft.Xna.Framework;
using Terraria;

namespace Entropy.Buffs {
    public class CorrEffect : BuffBase{
        public int severity;
        public int severityloss;
        public override bool isActive{
            get{return severity>0&&npc.active;}
            set{if(!value)severity=0;}
        }
        public CorrEffect(NPC npc, int sev, int sevloss = 1) : base(npc){
            severity = sev;
            severityloss = sevloss;
        }
        public override void Update(NPC npc){
            if(severityloss == 0)base.Update(npc);
            if(duration<=0){
                if(Main.rand.NextDouble()<0.06){
                    severity-=severityloss;
                    duration = 10;
                for(int i = 0; i < 2; i++)Dust.NewDustDirect(npc.position, npc.width, npc.height, 267, Alpha:200, newColor:Color.LimeGreen).noGravity = true;
                }
            }else{
                duration--;
            }
        }
    }
}