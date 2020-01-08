using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Text;
using System.Reflection;
using Terraria.Utilities;

namespace Entropy.Items.Mods
{
	public class Hornet : EntModItemMod {
		public override int type => 1;
		public override Color Rarity => Color.Silver;
		public override void ModifyTooltips(List<TooltipLine> tooltips){
			TooltipLine tip = new TooltipLine(mod, "Desc", "+"+((level+1)*20f)+"% damage");
			tooltips.Add(tip);
			base.ModifyTooltips(tooltips);
		}
	}
	public class ComboSpeed : EntModItemMod {
		public override int type => 6;
		public override Color Rarity => Color.Silver;
		public override bool CanApply(EntModItem item) => item.realCombo;
		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Relentless Fury");
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips){
			TooltipLine tip = new TooltipLine(mod, "Desc", "+"+((level+1)*0.001f)+"% speed\nStacks with combo multiplier");
			tooltips.Add(tip);
			base.ModifyTooltips(tooltips);
		}
	}
	public class CritCombo : EntModItemMod {
		public override int type => 18;
		public override int maxlevel => 0;
		public override Color Rarity => Color.Goldenrod;
		public override bool CanApply(EntModItem item) => item.realCombo;
		public override string Texture => "Entropy/Items/Mods/ComboSpeed";
		public override void SetStaticDefaults(){
		  DisplayName.SetDefault("Relentless Fury");
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips){
			TooltipLine tip = new TooltipLine(mod, "Desc", "+"+(level+1)+" combo gain on crits");
			tooltips.Add(tip);
			base.ModifyTooltips(tooltips);
		}
	}
	public class PrimedCritCombo : CritCombo {
		public override int maxlevel => 3;
		public override Color Rarity => Color.GhostWhite;
		public override int refPrice =>(int)(10000*Math.Pow(10,level+1));
		public override void SetStaticDefaults(){
		  DisplayName.SetDefault("Primed Relentless Fury");
		}
	}
	public class ComboStat : EntModItemMod {
		public override int type => 5;
		public override Color Rarity => Color.Silver;
		public override bool CanApply(EntModItem item) => item.realCombo;
		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Weeping Wounds");
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips){
			TooltipLine tip = new TooltipLine(mod, "Desc", "+"+((level+1)*7.5f)+"% status chance\nStacks with combo multiplier");
			tooltips.Add(tip);
			base.ModifyTooltips(tooltips);
		}
	}
	public class ComboCrit : EntModItemMod {
		public override int type => 4;
		public override int maxlevel => 10;
		public override Color Rarity => Color.Silver;
		public override bool CanApply(EntModItem item) => item.realCombo;
		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Blood Rush");
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips){
			#pragma warning disable 0618
			TooltipLine tip = new TooltipLine(mod, "Desc", "+"+((level+1)*15f)+Lang.tip[5]+"\nStacks with combo multiplier");
			#pragma warning disable 0618
			tooltips.Add(tip);
			base.ModifyTooltips(tooltips);
		}
	}
	public class Force : EntModItemMod {
		public override int type => 9;
		public override Color Rarity => Color.Goldenrod;
		public override void ModifyTooltips(List<TooltipLine> tooltips){
			TooltipLine tip = new TooltipLine(mod, "Desc", "+"+((level+1)*15f)+"% impact damage");
			tooltips.Add(tip);
			base.ModifyTooltips(tooltips);
		}
	}
	public class Thorn : EntModItemMod {
		public override int type => 10;
		public override Color Rarity => Color.Goldenrod;
		public override void ModifyTooltips(List<TooltipLine> tooltips){
			TooltipLine tip = new TooltipLine(mod, "Desc", "+"+((level+1)*15f)+"% puncture damage");
			tooltips.Add(tip);
			base.ModifyTooltips(tooltips);
		}
	}
	public class Cutting_Edge : EntModItemMod {
		public override int type => 8;
		public override Color Rarity => Color.Goldenrod;
		public override void ModifyTooltips(List<TooltipLine> tooltips){
			tooltips[0].text.Replace("_", " ");
			TooltipLine tip = new TooltipLine(mod, "Desc", "+"+((level+1)*15f)+"% slash damage");
			tooltips.Add(tip);
			base.ModifyTooltips(tooltips);
		}
	}
	public class Frost : EntModItemMod {
		public override int type => 14;
		public override Color Rarity => Color.Silver;
		public override void ModifyTooltips(List<TooltipLine> tooltips){
			TooltipLine tip = new TooltipLine(mod, "Desc", "+"+((level+1)*15f)+"% cold damage");
			tooltips.Add(tip);
			base.ModifyTooltips(tooltips);
		}
	}
	public class Cinder : EntModItemMod {
		public override int type => 15;
		public override Color Rarity => Color.Silver;
		public override void ModifyTooltips(List<TooltipLine> tooltips){
			TooltipLine tip = new TooltipLine(mod, "Desc", "+"+((level+1)*15f)+"% heat damage");
			tooltips.Add(tip);
			base.ModifyTooltips(tooltips);
		}
	}
	public class Shock : EntModItemMod {
		public override int type => 16;
		public override Color Rarity => Color.Silver;
		public override void ModifyTooltips(List<TooltipLine> tooltips){
			TooltipLine tip = new TooltipLine(mod, "Desc", "+"+((level+1)*15f)+"% electric damage");
			tooltips.Add(tip);
			base.ModifyTooltips(tooltips);
		}
	}
	public class Toxin : EntModItemMod {
		public override int type => 17;
		public override Color Rarity => Color.Silver;
		public override void ModifyTooltips(List<TooltipLine> tooltips){
			TooltipLine tip = new TooltipLine(mod, "Desc", "+"+((level+1)*15f)+"% toxic damage");
			tooltips.Add(tip);
			base.ModifyTooltips(tooltips);
		}
	}
	public class PunchThrough : EntModItemMod {
		public override int type => 19;
		public override int maxlevel => 3;
		public override Color Rarity => Color.Goldenrod;
		public override bool CanApply(EntModItem item) => item.isGun;
		public override void SetStaticDefaults(){
		  DisplayName.SetDefault("Metal Auger");
		}
		public override void SetDefaults(){
			item.width = 13;
			item.height = 5;
			item.rare = 2;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips){
			TooltipLine tip = new TooltipLine(mod, "Desc", "+"+(level+1)+" pierce");
			tooltips.Add(tip);
			base.ModifyTooltips(tooltips);
		}
	}
	public class PrimedPunchThrough : PunchThrough {
		public override int maxlevel => 10;
		public override Color Rarity => Color.GhostWhite;
		public override string Texture => "Entropy/Items/Mods/PunchThrough";
		public override void SetStaticDefaults(){
		  DisplayName.SetDefault("Primed Metal Auger");
		}
	}
	public class LackThereof : EntModItemMod {
		public override int type => -1;
		public override int maxlevel => 0;
		public override Color Rarity => Color.DimGray;
		public override string Texture => "Entropy/Items/Mods/Force";
		public override void SetStaticDefaults(){
		  DisplayName.SetDefault("Lack Thereof");
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips){
			TooltipLine tip = new TooltipLine(mod, "Desc", "-100% damage");
			tooltips.Add(tip);
			base.ModifyTooltips(tooltips);
		}
	}
	public class EntModItemMod : EntModItemBase
	{
		//public modData data;
		public virtual int type => 0;
		public virtual int level {get;set;}
		public virtual int maxlevel => 5;
		public virtual int refPrice =>(int)(5000*Math.Pow(3,level));
		public virtual Color Rarity => Color.Sienna;
        public virtual bool CanApply(EntModItem item) => true;
        public override bool IsMod => true;
		static int tempint = 0;
        public override bool Autoload(ref string name){
            if(name == "EntModItemMod")return false;
            return true;
        }
		public override void SetDefaults(){
			item.width = item.height = 32;
			item.rare = 2;
		}
        public override TagCompound Save(){
            TagCompound o = new TagCompound(){{"lvl",level}};
            return o;
        }
        public override void Load(TagCompound tag){
			#pragma warning disable 0612
            if(tag.HasTag("lvl"))level = tag.Get<int>("lvl");
			#pragma warning restore 0612
        }
		public override void ModifyTooltips(List<TooltipLine> tooltips){
			tooltips[0].overrideColor = Rarity;
			TooltipLine tip = new TooltipLine(mod, "ModLevel", "");
			int i = 0;
			for(; i < level; i++)tip.text+="◈";
			for(; i < maxlevel; i++)tip.text+="◇";
			tip.overrideColor = Rarity;
			tooltips.Add(tip);
		}
        public override bool ReforgePrice(ref int reforgePrice, ref bool canApplyDiscount){
			reforgePrice = level<maxlevel?refPrice:0;
			return true;
		}
		public override bool NewPreReforge(){
			tempint = level;
			return level<maxlevel;
		}
		public override void PostReforge(){
			level = tempint+1;
			item.prefix = 0;
		}
		public override int ChoosePrefix(UnifiedRandom rand){
			return 1;
		}
		public static int compared = 0;
		public static bool compare(EntModItemMod a){
			if(a == null)return false;
			return a.type==compared;
		}
        /* public class modData : TagSerializable {
			public int id = 0;
			public int level = 0;
        	public static readonly Func<TagCompound, modData> DESERIALIZER = new Func<TagCompound, modData>(Deserialize);
            public modData(int type, int lvl){
				id = type;
				level = lvl;
			}
			modData():this(0,0){}
			public TagCompound SerializeData(){
				TagCompound expr_05 = new TagCompound();
				expr_05["type"] = id;
				expr_05["lvl"] = level;
				return expr_05;
			}
			public static modData Deserialize(TagCompound tag){
				if(!tag.HasTag("type"))return new modData();
				if(!tag.HasTag("lvl"))return new modData(tag.GetInt("type"), 0);
				return new modData(tag.GetInt("type"), tag.GetInt("lvl"));
			}
        } */
    }
}