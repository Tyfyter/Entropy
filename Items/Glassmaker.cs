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
	//Took 4 days before jorney's end released, 1 day fixing raging inferno rendering after, ~1.5 of which were just fixing the raging inferno helmet rendering
	//Abilities:
	//Fireball
	//Raging Inferno
	//Fire Blast
	//World on Fire
    public class Glassmaker : CompModItem{
        public override bool Autoload(ref string name) {
		        Main.armorHeadTexture[134] = Main.instance.OurLoad<Texture2D>(string.Concat(new object[]{"Images",Path.DirectorySeparatorChar,"Armor_Head_134"}));
		        Main.armorHeadTexture[ArmorIDs.Head.LazuresValkyrieCirclet] = Main.instance.OurLoad<Texture2D>(string.Concat(new object[]{"Images",Path.DirectorySeparatorChar,"Armor_Head_"+ArmorIDs.Head.LazuresValkyrieCirclet}));
		        Main.armorHeadLoaded[134] = true;
		        Main.armorHeadLoaded[ArmorIDs.Head.LazuresValkyrieCirclet] = true;
		        Main.armorLegTexture[130] = Main.instance.OurLoad<Texture2D>(string.Concat(new object[]{"Images",Path.DirectorySeparatorChar,"Armor_Legs_130"}));
		        Main.armorLegsLoaded[130] = true;
		        Main.armorLegTexture[112] = Main.instance.OurLoad<Texture2D>(string.Concat(new object[]{"Images",Path.DirectorySeparatorChar,"Armor_Legs_112"}));
		        Main.armorLegsLoaded[112] = true;
		        Main.armorBodyTexture[177] = Main.instance.OurLoad<Texture2D>(string.Concat(new object[]{"Images",Path.DirectorySeparatorChar,"Armor_Body_177"}));
		        Main.armorArmTexture[177] = Main.instance.OurLoad<Texture2D>(string.Concat(new object[]{"Images",Path.DirectorySeparatorChar,"Armor_Arm_177"}));
		        Main.armorBodyLoaded[177] = true;
		        Main.femaleBodyTexture[175] = Main.instance.OurLoad<Texture2D>(string.Concat(new object[]{"Images",Path.DirectorySeparatorChar,"Female_Body_175"}));
		        Main.armorArmTexture[175] = Main.instance.OurLoad<Texture2D>(string.Concat(new object[]{"Images",Path.DirectorySeparatorChar,"Armor_Arm_175"}));
		        Main.armorBodyLoaded[175] = true;
            return true;
        }
        public static short customGlowMask = 0;
        public override void SetDefaults() {
            SetEntropyDefaults();
		    Main.armorHeadTexture[134] = Main.instance.OurLoad<Texture2D>(string.Concat(new object[]{"Images",Path.DirectorySeparatorChar,"Armor_Head_134"}));
		    Main.armorHeadTexture[181] = Main.instance.OurLoad<Texture2D>(string.Concat(new object[]{"Images",Path.DirectorySeparatorChar,"Armor_Head_181"}));
		    Main.armorHeadLoaded[134] = true;
		    Main.armorHeadLoaded[181] = true;
		    Main.armorLegTexture[130] = Main.instance.OurLoad<Texture2D>(string.Concat(new object[]{"Images",Path.DirectorySeparatorChar,"Armor_Legs_130"}));
		    Main.armorLegsLoaded[130] = true;
		    Main.armorLegTexture[112] = Main.instance.OurLoad<Texture2D>(string.Concat(new object[]{"Images",Path.DirectorySeparatorChar,"Armor_Legs_112"}));
		    Main.armorLegsLoaded[112] = true;
		    Main.armorBodyTexture[177] = Main.instance.OurLoad<Texture2D>(string.Concat(new object[]{"Images",Path.DirectorySeparatorChar,"Armor_Body_177"}));
		    Main.armorArmTexture[177] = Main.instance.OurLoad<Texture2D>(string.Concat(new object[]{"Images",Path.DirectorySeparatorChar,"Armor_Arm_177"}));
		    Main.armorBodyLoaded[177] = true;
		    Main.femaleBodyTexture[175] = Main.instance.OurLoad<Texture2D>(string.Concat(new object[]{"Images",Path.DirectorySeparatorChar,"Female_Body_175"}));
		    Main.armorArmTexture[175] = Main.instance.OurLoad<Texture2D>(string.Concat(new object[]{"Images",Path.DirectorySeparatorChar,"Armor_Arm_175"}));
		    Main.armorBodyLoaded[175] = true;
        }
        public override void SetEntropyDefaults(){
			item.ranged = true;
            item.width = 44;
            item.height = 24;
			if(!item.noUseGraphic){
				item.damage = 15;//realdmg = dmgbase = 35;
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
				if(ability == 0)item.damage = 50;//realdmg = dmgbase = 140;
				else if(ability == 2)item.damage = 40;//realdmg = dmgbase = 120;
				else if(ability == 3)item.damage = 60;//realdmg = dmgbase = 180;
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

				if(ability == 0)item.damage = 50;
				if(ability == 2){
					item.damage = 40;
					if(!player.CheckMana((int)(150*player.manaCost)))return false;
				}
				else if(ability == 3)item.damage = 60;
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
				if(!player.CheckMana(item, (int)(50*player.manaCost), true))return;
				modPlayer.infernorate = 1f;
				modPlayer.inferno = EntropyPlayer.InfernoMax;
				Main.PlaySound(SoundID.Item, (int)player.Center.X, (int)player.Center.Y, 34, pitchOffset:-0.55f);
				break;
				case 2:
				if(!player.CheckMana(item, (int)(150*player.manaCost), true))return;
                if(modPlayer.inferno>0) {
					modPlayer.infernorate = Math.Max(modPlayer.infernorate-1.5f, 0.25f);
					modPlayer.inferno = Math.Min(modPlayer.inferno+EntropyPlayer.InfernoMax/4, EntropyPlayer.InfernoMax);
				}
				Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<GlassmakerBlast>(), player.GetWeaponDamage(item), item.knockBack*1.5f, player.whoAmI);
				Main.PlaySound(SoundID.Item, (int)player.Center.X, (int)player.Center.Y, 34, 1f, -0.15f);
				break;
				case 3:
                if(modPlayer.inferno>0) {
					modPlayer.infernorate = Math.Min(modPlayer.infernorate+2f, 12.5f);
				}
				if(Projectile.NewProjectileDirect(player.Center, Vector2.Zero, ModContent.ProjectileType<GlassmakerTargeting>(), player.GetWeaponDamage(item), 0, player.whoAmI).modProjectile is GlassmakerTargeting p)p.dmgratio = dmgratio;
				break;
			}
		}
		//float a;
		//float m;
		//float a2;
		//float m2;
		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat){
			//a = add;
			//m = mult;
			base.ModifyWeaponDamage(player, ref add, ref mult, ref flat);
			EntropyPlayer modPlayer = player.GetModPlayer<EntropyPlayer>();
			if(modPlayer.inferno>0){
				float infernoPercent = (float)(1-(Math.Round(modPlayer.inferno*0.9)/EntropyPlayer.InfernoMax)*0.9);
				mult/=1+infernoPercent/4.5f;
				if(player.altFunctionUse==2){
					switch (ability){
						case 0:
						mult*=1+infernoPercent*4;
						break;
						case 1:
						break;
						case 2:
						mult*=1+infernoPercent;
						break;
						default:
						mult*=1+infernoPercent*2;
						break;
					}
				}else{
					mult*=1+infernoPercent;
				}
			}
			//a2 = add;
			//m2 = mult;
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
			bool a = false;
			if(dmgratio[5]<=0.15f)a=true;
			if(a)dmgratio[5]+=1f;
			base.ModifyTooltips(tooltips);
			if(a)dmgratio[5]-=1f;
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
			//damage = getDamage(player, damage, player.altFunctionUse==2);
			if(player.altFunctionUse==2){
				if(ability == 0){
					//realdmg = dmgbase = 140;
					//realcrit = basecrit = 17;
					//statchance = basestat = 37;
					if(!player.CheckMana(item, (int)(50*player.manaCost), true))return false;
					type = ProjectileID.InfernoFriendlyBolt;
					Main.PlaySound(SoundID.Item, (int)position.X, (int)position.Y, 34, 1f, -0.35f);
					damage*=5;
					speed = new Vector2(speedX, speedY).SafeNormalize(Vector2.Zero);
					position+=speed*16;
					return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);;
				}else if(ability==2)player.itemRotation = player.direction;
				else player.itemRotation = -player.direction;
				return false;
			}
			Main.PlaySound(SoundID.Item, (int)position.X, (int)position.Y, 34, 1f, -0.5f);
			//Main.PlaySound(SoundID.Item, (int)position.X, (int)position.Y, 14, 1f, -0.15f);
			speed = new Vector2(speedX, speedY).SafeNormalize(Vector2.Zero);
			position+=speed*24;
			position-=speed.RotatedBy(Math.PI/2)*6*player.direction;
			return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
		}
    }
}