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

    public class SovnusDash : EntModProjectile {
        public override string Texture => "Terraria/Projectile_55";
        public override void SetDefaults(){
            projectile.CloneDefaults(ProjectileID.Stinger);
            projectile.width = 32;
            projectile.height = 48;
            projectile.aiStyle = 0;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.tileCollide = true;
            projectile.timeLeft = 30;
            projectile.extraUpdates = 29;
            projectile.ignoreWater = true;
            projectile.penetrate = 1;
            projectile.alpha = 100;
			dmgratio = dmgratiobase = new float[15]{0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 1f, 0f};
        }
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Sovnus Dash");
		}
        public override bool OnTileCollide(Vector2 oldVelocity){
            bool f = Math.Abs(projectile.velocity.ToRotation()-oldVelocity.ToRotation())>0.1f;
            if(f&&projectile.timeLeft>15){
                Projectile.NewProjectileDirect(projectile.Center, new Vector2(), ModContent.ProjectileType<SovnusExpl>(), projectile.damage/2, projectile.knockBack, projectile.owner).hostile = false;
                //Main.player[projectile.owner].velocity+=(projectile.velocity-oldVelocity) * new Vector2(0.5f, 1.5f);
            }
            return f;
        }
        public override void Kill(int timeLeft){
            Player player = Main.player[projectile.owner];
            //player.Center = projectile.Center;
		    TeleportEffect(player.getRect());
            player.Teleport(projectile.position, 5);
		    TeleportEffect(player.getRect());
            player.immuneTime+=15;
            player.immune = true;
            if(timeLeft>0){
                if(player.itemAnimation>7)player.itemAnimation = 7;
            }
            /* int proj = Projectile.NewProjectile(projectile.Center, new Vector2(), ModContent.ProjectileType<SovnusExpl>(), projectile.damage, projectile.knockBack, projectile.owner);
            EntModProjectile expl = Main.projectile[proj].modProjectile as EntModProjectile;
            if(expl!=null){
                expl.critDMG = critDMG;
                expl.statchance = statchance;
                expl.dmgratio = dmgratio.Sum(new float[15]{-0.05f, -0.4f, -0.05f, 0f, 0f, 0f, 0f, 0.2f, 0f, 0f, 0f, 0f, 0f, 0f, 0f});
                expl.addElement(5, 0.8f);
                if(critcombo!=0)expl.critcombo += Math.Sign(critcombo);
            } */
        } 
        /* public override void OnHitNPC(NPC target, int damage, float knockback, bool crit){
            projectile.Kill();
        } */
        public override void AI(){
            //projectile.velocity.Y-=0.025f;
            Dust d = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 267, newColor:Color.DodgerBlue, Scale:0.6f);
            d.velocity = new Vector2(projectile.velocity.X/2, projectile.velocity.Y/2);
            d.fadeIn = 0.7f;
            d.noGravity = true;
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockBack, ref bool crit, ref int hitDirection){
            AddBuff(new BlastEffect(target, 60));
            target.velocity = lerp(projectile.velocity*2, target.velocity, 0.75f);
            knockBack = 0;
            Main.player[projectile.owner].GetModPlayer<EntropyPlayer>().comboadd(1);
            base.ModifyHitNPC(target, ref damage, ref knockBack, ref crit, ref hitDirection);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor){
            return false;
        }
        static void TeleportEffect(Rectangle effectRect){
            for (int l = 0; l < 50; l++){
                int num5 = Dust.NewDust(new Vector2((float)effectRect.X, (float)effectRect.Y), effectRect.Width, effectRect.Height, 180, 0f, 0f, 0, default(Color), 1f);
                Main.dust[num5].noGravity = true;
                for (int m = 0; m < 5; m++)
                {
                    if (Main.rand.Next(3) == 0)
                    {
                        Main.dust[num5].velocity *= 0.75f;
                    }
                }
                if (Main.rand.Next(3) == 0)
                {
                    Main.dust[num5].velocity *= 2f;
                    Main.dust[num5].scale *= 1.2f;
                }
                if (Main.rand.Next(3) == 0)
                {
                    Main.dust[num5].velocity *= 2f;
                    Main.dust[num5].scale *= 1.2f;
                }
                if (Main.rand.Next(2) == 0)
                {
                    Main.dust[num5].fadeIn = (float)Main.rand.Next(75, 100) * 0.01f;
                    Main.dust[num5].scale = (float)Main.rand.Next(25, 75) * 0.01f;
                }
                Main.dust[num5].scale *= 0.8f;
            }
        }
    }
}