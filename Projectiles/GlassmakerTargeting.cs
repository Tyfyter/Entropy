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

    public class GlassmakerTargeting : EntModProjectile{
		public override string Texture => "Entropy/Items/Mods/ComboSpeed";
        int hits = 0;
        public override void SetDefaults(){
            projectile.width = projectile.height = 1;
            projectile.magic = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.timeLeft = 120;
            projectile.extraUpdates = 3;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 120;
			dmgratio = dmgratiobase = new float[15]{0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 1f, 0f};
        }
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Glassmaker Blast");
		}
        public override void AI(){
            Main.player[projectile.owner].GetModPlayer<EntropyPlayer>().WorldonFiretime = projectile.timeLeft/2;
            /*for (int y = 0; y < 60; y++){
                Vector2 velocity = new Vector2((120-projectile.timeLeft)*8,0).RotatedBy(MathHelper.ToRadians(y*6-projectile.timeLeft));
                //Point pos = (projectile.Center+velocity).ToWorldCoordinates().ToPoint();
                //if(Main.tile[pos.X,pos.Y].collisionType<=0)continue;
                Dust d = Dust.NewDustPerfect(projectile.Center+velocity, 267, null, 0, Color.OrangeRed, 0.6f);
                //d.velocity = velocity.SafeNormalize(Vector2.Zero)*34;
                //d.position -= d.velocity * 8;
                d.noGravity = true;
            }*/
        }
        public override bool? CanHitNPC(NPC target){
            Player player = Main.player[projectile.owner];
            EntropyPlayer modPlayer = player.GetModPlayer<EntropyPlayer>();
            Vector2 pos = projectile.Center.constrained(target.TopLeft,target.BottomRight);
            int dist = (120-projectile.timeLeft)*8;
            Vector2 targetPos = target.Center;
            if(!target.friendly && (pos-projectile.Center).Length()<=dist && ((pos-targetPos-targetPos)-projectile.Center).Length()>=dist && projectile.localNPCImmunity[target.whoAmI]==0){
                if(!player.CheckMana(Entropy.ArbitraryCinder, (int)(40*player.manaCost), true, hits++>(player.statManaMax2/(40*player.manaCost))*5)){
                    projectile.Kill();
                    return false;
                }
                if(target.aiStyle != 6 && target.aiStyle != 37)AddBuff(new BlastEffect(target, target.boss?5:30));
                if(!target.buffImmune[BuffID.CursedInferno] && (!target.HasBuff<HeatEffect>()||Main.rand.Next(6)==0))AddBuff(new HeatEffect(target, projectile.damage/(3+target.CountBuff<HeatEffect>()), 180, 7, Main.player[projectile.owner], false));
                Projectile p = Projectile.NewProjectileDirect(target.Center, Vector2.Zero, ProjectileID.SolarWhipSwordExplosion, projectile.damage, 4, projectile.owner);
                p.direction = target.Center.X<projectile.Center.X?-1:1;
                p.magic = true;
                target.velocity.Y-=(target.knockBackResist+0.25f)*12;
			    Main.PlaySound(2, (int)pos.X, (int)pos.Y, 34, 1f, -0.35f);
                projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown;
                if(modPlayer.inferno>0)modPlayer.inferno = Math.Max(modPlayer.inferno-30,1);
                return true;
            }
            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox){
            return false;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor){
            return false;
        }
    }
}