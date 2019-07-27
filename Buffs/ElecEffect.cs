using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using static Entropy.EntropyExt;

namespace Entropy.Buffs {
    public class ElecEffect : BuffBase{
        int damage = 1;
        int basedamage = 1;
        public ElecEffect(NPC npc, int damage, int duration = 0) : base(npc){
            this.damage = damage;
            basedamage = damage;
            trigger(npc, true);
            this.duration = duration;
        }
        public override bool PreUpdate(NPC npc, bool canceled){
            return !isActive;
        }
        void trigger(NPC npc, bool self = false){
            try{
                if(self){
                    int[] a = npc.immune;
                    npc.immune = new int[npc.immune.Length];
                    npc.StrikeNPC((int)Entropy.DmgCalcNPC(damage, npc, 4), 0, 0, false, true, true);
                    npc.immune = a;
                }
                foreach (NPC npc2 in Main.npc){
                    if(npc.whoAmI==npc2.whoAmI)continue;
                    Vector2 va = constrain(npc.Center, npc2.TopLeft, npc2.BottomRight);
                    Vector2 vb = constrain(npc2.Center, npc.TopLeft, npc.BottomRight);
                    if((va-vb).Length()>96)continue;
                    Dust.NewDust(npc2.Center, 0, 0, DustID.Electric);
                    int[] a = npc2.immune;
                    npc2.immune = new int[npc2.immune.Length];
                    npc2.StrikeNPC((int)Entropy.DmgCalcNPC(damage, npc, 4), 0, 0, false, true, true);
                    npc2.immune = a;
                    if(damage>0){
                        damage-=basedamage/10;
                        trigger(npc2, false);
                    }
                }
            }catch (System.Exception){}
        }
    }
}