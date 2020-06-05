using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;

namespace Entropy.Items{
	public class InfinityPlus1 : EntModItem{
		int[] modsobsolete = new int[8] {6,3,0,0,0,0,0,0};
		int[] modlevelsobsolete = new int[8] {0,0,0,0,0,0,0,0};
        int mode = 0;
        public override void SetStaticDefaults(){
			DisplayName.SetDefault("∞+1 sword");
			Tooltip.SetDefault("technically it's the same as an ∞ sword");
		}
        public override void SetDefaults() { SetEntropyDefaults(); }
        public override void SetEntropyDefaults(){
            item.damage = 40;
			item.melee = true;
			item.width = 40;
			item.height = 40;
			item.crit = 0;
            item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = 1;
			item.knockBack = 9;
			item.value = 10000;
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.useTurn = true;
			realcrit = basecrit = 15;
			statchance = 100;
			dmgratio = dmgratiobase = new float[15] {0f,0f,0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f};
			dmgratio[mode] = 1;
		}
		public override bool AltFunctionUse(Player player){
			return true;
		}
		public override bool CanUseItem(Player player){
			if(player.altFunctionUse==2){
				item.useStyle = 2;
				mode=(mode+1)%15;
			}else{
				item.useStyle = 1;
			}
			return true;
		}
    }public class InfinityPlus2 : EntModItem{
		public override string Texture => "Entropy/Items/InfinityPlus1";
        public override void SetStaticDefaults(){
			DisplayName.SetDefault("∞+2 sword");
			Tooltip.SetDefault("technically it's the same as an ∞ + 1 sword");
		}
        public override void SetDefaults() { SetEntropyDefaults(); }
        public override void SetEntropyDefaults(){
            item.damage = 40;
			item.melee = true;
			item.width = 40;
			item.height = 40;
			item.crit = 0;
            item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = 1;
			item.knockBack = 9;
			item.value = 10000;
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.useTurn = true;
			realcrit = basecrit = 15;
			statchance = 100;
			dmgratio = dmgratiobase = new float[15] {1f,1f,1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f};
		}
    }
}
