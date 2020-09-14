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

namespace Entropy.Items.Mods{
	public class Hornet : EntModItemMod {
		public override int type => 1;
		public override Color Rarity => Color.Silver;
		public override int basereenforcement => 2;
		public override (int,int)[] reenforcePrice => new (int, int)[]{
			(2,ItemID.IronBar),
			(2,ItemID.IronBar),
			(1,ItemID.SoulofMight),
			(2,ItemID.HallowedBar),
			(2,ItemID.HallowedBar)
		};
		public override void ModifyTooltips(List<TooltipLine> tooltips){
			TooltipLine tip = new TooltipLine(mod, "Desc", "+"+((level+1)*5f)+"% damage");
			tooltips.Add(tip);
			base.ModifyTooltips(tooltips);
		}
	}
	public class ComboSpeed : EntModItemMod {
		public override int type => 6;
		public override Color Rarity => Color.Silver;
		public override bool CanApply(EntModItem item) => item.realCombo;
		public override void SetStaticDefaults(){
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
		public override (int,int)[] reenforcePrice => new (int, int)[]{
			(2,ItemID.IronBar),
			(2,ItemID.IronBar),
			(1,ItemID.SoulofSight),
			(2,ItemID.HallowedBar),
			(2,ItemID.HallowedBar)
		};
		public override void SetStaticDefaults(){
		  DisplayName.SetDefault("Weeping Wounds");
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips){
			TooltipLine tip = new TooltipLine(mod, "Desc", "+"+((level+1)*5f)+"% status chance\nStacks with combo multiplier");
			tooltips.Add(tip);
			base.ModifyTooltips(tooltips);
		}
	}
	public class ComboCrit : EntModItemMod {
		public override int type => 4;
		public override int maxlevel => 10;
		public override Color Rarity => Color.Silver;
		public override bool CanApply(EntModItem item) => item.realCombo;
		public override (int,int)[] reenforcePrice => new (int, int)[]{
			(2,ItemID.IronBar),
			(2,ItemID.IronBar),
			(1,ItemID.SoulofSight),
			(2,ItemID.HallowedBar),
			(2,ItemID.HallowedBar)
		};
		public override void SetStaticDefaults(){
		  DisplayName.SetDefault("Blood Rush");
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips){
			#pragma warning disable 0618
			TooltipLine tip = new TooltipLine(mod, "Desc", "+"+((level+1)*10f)+Lang.tip[5]+"\nStacks with combo multiplier");
			#pragma warning disable 0618
			tooltips.Add(tip);
			base.ModifyTooltips(tooltips);
		}
	}
	public class Force : EntModItemMod {
		public override int type => 9;
		public override Color Rarity => Color.Goldenrod;
		public override (int,int)[] reenforcePrice => new (int, int)[]{
			(2,ItemID.IronBar),
			(2,ItemID.IronBar),
			(1,ItemID.SoulofMight),
			(2,ItemID.HallowedBar),
			(2,ItemID.HallowedBar)
		};
		public override void ModifyTooltips(List<TooltipLine> tooltips){
			TooltipLine tip = new TooltipLine(mod, "Desc", "+"+((level+1)*10f)+"% impact damage");
			tooltips.Add(tip);
			base.ModifyTooltips(tooltips);
		}
	}
	public class Thorn : EntModItemMod {
		public override int type => 10;
		public override Color Rarity => Color.Goldenrod;
		public override (int,int)[] reenforcePrice => new (int, int)[]{
			(2,ItemID.IronBar),
			(2,ItemID.IronBar),
			(1,ItemID.SoulofMight),
			(2,ItemID.HallowedBar),
			(2,ItemID.HallowedBar)
		};
		public override void ModifyTooltips(List<TooltipLine> tooltips){
			TooltipLine tip = new TooltipLine(mod, "Desc", "+"+((level+1)*10f)+"% puncture damage");
			tooltips.Add(tip);
			base.ModifyTooltips(tooltips);
		}
	}
	public class Cutting_Edge : EntModItemMod {
		public override int type => 8;
		public override Color Rarity => Color.Goldenrod;
		public override (int,int)[] reenforcePrice => new (int, int)[]{
			(2,ItemID.IronBar),
			(2,ItemID.IronBar),
			(1,ItemID.SoulofMight),
			(2,ItemID.HallowedBar),
			(2,ItemID.HallowedBar)
		};
		public override void SetStaticDefaults(){
		  DisplayName.SetDefault("Cutting Edge");
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips){
			TooltipLine tip = new TooltipLine(mod, "Desc", "+"+((level+1)*10f)+"% slash damage");
			tooltips.Add(tip);
			base.ModifyTooltips(tooltips);
		}
	}
	public class Frost : EntModItemMod {
		public override int type => 14;
		public override Color Rarity => Color.Silver;
		public override (int,int)[] reenforcePrice => new (int, int)[]{
			(2,ItemID.IronBar),
			(2,ItemID.IronBar),
			(1,ItemID.SoulofFright),
			(2,ItemID.HallowedBar),
			(2,ItemID.HallowedBar)
		};
		public override void ModifyTooltips(List<TooltipLine> tooltips){
			TooltipLine tip = new TooltipLine(mod, "Desc", "[c/"+Entropy.dmgcolor[Entropy.id_cold]+":+"+((level+1)*6f)+$"%{Entropy.dmgtypes[Entropy.id_cold]} damage]");
			tooltips.Add(tip);
			base.ModifyTooltips(tooltips);
		}
	}
	public class Cinder : EntModItemMod {
		public override int type => 15;
		public override Color Rarity => Color.Silver;
		public override (int,int)[] reenforcePrice => new (int, int)[]{
			(2,ItemID.IronBar),
			(2,ItemID.IronBar),
			(1,ItemID.SoulofFright),
			(2,ItemID.HallowedBar),
			(2,ItemID.HallowedBar)
		};
		public override void ModifyTooltips(List<TooltipLine> tooltips){
			TooltipLine tip = new TooltipLine(mod, "Desc", "[c/"+Entropy.dmgcolor[Entropy.id_heat]+":+"+((level+1)*6f)+$"%{Entropy.dmgtypes[Entropy.id_heat]} damage]");
			tooltips.Add(tip);
			base.ModifyTooltips(tooltips);
		}
	}
	public class Shock : EntModItemMod {
		public override int type => 16;
		public override Color Rarity => Color.Silver;
		public override (int,int)[] reenforcePrice => new (int, int)[]{
			(2,ItemID.IronBar),
			(2,ItemID.IronBar),
			(1,ItemID.SoulofFright),
			(2,ItemID.HallowedBar),
			(2,ItemID.HallowedBar)
		};
		public override void ModifyTooltips(List<TooltipLine> tooltips){
			TooltipLine tip = new TooltipLine(mod, "Desc", "[c/"+Entropy.dmgcolor[Entropy.id_electric]+":+"+((level+1)*6f)+$"%{Entropy.dmgtypes[Entropy.id_electric]} damage]");
			tooltips.Add(tip);
			base.ModifyTooltips(tooltips);
		}
	}
	public class Toxin : EntModItemMod {
		public override int type => 17;
		public override Color Rarity => Color.Silver;
		public override (int,int)[] reenforcePrice => new (int, int)[]{
			(2,ItemID.IronBar),
			(2,ItemID.IronBar),
			(1,ItemID.SoulofFright),
			(2,ItemID.HallowedBar),
			(2,ItemID.HallowedBar)
		};
		public override void ModifyTooltips(List<TooltipLine> tooltips){
			TooltipLine tip = new TooltipLine(mod, "Desc", "[c/"+Entropy.dmgcolor[Entropy.id_toxic]+":+"+((level+1)*6f)+$"%{Entropy.dmgtypes[Entropy.id_toxic]} damage]");
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
			item.rare = ItemRarityID.Green;
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
		public override int basereenforcement => 3;
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
	public class EtherealForce : EntModItemMod {
		public override int type => 20;
		public override Color Rarity => Color.GhostWhite;
		public override (int,int)[] reenforcePrice => new (int, int)[]{
			(2,ItemID.IronBar),
			(2,ItemID.IronBar),
			(1,ItemID.SoulofSight),
			(2,ItemID.HallowedBar),
			(2,ItemID.HallowedBar)
		};
		public override void SetStaticDefaults(){
		  DisplayName.SetDefault("Ethereal Force");
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips){
			TooltipLine tip = new TooltipLine(mod, "Desc", "+"+((level+1)*10f)+"% impact damage");
			tooltips.Add(tip);
			tip = new TooltipLine(mod, "Desc", "+"+((reenforcement)*5f)+"% light damage");
			tooltips.Add(tip);
			base.ModifyTooltips(tooltips);
		}
	}
	public class EntModItemMod : EntModItemBase{
		//public modData data;
		public virtual int type => 0;
		public virtual int level {get;set;}
		public virtual int maxlevel => 5;
		public virtual int reenforcement {get;set;}
		public virtual int basereenforcement => 1;
		public virtual int refPrice =>(int)(5000*Math.Pow(3,level));
		public virtual Color Rarity => Color.Sienna;
        public virtual bool CanApply(EntModItem item) => true;
        public override bool IsMod => true;
		///<summary>
		///price and item type
		///</summary>
		public virtual (int,int)[] reenforcePrice => new (int, int)[]{
			(2,ItemID.IronBar),
			(2,ItemID.IronBar),
			(2,ItemID.IronBar),
			(2,ItemID.HallowedBar),
			(2,ItemID.HallowedBar)
		};
		//bool reenforcing = false;
		static int tempint = 0;
		static int tempint2 = 0;
        public override bool Autoload(ref string name){
            if(name == "EntModItemMod")return false;
            return true;
        }
		public override void SetDefaults(){
			item.width = item.height = 32;
			item.rare = ItemRarityID.Green;
			if(reenforcement<basereenforcement)reenforcement=basereenforcement;
		}
        public override TagCompound Save(){
            TagCompound o = new TagCompound(){{"lvl",level},{"reenforce",reenforcement}};
            return o;
        }
        public override void Load(TagCompound tag){
			#pragma warning disable 0612
            if(tag.HasTag("lvl"))level = tag.Get<int>("lvl");
            if(tag.HasTag("reenforce")){
				reenforcement = tag.Get<int>("reenforce");
			}else{
				reenforcement = level;
			}
			if(reenforcement<basereenforcement)reenforcement=basereenforcement;
			#pragma warning restore 0612
        }
		double rcdelay = 0;
        public override bool CanRightClick(){
			if(reenforcement>=maxlevel)return false;
			if(rcdelay<Main.GameUpdateCount){
				Player player = Main.player[item.owner];
				if(player.HeldItem!=null){
					int i = Math.Min(reenforcement,reenforcePrice.Length-1);
					if(i<0){
						i = 0;
					}
					if(i>=reenforcePrice.Length)return false;
					if(player.HeldItem.type==reenforcePrice[i].Item2&&player.HeldItem.stack>=reenforcePrice[i].Item1){
						player.HeldItem.stack-=reenforcePrice[i].Item1;
						Main.NewText(player.HeldItem.whoAmI);
						reenforcement++;
						Main.PlaySound(SoundID.MenuTick, player.Center);
					}
				}
				rcdelay = Main.GameUpdateCount+4;
				return false;
			}else{
				rcdelay = Main.GameUpdateCount+4;
				return false;
			}
        }
		public override void ModifyTooltips(List<TooltipLine> tooltips){
			if(tooltips.Count>0)tooltips[0].overrideColor = Rarity;
			TooltipLine tip = new TooltipLine(mod, "ModLevel", "");
			int i = 0;
			if(reenforcement>0)tip.text+=$"[c/{Rarity.Hex3()}:";
			for(; i < level; i++)tip.text+="◈";
			for(; i < maxlevel; i++){
				if(i==reenforcement)tip.text+=(reenforcement>0?"]":"")+$"[c/{Color.Lerp(Rarity, Color.Black, 0.75f).Hex3()}:";
				tip.text+="◇";
			}
			tip.text+="]";
			//tip.overrideColor = Rarity;
			tooltips.Add(tip);
			if(reenforcement<maxlevel){
				int r = Math.Min(reenforcement,reenforcePrice.Length-1);
				if(r<0){
					r = 0;
				}
				if(r>=reenforcePrice.Length)return;
				//string name = Lang.GetItemNameValue(reenforcePrice[r].Item2);
				tooltips.Add(new TooltipLine(mod, "Reenforce",$"Right click with [i/s{reenforcePrice[r].Item1}:{reenforcePrice[r].Item2}] to reenforce "));
			}
		}
        public override bool ReforgePrice(ref int reforgePrice, ref bool canApplyDiscount){
			reforgePrice = level<maxlevel?refPrice:0;
			return true;
		}
		public override bool NewPreReforge(){
			tempint = level;
			tempint2 = reenforcement;
			return level<reenforcement;
		}
		public override void PostReforge(){
			reenforcement = tempint2;
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