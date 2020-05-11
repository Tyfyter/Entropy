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

namespace Entropy.Items {
	public class DU_Bullets : EntModItem {
        public override bool isGun => false;
        public override bool realCombo => false;
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Depleted Uranium Rounds");
		}
		public override void SetDefaults() {
			//item.name = "jfdjfrbh";
			item.damage = 50;
			item.ranged = true;
			item.width = 40;
			item.height = 40;
			item.useStyle = 0;
			item.knockBack = 6;
			item.value = 25000;
			item.rare = 2;
			item.UseSound = SoundID.Item1;
			item.ammo = AmmoID.Bullet;
			item.shoot = ModContent.ProjectileType<DU_Bullets_P>();
			item.shootSpeed = 1.25f;
			dmgratio = dmgratiobase = new float[15]{0f, 0.15f, 0.8f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.05f, 0f, 0f, 0f};
            statchance = basestat = 23;
            realcrit = basecrit = 27;
		}
    }
}