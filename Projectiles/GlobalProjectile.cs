using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Entropy.Projectiles{
	public class EntropyGlobalProjectile : GlobalProjectile{
		/*public override void ModifyHitNPC(Terraria.Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection){
			if(target.modNPC != null)if((target.modNPC.GetType()+"").Contains("CalamityMod.NPCs.Yharon")){
				damage = damage+(int)(target.lifeMax*0.0025);
			}
			//if(projectile.modProjectile.mod.DisplayName == "Light" || projectile.modProjectile.mod.DisplayName == "elemental" || projectile.modProjectile.mod.DisplayName == "Reference The Gungeon" || projectile.modProjectile.mod.DisplayName.Contains("Epik")){
				Player player = Main.player[projectile.owner];
				if(crit){
					damage = (int)(damage/2);
				}
				int cc = 0;
				int clev = 0;
                if (projectile.melee)cc+=player.meleeCrit;
				if(projectile.thrown)cc+=player.thrownCrit;
				if(projectile.magic)cc+=player.magicCrit;
				if(projectile.ranged)cc+=player.rangedCrit;
				for(int i = cc; i > 0; i-=100){
					if(i < 100){
						if(i < Main.rand.Next(0,100))break;
						if(clev++>=1){
							for(int i2 = 0; i2 < 5; i2++)Dust.NewDust(target.Center, 0, 0, 31, newColor:clev>2?Color.Red:Color.OrangeRed);
						}
					}
					damage *= 2;
				}
			//}
		}*/
	}
}
//*/