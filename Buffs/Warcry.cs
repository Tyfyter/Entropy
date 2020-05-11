using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Entropy.Buffs{
	public class Warcry : ModBuff{
		public override void SetDefaults(){
			DisplayName.SetDefault("Warcry");
			Description.SetDefault("");
			//Main.buffNoTimeDisplay[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex){
			player.meleeSpeed+=0.15f;
			player.statDefense+=30;
		}
	}
}