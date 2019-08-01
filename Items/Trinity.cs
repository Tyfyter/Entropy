using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Entropy.Items
{
	public class Trinity1 : EntModItem
	{
		int[] modsobsolete = new int[8] {6,3,0,0,0,0,0,0};
		int[] modlevelsobsolete = new int[8] {0,0,0,0,0,0,0,0};
		List<float>[] dmgratios = new List<float>[]{
			new List<float>(new float[15] {0.88f,0.06f,0.06f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f}),
			new List<float>(new float[15] {0.06f,0.88f,0.06f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f}),
			new List<float>(new float[15] {0.06f,0.06f,0.88f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f})};
		int mode = 0;
		int time = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Trinity (Blade)");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults() {
			item.damage = realdmg = dmgbase = mode==0?60:50;
			item.melee = true;
			item.width = 40;
			item.height = 40;
			item.useTime = mode==2?15:mode==0?17:20;
			item.useAnimation = mode==2?15:mode==0?17:20;
			item.useStyle = 1;
			item.knockBack = mode==1?12:6;
			item.value = 10000;
			item.rare = 2;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.useTurn = true;
			dmgratio = dmgratiobase = dmgratios[mode].ToArray();
			critDMG = baseCD = mode==2?2f:1.5f;
			comboDMG = mode==1?0.75f:0.5f;
			combohits = mode==0?6:8;
			statchance = basestat = 25;
		}
		public override Vector2? HoldoutOffset(){
			return new Vector2(4,4);
		}

		/* void ME(){
			for(int i = 0; i < modsobsolete.Length; i++){
				/*Entropy.ModEffectobsolete(modsobsolete[i], modlevelsobsolete[i]);
			}
		} */

		public override void HoldItem(Player player){
			/* SetDefaults();
			ME(); */
			if(time>0)time--;
			base.HoldItem(player);
			/*
			for(int i = 0; i < modsobsolete.Length; i++){
				ModEffectobsolete(modsobsolete[i], modlevelsobsolete[i]);
			}*/
		}
		/* public override bool CanRightClick(){
			if(time>0){
				time = 7;
				return false;
			}
			time = 7;
			mode = ++mode%3;
			item.type = mod.ItemType("Trinity"+(mode+1));
			dmgratio = dmgratiobase = dmgratios[mode];
			return false;
		} */
		public override bool AltFunctionUse(Player player){
			if(time>0){
				time = 7;
				return false;
			}
			time = 7;
			mode = ++mode%3;
			item.type = mod.ItemType("Trinity"+(mode+1));
			dmgratio = dmgratiobase = dmgratios[mode].ToArray();
			return false;
		}
	}
	public class Trinity2 : EntModItem
	{
		int[] modsobsolete = new int[8] {6,3,0,0,0,0,0,0};
		int[] modlevelsobsolete = new int[8] {0,0,0,0,0,0,0,0};
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Trinity (Hammer)");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults() {
			/*item.type = mod.ItemType("Trinity1");
			item.SetDefaults(item.type);*/
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
				/*Entropy.*/ModEffectobsolete(modsobsolete[i], modlevelsobsolete[i]);
			}
		}
		public override bool CanRightClick(){
			item.type = mod.ItemType<Trinity3>();
			return false;
		}
	}
	public class Trinity3 : EntModItem
	{
		int[] modsobsolete = new int[8] {6,3,0,0,0,0,0,0};
		int[] modlevelsobsolete = new int[8] {0,0,0,0,0,0,0,0};
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Trinity (Scythe)");
			Tooltip.SetDefault("");
			Item.staff[item.type] = true;
		}
		public override void SetDefaults() {
			/*item.type = mod.ItemType("Trinity1");
			item.SetDefaults(item.type);*/
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
			item.type = mod.ItemType<Trinity1>();
			return false;
		}
	}
}
