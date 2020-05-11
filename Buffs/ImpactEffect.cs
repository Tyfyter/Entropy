using System;
using System.Collections.Generic;
using Entropy.Buffs;
using Terraria;

namespace Entropy.Buffs {
    public class ImpactEffect : BuffBase, IPostHitBuff{
        static int impacts = 0;
        public bool HitAction {get => hitAction; set => hitAction = value;}
        static bool hitAction = true;
        public ImpactEffect(NPC npc, int duration) : base(npc){
            this.duration = duration;
        }
        public override void ModifyHitItem(Player attacker, Item item, NPC target, ref int damage, ref bool crit){
            impacts++;
            damage+=target.defense/(3+impacts*2);
        }
        public override void ModifyHitProjectile(Projectile projectile, NPC target, ref int damage, ref bool crit){
            impacts++;
            damage+=target.defense/(3+impacts*2);
        }
        public void postHitAction(){
            impacts = 0;
            HitAction = true;
        }
        /*float kbr = 1;
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
            if(npc.boss||npc.collideX||npc.collideY||npc.noTileCollide||npc.wet||npc.noGravity){
                base.Update(npc);
            }
            if(!isActive&&kb){
                npc.knockBackResist = kbr;
            }
        }
        public override bool PreUpdate(NPC npc, bool canceled){
            return false;
        }*/
    }
}