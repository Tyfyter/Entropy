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

    public class VizenSlash : EntModProjectile {
        public override string Texture => "Terraria/Projectile_55";
        public override bool CloneNewInstances => true;
        public double angle;
        public override void SetDefaults(){
            projectile.CloneDefaults(ProjectileID.Bullet);
            projectile.Size = new Vector2(48, 48);
            projectile.aiStyle = 0;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 45;
            projectile.extraUpdates = 10;
            projectile.ignoreWater = true;
			dmgratio = dmgratiobase = new float[15]{0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f};
            statchance = 100;
        }
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Vizen Slash");
		}/* 
        public override bool OnTileCollide(Vector2 oldVelocity){
            return true;
        } */
        public override bool PreKill(int timeLeft){
            return false;
        }
        public override void AI(){
            Player player = Main.player[projectile.owner];
            projectile.Center = player.MountedCenter+projectile.velocity;
            angle = (projectile.timeLeft/12f*player.direction-(player.direction*Math.PI)/4f);
            //projectile.velocity.Y-=0.025f;
            Vector2 center = projectile.Center;
            Vector2 end = center + (projectile.velocity*((45f-projectile.timeLeft)/45+3)).RotatedBy(angle);
            for(float i = 0; i < 24; i++){
                Vector2 p = Vector2.Lerp(center, end, i/24f);
                Dust d = Dust.NewDustPerfect(p, 267, null, 0, new Color(0, 255, 128), 0.6f);
                d.velocity = new Vector2();//new Vector2(projectile.velocity.X*0.9f, projectile.velocity.Y*0.9f);
                d.fadeIn = 0.7f;
                d.noGravity = true;
            }
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox){
            Vector2 center = projectile.Center;
            Vector2 end = center + (projectile.velocity*4).RotatedBy(angle);
            float i = 0;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), center, end, 5, ref i);
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit){
            if(!target.HasBuff<YoteEffect>())target.velocity = Vector2.Lerp(target.velocity-new Vector2(0, 2), projectile.velocity, target.knockBackResist+0.1f);
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockBack, ref bool crit, ref int hitDirection){
            if(target.HasBuff<YoteEffect>()){
                Main.player[projectile.owner].position+=projectile.velocity*3;
                damage*=3;
                projectile.Kill();
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor){
            return false;
        }
    }
}