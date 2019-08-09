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

    public class ValhallaArrow : EntModProjectile{
        public override void SetDefaults(){
            projectile.CloneDefaults(474);
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.tileCollide = true;
            projectile.timeLeft = 900;
            projectile.extraUpdates = 3;
            projectile.ignoreWater = true;   
            aiType = 474;
			dmgratio = dmgratiobase = new float[15]{0.8f, 0.1f, 0.1f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f};
        }
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Valhalla Arrow");
		}
        public override bool PreKill(int timeLeft){
            return false;
        }
        public override void AI(){
            projectile.velocity.Y-=0.025f;
            Dust d = Dust.NewDustPerfect(projectile.Center, 267, null, 0, Color.GhostWhite, 0.6f);
            d.velocity = new Vector2(projectile.velocity.X * -2, projectile.velocity.Y * -2);
            d.position -= d.velocity * 8;
            d.fadeIn = 0.7f;
            d.noGravity = true;
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockBack, ref bool crit, ref int hitDirection){
            if(target.HasBuff<BlastEffect>())damage*=2;
            base.ModifyHitNPC(target, ref damage, ref knockBack, ref crit, ref hitDirection);
        }
    }
}