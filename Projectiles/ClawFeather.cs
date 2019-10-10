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

    public class ClawFeather : EntModProjectile{
        List<Vector2> oldPositions = new List<Vector2>(){};
        public override void SetDefaults(){
            projectile.CloneDefaults(38);
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.melee = true;
            projectile.thrown = true;
            projectile.tileCollide = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 900;
            projectile.extraUpdates = 5;
            projectile.ignoreWater = true;   
            projectile.aiStyle = 0;
			dmgratio = dmgratiobase = new float[15]{0.9f, 0f, 0.1f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f};
        }
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Feathered Claw");
            ProjectileID.Sets.TrailingMode[projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 16;
		}
        public override void AI(){
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
        }
        public override bool PreAI(){
            oldPositions.Add(projectile.position);
            if(oldPositions.Count>6){
                oldPositions.RemoveAt(6);
            }
            return true;
        }
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor){
            spriteBatch.Draw(mod.GetTexture("Projectiles/ClawFeather"), oldPositions[oldPositions.Count-1], null, lightColor, projectile.rotation, projectile.Center, 1, SpriteEffects.None, 0);
			return true;
		}
    }
}