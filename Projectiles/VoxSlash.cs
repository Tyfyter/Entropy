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

    public class VoxSlash : EntModProjectile {
        public override string Texture => "Terraria/Projectile_55";
        public override void SetDefaults(){
            projectile.CloneDefaults(ProjectileID.Stinger);
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.tileCollide = true;
            projectile.timeLeft = 30;
            projectile.extraUpdates = 10;
            projectile.ignoreWater = true;
			dmgratio = dmgratiobase = new float[15]{0.9f, 0.01f, 0.09f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f};
        }
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Vox Slash");
		}
        public override bool OnTileCollide(Vector2 oldVelocity){
            projectile.velocity+=projectile.velocity-oldVelocity;
            return false;
        }
        public override bool PreKill(int timeLeft){
            return false;
        }
        public override void AI(){
            projectile.velocity.Y-=0.025f;
            Dust d = Dust.NewDustPerfect(projectile.Center, 267, null, 0, Color.Azure, 0.6f);
            d.velocity = new Vector2(projectile.velocity.X/2, projectile.velocity.Y/2);
            d.fadeIn = 0.7f;
            d.noGravity = true;
        }
        /*public override void ModifyHitNPC(NPC target, ref int damage, ref float knockBack, ref bool crit, ref int hitDirection){
            if(Main.player[projectile.owner].HasBuff(BuffID.Hunter)){
                damage = (int)(damage*1.25f);
                crit = true;
            }
        }*/
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor){
            return false;
        }
    }
}