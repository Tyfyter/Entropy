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

namespace Entropy.Items.Mods
{
	public class Hornet : EntModItemMod {
		public override int type => 1;
		public override int maxlevel => 5;
		public override void ModifyTooltips(List<TooltipLine> tooltips){
			TooltipLine tip = new TooltipLine(mod, "Desc", "+"+((level+1)*20f)+"% damage");
			tooltips.Add(tip);
		}
	}
	public class ComboSpeed : EntModItemMod {
		public override int type => 1;
		public override int maxlevel => 5;
		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Relentless Fury");
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips){
			TooltipLine tip = new TooltipLine(mod, "Desc", "+"+((level+1)*5f)+"% speed\nStacks with combo multiplier");
			tooltips.Add(tip);
		}
	}
	public class Force : EntModItemMod {
		public override int type => 9;
		public override int maxlevel => 5;
		public override void ModifyTooltips(List<TooltipLine> tooltips){
			TooltipLine tip = new TooltipLine(mod, "Desc", "+"+((level+1)*15f)+"% impact damage");
			tooltips.Add(tip);
		}
	}
	public class Thorn : EntModItemMod {
		public override int type => 10;
		public override int maxlevel => 5;
		public override void ModifyTooltips(List<TooltipLine> tooltips){
			TooltipLine tip = new TooltipLine(mod, "Desc", "+"+((level+1)*15f)+"% puncture damage");
			tooltips.Add(tip);
		}
	}
	public class EntModItemMod : EntModItemBase
	{
		//public modData data;
		public virtual int type => 0;
		public virtual int level {get;set;}
		public virtual int maxlevel => 0;
        public override bool IsMod => true;
        public override bool Autoload(ref string name){
            if(name == "EntModItemMod")return false;
            return true;
        }
		public override void SetDefaults(){
			item.width = item.height = 32;
		}
        public override TagCompound Save(){
            TagCompound o = new TagCompound(){{"lvl",level}};
            return o;
        }
        public override void Load(TagCompound tag){
            if(tag.HasTag("lvl"))level = tag.Get<int>("lvl");
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