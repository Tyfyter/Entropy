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

    public class VoxAbility : EntModProjectile{
		public override string Texture => "Entropy/Items/Mods/ComboSpeed";
        public override void SetDefaults(){
            projectile.width = projectile.height = 160;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.timeLeft = 5;
            projectile.extraUpdates = 4;
            projectile.penetrate = -1;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
			dmgratio = dmgratiobase = new float[15]{0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 1f, 0f};
        }
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Vox");
		}
        public override void AI(){
            if(projectile.timeLeft==0)return;
            for (int y = -10; y < 10; y++){
                Vector2 velocity = projectile.velocity.SafeNormalize(new Vector2(160/projectile.timeLeft,0)).RotatedBy(MathHelper.ToRadians(y*6));
                Dust d = Dust.NewDustPerfect(projectile.Center+velocity, 267, null, 0, Color.DodgerBlue, 0.6f);
                d.velocity = velocity*4*(5-projectile.timeLeft);
                //d.position -= d.velocity * 8;
                d.fadeIn = 0.7f;
                d.noGravity = true;
            }
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockBack, ref bool crit, ref int hitDirection){
            AddBuff(new BlastEffect(target, 60));
            if(Main.player[projectile.owner].HasBuff(BuffID.Hunter)){
                damage = (int)(damage*1.25f);
                crit = true;
            }
            base.ModifyHitNPC(target, ref damage, ref knockBack, ref crit, ref hitDirection);
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox){
            float f = (5-projectile.timeLeft)/5f;
            hitbox.Inflate((int)(-hitbox.Width*f),(int)(-hitbox.Height*f));
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor){
            return false;
        }
    }
}