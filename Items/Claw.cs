using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Entropy.Projectiles;

namespace Entropy.Items{
	public class Claw : EntModItem{
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Laceres");
			Tooltip.SetDefault("");
			//Item.claw[item.type] = true;
		}
        public override void SetDefaults() { SetEntropyDefaults(); }
        public override void SetEntropyDefaults(){
			item.damage = 23;//realdmg = dmgbase = 23;
			item.melee = true;
			item.width = 26;
			item.height = 26;
			item.useTime = 10;
			item.useAnimation = 10;
			item.useStyle = 1;
			item.knockBack = 3;
			item.value = 10000;
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item1;
			item.useTurn = true;
			item.shootSpeed = 14.5f;
			dmgratio = dmgratiobase = new float[]{0.9f, 0f, 0.1f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f};
			critDMG = baseCD = 1.6f;
			comboDMG = 0.6f;
			realcrit = basecrit = 35;
			statchance = basestat = 5;
		}
		public override bool AltFunctionUse(Player player){
			return true;
		}

        public override bool CanUseItem(Player player){
            EntropyPlayer modPlayer = player.GetModPlayer<EntropyPlayer>();
            if (player.altFunctionUse == 2){
				if(modPlayer.combocounter>0){
					item.useTime = 8;
					item.useAnimation = 8;
					item.shoot = ModContent.ProjectileType<ClawFeather>();
					item.useStyle = 5;
					item.autoReuse = false;
					modPlayer.comboadd(-1, 150);
					item.UseSound = SoundID.Item39;
					realcrit = basecrit = 5;
					statchance = basestat = 35;
					item.shootSpeed = 14.5f;
				}else{
					item.useTime = 18;
					item.useAnimation = 18;
					item.shoot = ModContent.ProjectileType<ClawFeather>();
					item.useStyle = 5;
					item.autoReuse = false;
					item.UseSound = SoundID.Item39;
					realcrit = basecrit = 5;
					statchance = basestat = 15;
					item.shootSpeed = 7.5f;
				}
            }else{
                item.useTime = 10;
                item.useAnimation = 10;
				item.shoot = 0;
				item.useStyle = 1;
				item.autoReuse = true;
				item.UseSound = SoundID.Item1;
				realcrit = basecrit = 35;
				statchance = basestat = 5;
			}
            return base.CanUseItem(player);
        }
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
			if(player.itemAnimation==14)return false;
			if(player.itemAnimation==4)damage = (int)(damage/1.4f);
			//try{Main.PlaySound(71).Pitch = Main.mouseY/(Main.screenHeight/2f);}catch(Exception){Main.NewText(Main.mouseY/(Main.screenHeight/2f)+"is out of bounds");}
			return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
		}
	}
}
