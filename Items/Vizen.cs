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
	//Abilities:
	//Shield
	//Slash
	//Renewal
	//Smite
	public class Vizen : CompModItem{
		public override string Texture => "Entropy/Items/Vizen";
		public override int maxabilities => 4;
		public override bool realCombo => true;
		public override bool isGun => true;
		int time = 0;
        public static short customGlowMask = 0;
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Vizen");
			Tooltip.SetDefault("A brutal thorn...\n-Aurdeorum");
          	customGlowMask = Entropy.SetStaticDefaultsGlowMask(this);
		}
		public override void SetDefaults() {
			item.damage = 20;//realdmg = dmgbase = 50;
			statchance = basestat = 27;
			realcrit = basecrit = 30;
			//item.ranged = mode == 0;
			//item.melee = !item.ranged;
			item.width = 59;
			item.height = 20;
			//item.useTime = mode==2?15:mode==0?17:20;
			//item.useAnimation = mode==2?15:mode==0?17:20;
			//item.useStyle = mode == 0?5:1;
			//item.knockBack = mode==1?12:6;
			item.value = 10000;
			item.rare = 2;
			item.useTime = 7;
			item.useAnimation = 14;
			//item.UseSound = SoundID.Item1;
			item.autoReuse = false;
			item.useTurn = false;
			item.ranged = true;
			item.noMelee = true;
			item.noUseGraphic = false;
			item.shootSpeed = 13.5f;
			item.useStyle = 5;
			item.useAmmo = AmmoID.Bullet;
			item.shoot = ModContent.ProjectileType<VizenShot>();
			dmgratio = dmgratiobase = new float[15]{0.34f, 0.33f, 0.33f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f};
            item.glowMask = customGlowMask;
		}
		public override void PostSetDefaults(Player player){
			if(player.altFunctionUse==2){
				item.noUseGraphic = true;
				item.useTime = 22;
				item.useAnimation = 22;
				switch(ability){
					case 1:
					dmgratio = dmgratiobase = new float[15]{0f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f};
					return;
					case 3:
					dmgratio = dmgratiobase = new float[15]{0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f};
					return;
					default:
					return;
				}
			}
		}
		public override Vector2? HoldoutOffset(){
			return new Vector2(-11,0);
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
				}
			}
			return true;
		}
		/*
		public void CastAbility(int i){
			Player player = Main.player[item.owner];
			switch(i){
				case 1:
				if(!player.CheckMana(50, true))return;
				Projectile.NewProjectile(player.Center, (Main.MouseWorld-player.Center).SafeNormalize(new Vector2())*7.5f, ModContent.ProjectileType<VoxAbility>(), realdmg, 15, player.whoAmI);
				Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y, 38, pitchOffset:-0.55f);
				break;
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
		}//*/
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
			if(player.altFunctionUse==2){
				//Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y, 38, pitchOffset:0f);
				switch (ability){
					case 0:
					if(!player.CheckMana(35, true))break;
					type = ModContent.ProjectileType<VizenSmite>();
					Projectile.NewProjectileDirect(Main.MouseWorld, new Vector2(), type, damage, 0, item.owner, 1).penetrate = 60;
					Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y, 68, pitchOffset:0.15f).Volume = 0.45f;
					Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y, 117, pitchOffset:0.15f);
					break;
					case 1:
					type = ModContent.ProjectileType<VizenSlash>();
					speedX*=0.75f;
					speedY*=0.75f;
					knockBack*=3;
					position = player.position;
					int proj = Projectile.NewProjectile(position, new Vector2(speedX,speedY), type, damage, knockBack, item.owner);
					Main.projectile[proj].maxPenetrate += punchthrough;
					Main.projectile[proj].penetrate += punchthrough;
					EntModProjectile p = Main.projectile[proj].modProjectile as EntModProjectile;
					if(p!=null){
						p.dmgratio = p.dmgratiobase = dmgratio;
						p.critcombo = critcomboboost;
					}
					break;
					case 2:
					if(!player.CheckMana(50, true))break;
					int id = ModContent.BuffType<Renewal>();
					for (int i = 0; i < Main.player.Length; i++){
						Player target = Main.player[i];
						if(target.active && target.team==player.team){
							target.AddBuff(id, 300);
						}
					}
					break;
					case 3:
					if(!player.CheckMana(35, true))break;
					type = ModContent.ProjectileType<VizenSmite>();
					Vector2 vec2 = new Vector2(Main.lastMouseX+Main.screenPosition.X, Main.lastMouseY+Main.screenPosition.Y);
					Vector2 vec3 = Main.MouseWorld;
					vec3 = (vec3 - vec2).SafeNormalize(new Vector2())*32;
					speedX = vec3.X;
					speedY = vec3.Y;
					damage/=3;
					knockBack = 0;
					position = Main.MouseWorld;
					base.Shoot(player, ref vec2, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
					Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y, 68, pitchOffset:0.15f).Volume = 0.45f;
					Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y, 117, pitchOffset:0.15f);
					break;
				}
				return false;
			}else{
				type = ModContent.ProjectileType<VizenShot>();
				Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y, 38, pitchOffset:0.15f).Volume = 0.55f;
				Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y, 40, pitchOffset:0.15f).Volume = 0.45f;
				Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y, 89, pitchOffset:0.15f);
			}
			Vector2 vec = new Vector2(speedX, speedY);
			position+=vec;
			vec = vec.RotatedByRandom(0.02f);
			speedX = vec.X;
			speedY = vec.Y;
			base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
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
