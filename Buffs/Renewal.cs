using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Entropy.Buffs{
	public class Renewal : ModBuff{
		public override void SetDefaults(){
			DisplayName.SetDefault("Renewal");
			Description.SetDefault("");
			//Main.buffNoTimeDisplay[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex){
			player.lifeRegen+=10;
		}
	}
}