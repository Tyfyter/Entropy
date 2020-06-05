using System;
using System.Collections.Generic;
using Entropy.Buffs;
using Entropy.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using static Entropy.EntropyExt;

namespace Entropy.Items{
    public class CorrSniper : EntModItem{
        public static short customGlowMask = 0;
		public override bool realCombo => true;
		public override bool isGun => true;
		public override bool CloneNewInstances => true;
        public override void SetDefaults() { SetEntropyDefaults(); }
        public override void SetEntropyDefaults(){
            //item.name = "lightning";
            item.damage = 75;//realdmg = dmgbase = 225;
			item.ranged = true;
            item.width = 96;
            item.height = 20;
			item.useTime = 45;
			item.useAnimation = 45;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 7.5f;
            item.value = 1000;
            item.rare = ItemRarityID.Cyan;
			item.alpha = 100;
            item.autoReuse = false;
			item.useAmmo = AmmoID.Bullet;
			item.shoot = ModContent.ProjectileType<CorrShotProj>();
            item.shootSpeed = 12.5f;
			realcrit = basecrit = 45;
			dmgratio = dmgratiobase = new float[15] {0f, 0f, 1f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f};
			combohits = 3;
			comboDMG = 0.1f;
            item.glowMask = customGlowMask;
        }

		public override void SetStaticDefaults(){
		  DisplayName.SetDefault("Magnus Kirudo");
		  Tooltip.SetDefault("");
          customGlowMask = Entropy.SetStaticDefaultsGlowMask(this);
		}
		public override void AddRecipes(){
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.PiranhaGun, 1);
			recipe.AddIngredient(ItemID.SoulofSight, 15);
			recipe.AddIngredient(ItemID.Emerald, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
		public override Vector2? HoldoutOffset(){
			return new Vector2(-16,-6);
		}
		public override void HoldItem (Player player){
			base.HoldItem(player);
			addElement(8, 1);
		}
		public override void HoldStyle (Player player){
			player.shroomiteStealth = true;
			player.stealth = player.HasBuff(BuffID.Shine)?0.1f:0.3f;
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
			bool a = false;
			if(dmgratio[8]<=1)a=true;
			if(a)dmgratio[8]+=1f;
			base.ModifyTooltips(tooltips);
			if(a)dmgratio[8]-=1f;
            for (int i = 0; i < tooltips.Count; i++){
                if (tooltips[i].Name.Equals("ItemName")){
                    tooltips[i].overrideColor = new Color(0, 255, 25, 200);
				}else if (tooltips[i].Name.Equals("Damage")){
                    tooltips[i].overrideColor = new Color(0, 255, 25, 200);
				}
            }
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
			if(type<ProjectileID.Count)type = ModContent.ProjectileType<CorrShotProj>();
			Main.PlaySound(SoundID.Item, (int)position.X, (int)position.Y, 38).Volume = 1;
			Main.PlaySound(SoundID.Item, (int)position.X, (int)position.Y, 40).Pitch = -1;
			return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
		}
    }
}