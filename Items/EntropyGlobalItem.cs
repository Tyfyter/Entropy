/*
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Entropy.Items{
	public class EntropyGlobalItem : GlobalItem{
		//*
		public override void ModifyHitNPC(Item item, Player player, NPC target, ref int damage, ref float knockBack, ref bool crit){
				if(crit){
					damage = (int)(damage/2);
				}
				int cc = item.crit;
				int clev = 0;
				if(item.melee)cc+=player.meleeCrit;
				if(item.thrown)cc+=player.thrownCrit;
				if(item.magic)cc+=player.magicCrit;
				if(item.ranged)cc+=player.rangedCrit;
				for(int i = cc; i > 0; i-=100){
					if(i < 100){
						if(i < Main.rand.Next(0,100))break;
						if(clev++>=1){
							for(int i2 = 0; i2 < 5; i2++)Dust.NewDust(target.Center, 0, 0, 31, newColor:clev>2?Color.Red:Color.OrangeRed);
						}
					}
					damage *= 2;
				}
		}//* /
		public override bool Shoot(Item item, Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
			return type != ModContent.ProjectileType("FakeGrapple");
		}
	}
}// */