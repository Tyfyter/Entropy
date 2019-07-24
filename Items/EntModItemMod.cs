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

namespace Entropy.Items
{
	public class EntModItemMod : EntModItem
	{
		public modData data;
        public override bool IsMod => true;
        public override bool Autoload(ref string name){
            if(name == "EntModItemMod")return false;
            return true;
        }
		public EntModItemMod(int id, int level){
			data.id = id;
			data.level = level;
		}
        public class modData : TagSerializable {
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
        }
    }
}