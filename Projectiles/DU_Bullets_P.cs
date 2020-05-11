using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Entropy.Projectiles{
    public class DU_Bullets_P : EntModProjectile{
        public override void SetDefaults(){
            //projectile.name = "Wind Shot";  //projectile name
            projectile.width = 12;       //projectile width
            projectile.height = 12;  //projectile height
            projectile.friendly = true;      //make the projectile will not damage players allied with its owner
            projectile.ranged = true;         //
            projectile.tileCollide = true;   //make it so that the projectile will be destroyed if it hits terrain
            projectile.penetrate = 20;      //how many npcs will penetrate
            projectile.timeLeft = 600;   //how many time this projectile has before it expipires
            projectile.extraUpdates = 4;
            projectile.ignoreWater = true;
            projectile.localNPCHitCooldown = 20;
            projectile.usesLocalNPCImmunity = true;
			dmgratio = dmgratiobase = new float[15]{0f, 0.15f, 0.8f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.05f, 0f, 0f, 0f};
            statchance = 23;
        }
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("DU Round");
		}
        public override void AI(){
            projectile.rotation = projectile.velocity.ToRotation();
        }
        public override void PostShoot(){

        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockBack, ref bool crit, ref int hitDirection){
            dmgratio[11]+=0.05f;
            Projectile.NewProjectileDirect(projectile.Center, Vector2.Zero, ModContent.ProjectileType<DU_Burn>(), damage/5, 0, projectile.owner).rotation = projectile.rotation-(float)(Math.PI/2);
            base.ModifyHitNPC(target, ref damage, ref knockBack, ref crit, ref hitDirection);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor){
            Dust.NewDustPerfect(projectile.position, /*projectile.width, projectile.height, */DustID.GoldFlame, new Vector2()/*, 0, 0*/, 0, new Color(-255, -255, 255));
            return true;
        }
    }
}