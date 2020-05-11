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

    public class NarulSpread : EntModProjectile {
        public override string Texture => "Terraria/Projectile_55";
        public override void SetDefaults(){
            projectile.CloneDefaults(ProjectileID.Bullet);
            projectile.aiStyle = 0;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.tileCollide = true;
            projectile.timeLeft = 60;
            projectile.extraUpdates = 14;
            projectile.ignoreWater = true;
            projectile.light = 0;
			dmgratio = dmgratiobase = new float[15]{0.85f, 0.08f, 0.07f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f};
            wallPenProgress = 7;
        }
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Narül");
		}/* 
        public override bool OnTileCollide(Vector2 oldVelocity){
            return true;
        } */
        public override bool PreKill(int timeLeft){
            return false;
        }
        public override void AI(){
            //projectile.velocity.Y-=0.025f;
            Dust d = Dust.NewDustPerfect(projectile.Center, 267, projectile.velocity, 0, new Color(100, 0, 0), 0.7f);
            //d.velocity = new Vector2(projectile.velocity.X, projectile.velocity.Y);
            d.noGravity = true;
        }
        /* public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor){
            return false;
        } */
    }
}