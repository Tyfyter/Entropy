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
using static Entropy.EntropyExt;

namespace Entropy.Projectiles{

    public class GlassmakerBlast : EntModProjectile{
		public override string Texture => "Entropy/Items/Mods/ComboSpeed";
        public override void SetDefaults(){
            projectile.width = projectile.height = 1;
            projectile.magic = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.timeLeft = 120;
            projectile.extraUpdates = 3;
            projectile.ignoreWater = true;
            projectile.penetrate = 15;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 90;
			dmgratio = dmgratiobase = new float[15]{0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 1f, 0f};
        }
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Glassmaker Blast");
		}
        public override void AI(){
            for (int y = 0; y < 60; y++){
                Vector2 velocity = new Vector2((120-projectile.timeLeft)*8,0).RotatedBy(MathHelper.ToRadians(y*6-projectile.timeLeft));
                //Point pos = (projectile.Center+velocity).ToWorldCoordinates().ToPoint();
                //if(Main.tile[pos.X,pos.Y].collisionType<=0)continue;
                Dust d = Dust.NewDustPerfect(projectile.Center+velocity, 267, null, 0, Color.OrangeRed, 0.6f);
                d.velocity = velocity.SafeNormalize(Vector2.Zero)*28;
                //d.position -= d.velocity * 8;
                d.fadeIn = 0.7f;
                d.noGravity = true;
            }
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox){
            Vector2 pos = projectile.Center.constrained(targetHitbox.TopLeft(),targetHitbox.BottomRight());
            int dist = (120-projectile.timeLeft)*8;
            Vector2 target = targetHitbox.Center.ToVector2();
            if((pos-projectile.Center).Length()<=dist && ((pos-target-target)-projectile.Center).Length()>=dist){
                return true;
            }
            return false;
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockBack, ref bool crit, ref int hitDirection){
            hitDirection = target.Center.X<projectile.Center.X?-1:1;
            AddBuff(new BlastEffect(target, target.boss?5:10));
            if(!target.buffImmune[BuffID.CursedInferno] && (!target.HasBuff<HeatEffect>()||Main.rand.Next(6)==0))AddBuff(new HeatEffect(target, damage/(3+target.CountBuff<HeatEffect>()), 180, 7, Main.player[projectile.owner], false));
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor){
            return false;
        }
    }
}