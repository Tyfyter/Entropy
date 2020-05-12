using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using Entropy.Projectiles;
using Entropy.Buffs;
using static Entropy.NPCs.EntropyGlobalNPC;
using Terraria.Localization;

namespace Entropy.Items{
	//Maraṇānantara is a bit long, no?
	public class Narul : CompModItem{
		public override string Texture => "Entropy/Items/Narul";
		public override int maxabilities => 4;
		public override bool realCombo => true;
		public override bool isGun => true;
		int time = 0;
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Narül");
			Tooltip.SetDefault("You will find no greater power than the simple thought of your own name, inscribed upon a grave.\n-Aurdeorum");
		}
		public override void SetDefaults() {
			item.damage = 50;//realdmg = dmgbase = 140;
			statchance = basestat = 17;
			realcrit = basecrit = 36;
			//item.ranged = mode == 0;
			//item.melee = !item.ranged;
			item.width = 62;
			item.height = 18;
			//item.useTime = mode==2?15:mode==0?17:20;
			//item.useAnimation = mode==2?15:mode==0?17:20;
			//item.useStyle = mode == 0?5:1;
			//item.knockBack = mode==1?12:6;
			item.value = 10000;
			item.rare = 2;
			item.useTime = 23;
			item.useAnimation = 23;
			//item.UseSound = SoundID.Item1;
			item.autoReuse = false;
			item.useTurn = false;
			item.ranged = true;
			item.noMelee = true;
			item.noUseGraphic = false;
			item.shootSpeed = 13.5f;
			item.useStyle = 5;
			item.useAmmo = AmmoID.Bullet;
			item.shoot = ModContent.ProjectileType<NarulSpread>();
			dmgratio = dmgratiobase = new float[15]{0.85f, 0f, 0.1f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.05f};
		}
		public override void PostSetDefaults(Player player){
			if(player.altFunctionUse==2){
				if(ability != 0){
					item.noUseGraphic = true;
					return;
				}
				dmgratio = dmgratiobase = new float[15]{0.05f, 0.4f, 0.05f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f};
			}
		}
		public override Vector2? HoldoutOffset(){
			return new Vector2(-12,4);
		}
		public override void tryScroll(int dir){
			ability=(ability+dir+4)%4;
		}
		public override void HoldItem(Player player){
			base.HoldItem(player);
			if(time>0)time--;
		}
		public override bool AltFunctionUse(Player player){
			return true;
		}
		public override bool CanUseItem(Player player){
			if(!base.CanUseItem(player))return false;
			if(player.altFunctionUse==2){
				if(ability == 0){
					//realdmg = dmgbase = 140;
					statchance = basestat = 45;
					realcrit = basecrit = 12;
					return true;
				}
				if(ability == 1){
					//CastAbility(1);
					item.noUseGraphic = true;
					return player.CheckMana(50, true);
				}
				if(time==0)CastAbility(ability);
				time = 3;
				return false;
			}
			return true;
		}
		public void CastAbility(int i){
			Player player = Main.player[item.owner];
			switch(i){
				/* case 1:
				if(!player.CheckMana(50, true))return;
				Projectile.NewProjectile(player.Center, (Main.MouseWorld-player.Center).SafeNormalize(new Vector2())*7.5f, ModContent.ProjectileType<VoxAbility>(), realdmg, 15, player.whoAmI);
				Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y, 38, pitchOffset:-0.55f);
				break; */
				case 2:
				if(!player.CheckMana(75, true))return;
				Projectile.NewProjectile(player.Center, (Main.MouseWorld-player.Center).SafeNormalize(new Vector2())*7.5f, ModContent.ProjectileType<SovnusAbility>(), realdmg/3, 1, player.whoAmI);
				Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y, 8, pitchOffset:-0.55f);
				break;
				case 3:
				if(!player.CheckMana(75, true))return;
				Projectile.NewProjectile(player.Center, (Main.MouseWorld-player.Center).SafeNormalize(new Vector2())*7.5f, ModContent.ProjectileType<SovnusAbility>(), realdmg, 15, player.whoAmI, 1);
				Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y, 8, pitchOffset:-0.55f);
				break;
			}
		}
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
			if(player.altFunctionUse==2){
				if(ability == 1){
					type = ModContent.ProjectileType<NarulDash>();
					damage*=2;
					knockBack*=3;
					position = player.position;
					int proj = Projectile.NewProjectile(position, new Vector2(speedX,speedY), type, damage, knockBack, item.owner);
					Main.projectile[proj].maxPenetrate += punchthrough;
					Main.projectile[proj].penetrate += punchthrough;
					EntModProjectile p = Main.projectile[proj].modProjectile as EntModProjectile;
					if(p!=null){
						p.critcombo = critcomboboost;
					}
					return false;
				}
				type = ModContent.ProjectileType<SovnusSlug>();
				return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
				//Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y, 38, pitchOffset:0f);
			}else{
				type = ModContent.ProjectileType<NarulSpread>();
				Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y, 38, pitchOffset:0.15f).Volume = 0.65f;
			}
			damage/=5; 
			Vector2 vec = new Vector2(speedX, speedY);
			position+=vec;
			for (int i = 0; i < 6; i++){
				Vector2 vec2 = vec.RotatedByRandom(0.07f);
				speedX = vec2.X;
				speedY = vec2.Y;
				//Vector2 pos = position + (vec/2).RotatedBy((Math.PI/3)*(i-3));
				base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
			}
			return false;
		}
	}
	/* public class Valhalla2 : Valhalla{
		int[] modsobsolete = new int[8] {6,3,0,0,0,0,0,0};
		int[] modlevelsobsolete = new int[8] {0,0,0,0,0,0,0,0};
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Valhalla");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults() {
			item.type = ModContent.ItemType("Trinity1");
			item.SetDefaults(item.type);
			item.damage = 50;
			item.melee = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = 1;
			item.knockBack = 6;
			item.value = 10000;
			item.rare = 2;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			dmgratio = dmgratiobase = new float[15] {0.06f,0.88f,0.06f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f};
			critDMG = 2;
			statchance = basestat = 25;
		}

		public override void HoldItem(Player player){
			SetDefaults();
			for(int i = 0; i < modsobsolete.Length; i++){
				Entropy.ModEffectobsolete(modsobsolete[i], modlevelsobsolete[i]);
			}
		}
		public override bool CanRightClick(){
			item.type = ModContent.ItemType<Trinity3>();
			return false;
		}
	}
	public class Valhalla3 : Valhalla{
		int[] modsobsolete = new int[8] {6,3,0,0,0,0,0,0};
		int[] modlevelsobsolete = new int[8] {0,0,0,0,0,0,0,0};
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Valhalla");
			Tooltip.SetDefault("");
			Item.staff[item.type] = true;
		}
		public override void SetDefaults() {
			item.type = ModContent.ItemType("Trinity1");
			item.SetDefaults(item.type);
			item.damage = 50;
			item.melee = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = 1;
			item.knockBack = 6;
			item.value = 10000;
			item.rare = 2;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			dmgratio = dmgratiobase = new float[15] {0.06f,0.06f,0.88f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f};
			critDMG = 2;
			statchance = basestat = 25;
		}

		public override bool CanRightClick(){
			item.type = ModContent.ItemType<Trinity1>();
			return false;
		}
	} */
}
