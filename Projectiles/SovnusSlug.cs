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

    public class SovnusSlug : EntModProjectile {
        public override string Texture => "Terraria/Projectile_55";
        public override void SetDefaults(){
            projectile.CloneDefaults(ProjectileID.Stinger);
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.tileCollide = true;
            projectile.timeLeft = 30;
            projectile.extraUpdates = 4;
            projectile.ignoreWater = true;
            projectile.penetrate = 1;
            projectile.alpha = 100;
			dmgratio = dmgratiobase = new float[15]{0f, 0.5f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f};
        }
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Sovnus Grenade");
		}
        public override bool OnTileCollide(Vector2 oldVelocity){
            return true;
        }
        public override void Kill(int timeLeft){
            int proj = Projectile.NewProjectile(projectile.Center, new Vector2(), mod.ProjectileType<SovnusExpl>(), projectile.damage, projectile.knockBack, projectile.owner);
            EntModProjectile expl = Main.projectile[proj].modProjectile as EntModProjectile;
            if(expl!=null){
                expl.critDMG = critDMG;
                expl.statchance = statchance;
                expl.dmgratio = dmgratio.Sum(new float[15]{-0.05f, -0.4f, -0.05f, 0f, 0f, 0f, 0f, 0.2f, 0f, 0f, 0f, 0f, 0f, 0f, 0f});
                expl.addElement(5, 0.8f);
                if(critcombo!=0)expl.critcombo += Math.Sign(critcombo);
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit){
            projectile.Kill();
        }
        public override void AI(){
            //projectile.velocity.Y-=0.025f;
            Dust d = Dust.NewDustPerfect(projectile.Center, 267, null, 0, Color.DodgerBlue, 0.6f);
            d.velocity = new Vector2(projectile.velocity.X/2, projectile.velocity.Y/2);
            d.fadeIn = 0.7f;
            d.noGravity = true;
        }
    }
}