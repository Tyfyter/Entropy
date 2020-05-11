using System;
using System.Collections.Generic;
using Entropy.Buffs;
using Microsoft.Xna.Framework;
using Terraria;

namespace Entropy.Buffs {
    public class ViralEffect : BuffBase, IPostHitBuff{
        static int impacts = 0;
        public override int priority => 2;
        public bool HitAction {get => hitAction; set => hitAction = value;}
        static bool hitAction = true;
        public override Color? color => Color.DarkOliveGreen;
        public ViralEffect(NPC npc, int duration) : base(npc){
            this.duration = duration;
            //i = npc.lifeMax%2;
            //npc.life/=2;
            //npc.lifeMax/=2;
        }
        public override void ModifyHitItem(Player attacker, Item item, NPC target, ref int damage, ref bool crit){
            impacts++;
            damage+=damage/(impacts*3);
        }
        public override void ModifyHitProjectile(Projectile projectile, NPC target, ref int damage, ref bool crit){
            impacts++;
            damage+=damage/(impacts*3);
        }
        public void postHitAction(){
            impacts = 0;
            HitAction = true;
        }
        /*public override void Update(NPC npc){
            base.Update(npc);
            if(!isActive){
                npc.lifeMax=(npc.lifeMax*2)+i;
                npc.life*=2;
            }
        }*/
    }
}