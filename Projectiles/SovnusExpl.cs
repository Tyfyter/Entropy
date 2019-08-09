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

    public class SovnusExpl : EntModProjectile{
		public override string Texture => "Entropy/Items/Mods/ComboSpeed";
        public override void SetDefaults(){
            projectile.width = projectile.height = 240;
            projectile.friendly = true;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 1;
            projectile.extraUpdates = 3;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
			dmgratio = dmgratiobase = new float[15]{0f, 0f, 0f, 0f, 0f, 0.8f, 0f, 0.2f, 0f, 0f, 0f, 0f, 0f, 0f, 0f};
        }
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Sovnus Grenade");
		}
        public override void AI(){
            
            float f = 6;
            if(Main.rand.NextBool())f = -6;
            int t = (int)Main.time;
            for (int y = t; y < t+60; y++){
                Vector2 velocity = new Vector2(((y&3)<<(y%3))/2+6,0).RotatedBy(MathHelper.ToRadians(y*f));
                Dust d = Dust.NewDustPerfect(projectile.Center+velocity, 267, null, 0, Color.DodgerBlue, 0.6f);
                d.velocity = velocity*2;
                //d.position -= d.velocity * 8;
                d.fadeIn = 0.7f;
                d.noGravity = true;
            }
        }
        public override bool? CanHitNPC(NPC target){
            if(target.friendly)return false;
            return base.CanHitNPC(target);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor){
            return false;
        }
    }
}