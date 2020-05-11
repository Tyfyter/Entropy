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

namespace Entropy.Projectiles{

    public class VizenShot : EntModProjectile {
        public override string Texture => "Terraria/Projectile_55";
        public override void SetDefaults(){
            projectile.CloneDefaults(ProjectileID.Bullet);
            projectile.aiStyle = 0;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.tileCollide = true;
            projectile.timeLeft = 300;
            projectile.extraUpdates = 14;
            projectile.ignoreWater = true;
			dmgratio = dmgratiobase = new float[15]{0.34f, 0.33f, 0.33f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f};
        }
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Vizen");
		}/* 
        public override bool OnTileCollide(Vector2 oldVelocity){
            return true;
        } */
        public override bool PreKill(int timeLeft){
            return false;
        }
        public override void AI(){
            //projectile.velocity.Y-=0.025f;
            Dust d = Dust.NewDustPerfect(projectile.Center, 267, null, 0, new Color(0, 255, 128), 0.6f);
            d.velocity = new Vector2(projectile.velocity.X*0.9f, projectile.velocity.Y*0.9f);
            d.fadeIn = 0.7f;
            d.noGravity = true;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor){
            return false;
        }
    }
}