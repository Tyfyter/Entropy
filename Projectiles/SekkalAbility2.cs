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

    public class SekkalAbility2 : EntModProjectile{
        Color? elCol;
        public Color ElColor {
            get{
                if(elCol.HasValue)return elCol.Value;
                if(dmgratio[3]>0){
                    elCol = new Color(100,235,255);
                }else if(dmgratio[4]>0){
                    elCol = Color.DarkViolet;
                }else if(dmgratio[5]>0){
                    elCol = Color.OrangeRed;
                }else if(dmgratio[6]>0){
                    elCol = Color.DarkGreen;
                }
                return (elCol??Color.White);
            }
        }
		public override string Texture => "Entropy/Items/Mods/ComboSpeed";
        public override void SetDefaults(){
            projectile.width = projectile.height = 480;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.timeLeft = 1;
            projectile.penetrate = -1;
            projectile.ignoreWater = true;
			dmgratio = dmgratiobase = new float[15]{0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f};
            statchance = 100;
        }
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Sekkal");
		}
        public override void AI(){
            float f = 6;
            if(Main.rand.NextBool())f = -6;
            int t = (int)Main.time;
            for (int y = t; y < t+60; y++){
                Vector2 velocity = new Vector2(((y&3)<<(y%3)),0).RotatedBy(MathHelper.ToRadians(y*f));
                Dust d = Dust.NewDustPerfect(projectile.Center+velocity, 267, null, 0, ElColor, 0.6f);
                d.velocity = velocity*2;
                //d.position -= d.velocity * 8;
                d.fadeIn = 0.7f;
                d.noGravity = true;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor){
            return false;
        }
    }
}