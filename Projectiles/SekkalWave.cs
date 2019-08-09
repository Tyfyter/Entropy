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

    public class SekkalWave : EntModProjectile {
        public override string Texture => "Terraria/Projectile_684";
        public override void SetDefaults(){
            projectile.CloneDefaults(ProjectileID.DD2SquireSonicBoom);
            projectile.width = 48;
            projectile.height = 80;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 30;
            projectile.extraUpdates = 5;
            projectile.ignoreWater = true;
            aiType = ProjectileID.DD2SquireSonicBoom;
			dmgratio = dmgratiobase = new float[15]{0f, 1f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f};
        }
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Sekkal Wave");
		}
        public override void AI(){
            //projectile.velocity.Y-=0.025f;
            Dust d = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 267, 0, 0, 0, Color.Goldenrod, 0.6f);
            d.velocity = new Vector2(projectile.velocity.X*-2, projectile.velocity.Y*-2);
            d.position -= d.velocity * 8;
            d.fadeIn = 0.7f;
            d.noGravity = true;
        }
    }
}