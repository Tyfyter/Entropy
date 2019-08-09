using System;
using System.Collections.Generic;
using Entropy.Buffs;
using Entropy.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Entropy.NPCs.EntropyGlobalNPC;

namespace Entropy.Projectiles
{

    public class ValhallaAbility : EntModProjectile{
		public override string Texture => "Entropy/Items/Mods/ComboSpeed";
        public override void SetDefaults(){
            projectile.width = projectile.height = 160;
            projectile.friendly = true;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 1;
            projectile.extraUpdates = 3;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
			dmgratio = dmgratiobase = new float[15]{0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 1f, 0f};
        }
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Valhalla");
		}
        public override void AI(){
            for (int y = 0; y < 60; y++){
                Vector2 velocity = new Vector2(8,0).RotatedBy(MathHelper.ToRadians(y*6));
                Dust d = Dust.NewDustPerfect(projectile.Center+velocity, 267, null, 0, Color.OrangeRed, 0.6f);
                d.velocity = velocity*2;
                //d.position -= d.velocity * 8;
                d.fadeIn = 0.7f;
                d.noGravity = true;
            }
        }
        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit){
            if(target.team == Main.player[projectile.owner].team){
                target.AddBuff(mod.BuffType<Warcry>(), 600);
            }else{
                target.noKnockback = false;
            }
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockBack, ref bool crit, ref int hitDirection){
            AddBuff(new BlastEffect(target, 180));
            float f = target.Center.X-projectile.Center.X;
            hitDirection = (int)(f/Math.Abs(f));
            //if(f!=0)knockBack=(f/Math.Abs(f))*Math.Abs(knockBack);
        }
        public override bool CanHitPlayer(Player target){
            bool t = (target.team == Main.player[projectile.owner].team);
            if(t)target.statLife++;
            return (projectile.damage == 1) == t;
        }
        public override bool? CanHitNPC(NPC target){
            return projectile.damage != 1;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor){
            return false;
        }
    }
}