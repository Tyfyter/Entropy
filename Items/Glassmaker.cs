using System;
using System.Collections.Generic;
using System.IO;
using Entropy.Buffs;
using Entropy.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using static Entropy.EntropyExt;

namespace Entropy.Items{
	//Abilities:
	//Fireball
	//Raging Inferno
	//Fire Blast
	//World on Fire
    public class Glassmaker : CompModItem{
        public override bool Autoload(ref string name) {
		        Main.armorHeadTexture[134] = Main.instance.OurLoad<Texture2D>(string.Concat(new object[]{"Images",Path.DirectorySeparatorChar,"Armor_Head_134"}));
		        Main.armorHeadTexture[181] = Main.instance.OurLoad<Texture2D>(string.Concat(new object[]{"Images",Path.DirectorySeparatorChar,"Armor_Head_181"}));
		        Main.armorHeadLoaded[134] = true;
		        Main.armorHeadLoaded[181] = true;
		        Main.armorLegTexture[130] = Main.instance.OurLoad<Texture2D>(string.Concat(new object[]{"Images",Path.DirectorySeparatorChar,"Armor_Legs_130"}));
		        Main.armorLegsLoaded[130] = true;
		        Main.armorBodyTexture[177] = Main.instance.OurLoad<Texture2D>(string.Concat(new object[]{"Images",Path.DirectorySeparatorChar,"Armor_Body_177"}));
		        Main.femaleBodyTexture[177] = Main.instance.OurLoad<Texture2D>(string.Concat(new object[]{"Images",Path.DirectorySeparatorChar,"Female_Body_177"}));
		        Main.armorArmTexture[177] = Main.instance.OurLoad<Texture2D>(string.Concat(new object[]{"Images",Path.DirectorySeparatorChar,"Armor_Arm_177"}));
		        Main.armorBodyLoaded[177] = true;
            return true;
        }
        public static short customGlowMask = 0;
        public override void SetDefaults(){
			item.ranged = true;
            item.width = 44;
            item.height = 24;
			if(!item.noUseGraphic){
				item.damage = realdmg = dmgbase = 35;
				item.useTime = 1;
				item.useAnimation = 7;
				item.shootSpeed = 15.5f;
				realcrit = basecrit = 15;
				statchance = basestat = 7;
            	item.autoReuse = true;
				item.magic = false;
			}else{
				item.useTime = 28;
				item.useAnimation = 28;
				if(ability == 0)realdmg = dmgbase = 140;
				else if(ability == 2)realdmg = dmgbase = 120;
				else if(ability == 3)realdmg = dmgbase = 180;
				realcrit = basecrit = 17;
				statchance = basestat = 37;
				item.shoot = ProjectileID.InfernoFriendlyBolt;
            	item.autoReuse = false;
				item.magic = true;
			}
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 5.5f;
            item.value = 100000;
            item.rare = ItemRarityID.Yellow;
			item.useAmmo = AmmoID.Gel;
			item.shoot = ModContent.ProjectileType<Glassmaker_P>();
			//basestat = 17;
			dmgratio = dmgratiobase = new float[15] {0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f};
            //item.glowMask = customGlowMask;
        }
		public override bool ConsumeAmmo(Player player){
			return player.altFunctionUse!=2;
		}
		public override bool AltFunctionUse(Player player){
			return true;
		}
		public override void tryScroll(int dir){
			ability=(ability+dir+4)%4;
		}
		public override bool CanUseItem(Player player){
			if(!base.CanUseItem(player))return false;
			if(player.altFunctionUse==2){
            	item.autoReuse = false;
				item.noUseGraphic = true;
				item.useTime = 28;
				item.useAnimation = 28;
				if(ability == 0)realdmg = dmgbase = 140;
				else if(ability == 2){
					realdmg = dmgbase = 120;
					if(!player.CheckMana(150))return false;
				}
				else if(ability == 3)realdmg = dmgbase = 180;
				realcrit = basecrit = 17;
				statchance = basestat = 37;
				item.magic = true;
				CastAbility(ability);
				return true;
			}
			item.noUseGraphic = false;
			return true;
		}
		public void CastAbility(int i){
			Player player = Main.player[item.owner];
			EntropyPlayer modPlayer = player.GetModPlayer<EntropyPlayer>();
			switch(i){
				case 1:
                if(modPlayer.inferno>0) {
				    modPlayer.inferno = -1;
                    return;
                }
				if(!player.CheckMana(50, true))return;
				modPlayer.infernorate = 0.5f;
				modPlayer.inferno = EntropyPlayer.InfernoMax;
				Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y, 34, pitchOffset:-0.55f);
				break;
				case 2:
				if(!player.CheckMana(150, true))return;
				modPlayer.infernorate = Math.Max(modPlayer.infernorate-2f, 0.25f);
				modPlayer.inferno = Math.Min(modPlayer.inferno+EntropyPlayer.InfernoMax/4, EntropyPlayer.InfernoMax);
				Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<GlassmakerBlast>(), realdmg, item.knockBack*1.5f, player.whoAmI);
				Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y, 34, 1f, -0.15f);
				break;
				case 3:
				modPlayer.infernorate+=0.5f;
				if(Projectile.NewProjectileDirect(player.Center, Vector2.Zero, ModContent.ProjectileType<GlassmakerTargeting>(), realdmg, 0, player.whoAmI).modProjectile is GlassmakerTargeting p)p.dmgratio = dmgratio;
				break;
			}
		}

		public override void SetStaticDefaults(){
		  DisplayName.SetDefault("Glassmaker");
		  Tooltip.SetDefault("Fire, all-consuming.");
          //customGlowMask = Entropy.SetStaticDefaultsGlowMask(this,"");
		}
		public override void AddRecipes(){
			ModRecipe recipe = new ModRecipe(mod);
			int? rs = ModLoader.GetMod("Artifice")?.ItemType("RedShroomite");
			if(rs.HasValue){
				recipe.AddIngredient(rs.Value, 18);
			}else{
				recipe.AddIngredient(ItemID.EldMelter, 1);
			}
			recipe.AddIngredient(ItemID.GlassKiln, 1);
			recipe.AddIngredient(ItemID.SoulofFright, 5);
			recipe.AddIngredient(ItemID.SoulofMight, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
		public override Vector2? HoldoutOffset(){
			return new Vector2(-8,0);
		}
		public override void HoldItem (Player player){
			if(player.itemAnimation==0)item.noUseGraphic = false;
			base.HoldItem(player);
			addElement(5, 0.15f);
			dmgratio[5]+=0.85f;
            //EntropyPlayer modPlayer = player.GetModPlayer<EntropyPlayer>();
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips){
			base.ModifyTooltips(tooltips);
            for (int i = 0; i < tooltips.Count; i++){
                if (tooltips[i].Name.Equals("ItemName")){
                    tooltips[i].overrideColor = new Color(230, 61, 0, 200);
				}
            }
        }
		public override void PostShoot(Projectile p){
			if(Main.player[p.owner].altFunctionUse == 2 && ability == 0)p.extraUpdates++;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
			Vector2 speed;
			if(player.altFunctionUse==2){
				if(ability == 0){
					//realdmg = dmgbase = 140;
					//realcrit = basecrit = 17;
					//statchance = basestat = 37;
					if(!player.CheckMana(50, true))return false;
					type = ProjectileID.InfernoFriendlyBolt;
					Main.PlaySound(2, (int)position.X, (int)position.Y, 34, 1f, -0.35f);
					damage*=5;
					speed = new Vector2(speedX, speedY).SafeNormalize(Vector2.Zero);
					position+=speed*16;
					return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);;
				}
				if(ability==2)player.itemRotation = player.direction;
				if(ability==3)player.itemRotation = -player.direction;
				return false;
			}
			Main.PlaySound(2, (int)position.X, (int)position.Y, 34, 1f, -0.5f);
			//Main.PlaySound(2, (int)position.X, (int)position.Y, 14, 1f, -0.15f);
			speed = new Vector2(speedX, speedY).SafeNormalize(Vector2.Zero);
			position+=speed*24;
			position-=speed.RotatedBy(Math.PI/2)*6*player.direction;
			return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
		}
    }
}