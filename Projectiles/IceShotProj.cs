using System;
using Entropy.Buffs;
using Entropy.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Entropy.NPCs.EntropyGlobalNPC;

namespace Entropy.Projectiles{

    public class IceShotProj : EntModProjectile
    {
        public override string Texture => "Terraria/Projectile_638";
        public override void SetDefaults(){
            projectile.CloneDefaults(14);
            projectile.friendly = true;
            projectile.magic = true;
            projectile.ranged = true;
            projectile.tileCollide = true;
            projectile.penetrate = 2;
            projectile.timeLeft = 900;
            projectile.extraUpdates = 3;
            projectile.ignoreWater = true;   
            projectile.alpha = 150;
            projectile.aiStyle = 0;
			dmgratio = dmgratiobase = new float[15] {0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f};
        }
		public override void SetStaticDefaults(){
			//DisplayName.SetDefault("Crystal Shard"); Original name
			DisplayName.SetDefault("Ice Bullet");
		}
        public override void AI(){
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;  
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit){
            AddBuff(new ColdEffect(target, (target.boss?60:360)/(target.HasBuff<ColdEffect>()?2:1), 4-(Math.Max((target.CountBuff<ColdEffect>()),1))).withColor(new Color(0,235,255)));
            if(projectile.penetrate<=1){
                if(Main.player[projectile.owner].HeldItem!=null){
                    Ice_Revolver ir = (Main.player[projectile.owner].HeldItem.modItem as Ice_Revolver);
                    if(ir!=null&&ir.RoundsLeft<Ice_Revolver.RoundsMax)ir.RoundsLeft++;
                }
            }
        }
		public override bool PreDraw (SpriteBatch spriteBatch, Color lightColor){
            lightColor = Color.Cyan;
			for(int i = 0; i < 3; i++){
			    Dust.NewDustPerfect(projectile.Center, 264, projectile.velocity*-0.5f, 0, Color.Cyan, 0.5f).noGravity = true;
			}
			return true;
		}
		/* public class ColdEffect : BuffBase{
			int rate = 1;
			public override Color? color => new Color(0,235,255);
			//new string[15] {"Slash", "Impact", "Puncture", "Cold", "Electric", "Heat", "Toxic", "Blast", "Corrosive", "Gas", "Magnetic", "Radiation", "Viral", "True", "Void"}
			public ColdEffect(NPC npc, int duration, int rate = 3) : base(npc){
				this.duration = duration;
				this.rate = rate;
			}
			public override bool PreUpdate(NPC npc, bool canceled){
				if(canceled&&duration%rate==0)duration++;
				return duration%rate==0;
			}
        } */
    }
}