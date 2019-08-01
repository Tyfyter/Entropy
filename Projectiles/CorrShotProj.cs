using System;
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

    public class CorrShotProj : EntModProjectile
    {
        public override string Texture => "Terraria/Projectile_207";
        public override void SetDefaults(){
            projectile.CloneDefaults(14);
            projectile.friendly = true;
            projectile.magic = true;
            projectile.ranged = true;
            projectile.tileCollide = true;
            projectile.timeLeft = 900;
            projectile.extraUpdates = 9;
            projectile.ignoreWater = true;   
            projectile.alpha = 150;
            projectile.aiStyle = 0;
            projectile.light = 0;
			dmgratio = dmgratiobase = new float[15] {0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 0f, 0f};
        }
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Crystal Shard"); Original name
			DisplayName.SetDefault("Magnus Kirudo");
		}
        public override void AI(){
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;  
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockBack, ref bool crit, ref int hitDirection){
            AddBuff(new CorrEffect(target, (int)(damage*0.75f), damage>66?(int)(damage*0.015f):1));
			EntropyPlayer modPlayer = Main.player[projectile.owner].GetModPlayer<EntropyPlayer>(mod);
			modPlayer.comboadd(1);
            base.ModifyHitNPC(target, ref damage, ref knockBack, ref crit, ref hitDirection);
        }
		public override bool PreDraw (SpriteBatch spriteBatch, Color lightColor)
		{
            lightColor = Color.LimeGreen;
			for(int i = 0; i < 6; i++){
			    Dust.NewDustPerfect(projectile.Center-((projectile.velocity/6)*(i+1)), 264, projectile.velocity*0.5f, 0, Color.LimeGreen, 0.5f).noGravity = true;
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