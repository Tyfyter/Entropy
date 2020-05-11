using System;
using System.Collections.Generic;
using Entropy.Buffs;
using Entropy.Items;
using Entropy.NPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Entropy.NPCs.EntropyGlobalNPC;

namespace Entropy.Projectiles{

    public class Glassmaker_P : EntModProjectile
    {
        public override string Texture => "Terraria/Projectile_638";
        float heat;
        public override void SetDefaults(){
            projectile.CloneDefaults(14);
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.tileCollide = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 600;
            projectile.extraUpdates = 3;
            projectile.ignoreWater = false;   
            projectile.alpha = 150;
            projectile.aiStyle = 1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 6;
			dmgratio = dmgratiobase = new float[15] {0f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f};
        }
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Glassmaker");
		}
        public override void AI(){
            if(projectile.wet && projectile.timeLeft%8==0){
			    Dust.NewDust(projectile.Center-new Vector2(3,3), 6, 6, 34, 0, 0, 0);
            }
            Dust.NewDustDirect(projectile.Center-new Vector2(3,3), 6, 6, 174, 0, 0, 0, new Color(230, 61, 0), 0.5f).noGravity = projectile.wet;
        }
        public override bool OnTileCollide(Vector2 oldVelocity){
            projectile.velocity*=0;
            projectile.position-=oldVelocity/2;
            projectile.tileCollide = false;
            projectile.penetrate = 25;
            projectile.aiStyle = 0;
            projectile.timeLeft/=2;
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit){
            HeatEffect buff = target.GetBuff<HeatEffect>();
            if(buff.duration<360)buff.duration = 360;
            //if(buff.rate>7&&crit)buff.rate--;
            if(!target.buffImmune[BuffID.CursedInferno] && Main.rand.Next(6)==0)AddBuff(new HeatEffect(target, damage/(7+target.CountBuff<HeatEffect>()), 360, 10, Main.player[projectile.owner]));
        }
        public override void PreProc(){
            heat = dmgratio[5];
            dmgratio[5] = 0;
        }
        public override void PostProc(){
            dmgratio[5] = heat;
        }
		public override bool PreDraw (SpriteBatch spriteBatch, Color lightColor){
			/*for(int i = 0; i < 3; i++){
			    Dust.NewDust(projectile.Center-new Vector2(3,3), 6, 6, 174, 0, 0, 0, new Color(230, 61, 0), 0.5f);
			}*/
			return false;
		}
    }
}