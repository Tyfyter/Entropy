using System;
using System.Collections.Generic;
using Entropy.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace Entropy.Buffs {
    public class FrostburnEffect : BuffBase, IPostHitBuff{
        static int impacts = 0;
        static int dusts = 0;
        public override int priority => 3;
        public bool HitAction {get => hitAction; set => hitAction = value;}
        static bool hitAction = true;
        public override Color? color => new Color(0,235,255);
        public FrostburnEffect(NPC npc, int duration) : base(npc){
            this.duration = duration;
        }
        public override void ModifyHitItem(Player attacker, Item item, NPC target, ref int damage, ref bool crit){
            impacts++;
            if(damage/(impacts*1.5f)>target.defense)damage+=(int)(damage/(impacts*1.5f)-target.defense);
        }
        public override void ModifyHitProjectile(Projectile projectile, NPC target, ref int damage, ref bool crit){
            impacts++;
            if(damage>target.defense)damage+=damage-target.defense;
        }
        public override bool PreUpdate(NPC npc, bool canceled){
            dusts = 0;
            return base.PreUpdate(npc, canceled);
        }
        public override void Update(NPC npc){
            base.Update(npc);
            if(Main.time%(++dusts*2)!=0)return;
            Dust d = Dust.NewDustDirect(npc.position,npc.width,npc.height,135);
            d.noGravity = true;
            d.fadeIn = 1.5f;
        }
        public void postHitAction(){
            impacts = 0;
            HitAction = true;
        }
    }
}