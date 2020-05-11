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
using Enrtopy.Items;
using Microsoft.Xna.Framework.Audio;

namespace Entropy.Projectiles{

    public class NarulDash : EntModProjectile {
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
            projectile.timeLeft = 20;
            projectile.extraUpdates = 3;
            projectile.ignoreWater = true;
            projectile.penetrate = 3;
            projectile.alpha = 100;
			dmgratio = dmgratiobase = new float[15]{0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 1f};
            statchance = 1;
        }
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Narül");
		}
        public override bool OnTileCollide(Vector2 oldVelocity){return Math.Abs(projectile.velocity.ToRotation()-oldVelocity.ToRotation())>0.05f;}
        /* public override void OnHitNPC(NPC target, int damage, float knockback, bool crit){
            projectile.Kill();
        } */
        public override void AI(){
            //projectile.velocity.Y-=0.025f;
            Dust d = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 267, newColor:Color.Goldenrod, Scale:0.6f);
            d.velocity = new Vector2(projectile.velocity.X/2, projectile.velocity.Y/2);
            d.fadeIn = 0.7f;
            d.noGravity = true;
            int num2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 159, 0f, 0f, 0, default(Color), 1f);
            Main.dust[num2].scale = (float)Main.rand.Next(20, 70) * 0.01f;
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockBack, ref bool crit, ref int hitDirection){
            AddBuff(new YoteEffect(target, 15));
            target.velocity = lerp(projectile.velocity*3, target.velocity, 0.75f);
            knockBack = 0;
            Player player = Main.player[projectile.owner];
		    TeleportEffect(player.getRect());
            player.Teleport(projectile.position, 5);
		    TeleportEffect(player.getRect());
            player.immuneTime+=15;
            player.immune = true;
            player.GetModPlayer<EntropyPlayer>().comboadd(3);
            /*int s = mod.GetSoundSlot(SoundType.Item, "Entropy/Sounds/Items/NarulSound");
            Main.NewText(Entropy.mod.ns.ToString());
            Main.NewText(s);
            //Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y, s);*/
            SoundEffectInstance n = null;
            Main.PlaySoundInstance(Entropy.mod.ns.PlaySound(ref n, 1, 0, SoundType.Item));
            projectile.Kill();
            base.ModifyHitNPC(target, ref damage, ref knockBack, ref crit, ref hitDirection);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor){
            return false;
        }
        static void TeleportEffect(Rectangle effectRect){
            int num = effectRect.Width * effectRect.Height / 5;
            for (int i = 0; i < num; i++)
            {
                int num2 = Dust.NewDust(new Vector2((float)effectRect.X, (float)effectRect.Y), effectRect.Width, effectRect.Height, 159, 0f, 0f, 0, default(Color), 1f);
                Main.dust[num2].scale = (float)Main.rand.Next(20, 70) * 0.01f;
                if (i < 10)
                {
                    Main.dust[num2].scale += 0.25f;
                }
                if (i < 5)
                {
                    Main.dust[num2].scale += 0.25f;
                }
            }
            return;
        }
    }
}