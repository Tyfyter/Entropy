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

namespace Entropy.Items{
	public class Vox : CompModItem{
		public override string Texture => "Entropy/Items/Vox";
		public override int maxabilities => 3;
		public override bool isGun => true;
		int time = 0;
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Vox");
			Tooltip.SetDefault("Wrath without sound\n-Aurdeorum");
		}
		public override void SetDefaults() {
			item.damage = 70;//realdmg = dmgbase = 210;
			statchance = basestat = 8;
			realcrit = basecrit = 26;
			//item.ranged = mode == 0;
			//item.melee = !item.ranged;
			item.width = 38;
			item.height = 14;
			//item.useTime = mode==2?15:mode==0?17:20;
			//item.useAnimation = mode==2?15:mode==0?17:20;
			//item.useStyle = mode == 0?5:1;
			//item.knockBack = mode==1?12:6;
			item.value = 10000;
			item.rare = 2;
			item.useTime = 15;
			item.useAnimation = 15;
			//item.UseSound = SoundID.Item1;
			item.autoReuse = false;
			item.useTurn = false;
			item.ranged = true;
			item.noMelee = true;
			item.noUseGraphic = false;
			item.shootSpeed = 13.5f;
			item.useStyle = 5;
			item.useAmmo = AmmoID.Bullet;
			item.shoot = ModContent.ProjectileType<VoxSlug>();
			dmgratio = dmgratiobase = new float[15] {0.1f,0.8f,0.1f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f};
		}
		public override void PostSetDefaults(Player player){
			if(player.altFunctionUse==2){
				if(ability == 1){
					item.noUseGraphic = true;
					return;
				}
				dmgratio = dmgratiobase = new float[15] {0.9f,0.01f,0.09f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f};
			}
		}
		public override Vector2? HoldoutOffset(){
			return new Vector2(4,4);
		}
		public override void tryScroll(int dir){
			ability=(ability+dir+3)%3;
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
					item.damage = 50;//realdmg = dmgbase = 140;
					statchance = basestat = 22;
					realcrit = basecrit = 6;
					item.shoot = ModContent.ProjectileType<VoxSlash>();
					return true;
				}
				if(ability == 1){
					if(time==0)CastAbility(1);
					return true;
				}
				if(time==0)CastAbility(ability);
				time = 3;
				return false;
			}
			return true;
		}
		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat){
			base.ModifyWeaponDamage(player, ref add, ref mult, ref flat);
			if(player.detectCreature)mult*=1.25f;
		}
		public override void GetWeaponCrit(Player player, ref int crit){
			if(player.detectCreature)crit = 100;
		}
		public void CastAbility(int i){
			Player player = Main.player[item.owner];
			switch(i){
				case 1:
				if(!player.CheckMana(50, true))return;
				Projectile.NewProjectile(player.Center, (Main.MouseWorld-player.Center).SafeNormalize(new Vector2())*7.5f, ModContent.ProjectileType<VoxAbility>(), player.GetWeaponDamage(item), 15, player.whoAmI);
				Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y, 38, pitchOffset:-0.55f);
				break;
				case 2:
				if(!player.CheckMana(150, true))return;
				player.AddBuff(BuffID.Hunter, 600);
				/*for (int i2 = 0; i2 < Main.npc.Length; i2++){
					if(Main.npc[i2]?.active==true){
						AddBuff(new SonarEffect(Main.npc[i2], 600, player));
					}
				}*/
				for (int y = 0; y < 60; y++){
					Vector2 velocity = new Vector2(8,0).RotatedBy(MathHelper.ToRadians(y*6));
					Dust d = Dust.NewDustPerfect(player.Center+velocity, 267, null, 0, Color.Azure, 0.6f);
					d.velocity = velocity*10;
					//d.position -= d.velocity * 8;
					d.fadeIn = 0.7f;
					d.noGravity = true;
				}
				Main.PlaySound(29, (int)player.Center.X, (int)player.Center.Y, 8).Pitch = 1;
				break;
			}
		}
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
			if(player.altFunctionUse==2){
				type = ModContent.ProjectileType<VoxSlash>();
				Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y, 38, pitchOffset:0.15f);
			}else{
				type = ModContent.ProjectileType<VoxSlug>();
				Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y, 38, pitchOffset:0f);
			}
			if(type == ModContent.ProjectileType<VoxSlug>())return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
			if(type == ModContent.ProjectileType<VoxSlash>()&&ability == 1){
				return false;
			}
			damage/=7; 
			Vector2 vec = new Vector2(speedX, speedY);
			for (int i = 0; i < 7; i++){
				Vector2 vec2 = vec.RotatedByRandom(0.3f);
				speedX = vec2.X;
				speedY = vec2.Y;
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
