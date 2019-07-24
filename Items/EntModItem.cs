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
	public class EntModItem : ModItem
	{
		public float critDMG = 2;
		public float statchance = 15;
        public float comboDMG = 0.5f;
        public int combohits = 5;
        public float combotime = 1800;
        public float[] dmgratiobase = new float[15] {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1};
		public float[] dmgratio = new float[15] {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1};
        public int dmgbase = 50;
        public int basecrit = 15;
        public int realcrit = 15;
        public float normcritmult = 1;
        public float combospeedmult = 0;
        public Item[] mods = new Item[8];
        public virtual bool IsMod => false;
        public override bool CloneNewInstances => true;
        public override bool Autoload(ref string name){
            if(name == "EntModItem")return false;
            return true;
        }
        /*public static EntModItem New(int type = 0, int id = 0, int level = 0){
            EntModItem output;
            switch(type){
                default:
                output = new EntModItemMod(id, level);
                break;
            }
            return output;
        }
        public void ModEffect(EntModItemMod Mod){

        }*/
        public override TagCompound Save(){
            TagCompound o = new TagCompound(){};
            for(int i = 0; i < 8; i++)o.Add(new KeyValuePair<string, object>("mod"+i,mods[i]));
            return o;
        }
        public override void Load(TagCompound tag){
            for(int i = 0; i < 8; i++)if(tag.HasTag("mod"+i))mods[i] = (Item)tag.Get<object>("mod"+i);
        }
        public void ModEffectobsolete(int modid, float level){
            Player player = Main.player[item.owner];
            EntropyPlayer modPlayer = player.GetModPlayer<EntropyPlayer>(mod);
            int usedcrit = 0;
            if(item.melee){
                usedcrit = player.meleeCrit;
            }else if(item.ranged){
                usedcrit = player.rangedCrit;
            }else if(item.magic){
                usedcrit = player.magicCrit;
            }else if(item.thrown){
                usedcrit = player.thrownCrit;
            }
            switch (modid)
            {
                case 1://base damage
                item.damage += (int)(dmgbase * ((1 + level) * 0.20));
                break;
                case 2://base cc
                realcrit = (int)((usedcrit) * (1 + ((1 + level) * 0.10)));
                break;
                case 3://base cd
                critDMG = (critDMG * (float)(1 + ((1 + level) * 0.15)));
                break;
                case 4://br
                if(modPlayer.combocounter < combohits)break;
                //realcrit = (int)((item.crit + usedcrit) * (1 + (((1 + level) * 0.15)*(1+(comboDMG*modPlayer.comboget())))));
                break;
                case 5://ww
                if(modPlayer.combocounter < combohits)break;
                //statchance = (int)(statchance * (1 + (((1 + level) * 0.15)*(1+(comboDMG*modPlayer.comboget())))));
                break;
                case 6://speed
                if(modPlayer.combocounter < combohits){
                    combospeedmult = 0;
                    break;
                }
                //item.useAnimation = (int)(item.useAnimation / (1 + (((1 + level) * 0.15)*(1+(comboDMG*modPlayer.comboget())))));
                //item.useTime = (int)(item.useTime / (1 + (((1 + level) * 0.15)*(1+(comboDMG*modPlayer.comboget())))));
                combospeedmult = (float)((1 + level) * 0.15);
                break;
                case 7://base speed
                item.useTime = (int)(item.useTime*(1-(0.05*level)));
                item.useAnimation = (int)(item.useAnimation*(1-(0.05*level)));
                break;
                case 8://slash
                dmgratio[0] += (int)(dmgratio[0]*(0.15*level));
                break;
                case 9://impact
                dmgratio[1] += (int)(dmgratio[1]*(0.15*level));
                break;
                case 10://puncure
                dmgratio[2] += (int)(dmgratio[2]*(0.15*level));
                break;
                case 11://slash
                dmgratio[0] += (int)(dmgratio[0]*(0.2*level));
                break;
                case 12://impact
                dmgratio[1] += (int)(dmgratio[1]*(0.2*level));
                break;
                case 13://puncure
                dmgratio[2] += (int)(dmgratio[2]*(0.2*level));
                break;
                default:
                break;
            }
        }
		public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
			//HoldItem(Main.player[item.owner]);
			float[] dmgarray = Entropy.GetDmgRatio((int)(item.damage * Main.player[item.owner].meleeDamage), dmgratio);
            for (int i = 0; i < tooltips.Count; i++)
            {
                if (tooltips[i].Name.Equals("Damage"))
                {
					String[] SplitText = tooltips[i].text.Split();
                    TooltipLine tip;
					//tooltips[i].text.Substring(8, tooltips[i].text.Length-8);
					string alltypesstring = "";
					for(int i2 = 0; i2 < dmgarray.Length; i2++){
						if(dmgratio[i2] != 0){
							alltypesstring = alltypesstring + (dmgarray[i2]+" "+Entropy.dmgtypes[i2].ToLower()+" damage");
						}
						if(i2 < dmgarray.Length-1){
							if(dmgarray[i2+1] != 0){
								alltypesstring = alltypesstring + "\n";
							}
						}
					}
					tip = new TooltipLine(mod, "Damage", alltypesstring);
                    //tip = new TooltipLine(mod, "melee", dmgarray[0]+" slash damage\n"+dmgarray[1]+" puncture damage\n"+dmgarray[2]+" impact damage\n");
                    //tip.overrideColor = new Color(255, 32, 174, 200);
					tip.overrideColor = new Color(255, 255, 255);
                    tooltips.RemoveAt(i);
                    tooltips.Insert(i, tip);
                }else if(tooltips[i].text.Contains("infoidk"))
                {
					String[] SplitText = tooltips[i].text.Split();
                    TooltipLine tip;
					tip = new TooltipLine(mod, "info", basecrit+":"+item.crit+":"+Main.player[item.owner].meleeCrit);
                    //tip = new TooltipLine(mod, "melee", dmgarray[0]+" slash damage\n"+dmgarray[1]+" puncture damage\n"+dmgarray[2]+" impact damage\n");
                    //tip.overrideColor = new Color(255, 32, 174, 200);
					tip.overrideColor = new Color(255, 255, 255);
                    tooltips.RemoveAt(i);
                    tooltips.Insert(i, tip);
                }
            }//*/
        }

        public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit){
			EntropyPlayer modPlayer = player.GetModPlayer<EntropyPlayer>(mod);
			modPlayer.comboadd();
			float[] dmgarray = Entropy.GetDmgRatio(damage, dmgratio);
			damage = (int)Entropy.DmgCalcNPC(dmgarray, target);
			if(crit){
				damage /= 2;
			}
			Entropy.Proc(this, target, damage);
			int cc = item.crit+player.meleeCrit;
			for(int i = cc; i > 0; i-=100){
				if(i < 100){
					if(i < Main.rand.Next(0,100))break;
				}
				damage = (int)(damage * critDMG);
			}
        }
        public override void GetWeaponCrit(Player player, ref int crit){
            
        }
        public override float MeleeSpeedMultiplier(Player player){
			EntropyPlayer modPlayer = player.GetModPlayer<EntropyPlayer>(mod);
            return (float)(combospeedmult*(Math.Floor(Math.Max(Math.Log(modPlayer.combocounter/combohits,3), 0))*(comboDMG+1)))+1;
        }
    }
}