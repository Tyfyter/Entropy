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
using System.Text.RegularExpressions;
using Entropy.UI;
using Entropy.Items.Mods;

namespace Entropy.Items
{
    public class EntModItemBase : ModItem{
        public virtual bool IsMod => false;
        public override bool CloneNewInstances => true;
        public override bool Autoload(ref string name){return false;}
    }
	public class EntModItem : EntModItemBase
	{
		public float critDMG = 2;
		public float baseCD = 2;
		public float basestat = 15;
		public float statchance = 15;
        public float comboDMG = 0.5f;
        public int combohits = 5;
        public float combotime = 1800;
        public float[] dmgratiobase = new float[15] {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1};
		public float[] dmgratio = new float[15] {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1};
        public int dmgbase = 50;
        public int realdmg = 50;
        public int basecrit = 15;
        public int realcrit = 15;
        public float normcritmult = 1;
        public float combospeedmult = 0;
        public Item[] mods = new Item[8];
        int timesinceright = 0;
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
            for(int i = 0; i < 8; i++)o.Add("mod"+i,mods[i]);
            return o;
        }
        public override void Load(TagCompound tag){
            for(int i = 0; i < 8; i++)if(tag.HasTag("mod"+i))mods[i] = tag.Get<Item>("mod"+i);
        }
        public void ModEffect(EntModItemMod mod){
            if(mod!=null)ModEffectobsolete(mod.type, mod.level);
        }
        public override void HoldItem(Player player){
            SetDefaults();
            for(int i = 0; i < mods.Length; i++){
				/*Entropy.*/ModEffect(mods[i].modItem as EntModItemMod);
			}
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
            level++;
            switch (modid)
            {
                case 1://base damage
                realdmg += (int)(dmgbase * (level * 0.20));
                break;
                case 2://base cc
                realcrit += (int)((usedcrit) * (level * 0.10));
                break;
                case 3://base cd
                critDMG += (baseCD * (float)(level * 0.15));
                break;
                case 4://br
                if(modPlayer.combocounter < combohits)break;
                //realcrit = (float)((item.crit + usedcrit) * (1 + (((1 + level) * 0.15)*(1+(comboDMG*modPlayer.comboget())))));
                break;
                case 5://ww
                if(modPlayer.combocounter < combohits)break;
                statchance = (float)(statchance * (1 + ((level * 0.15)*comboMult(modPlayer.combocounter, combohits, comboDMG))));
                break;
                case 6://speed
                if(modPlayer.combocounter < combohits){
                    combospeedmult = 0;
                    break;
                }
                //item.useAnimation = (int)(item.useAnimation / (1 + (((1 + level) * 0.15)*(1+(comboDMG*modPlayer.comboget())))));
                //item.useTime = (int)(item.useTime / (1 + (((1 + level) * 0.15)*(1+(comboDMG*modPlayer.comboget())))));
                combospeedmult = (float)(level * 0.05);
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
			float[] dmgarray = Entropy.GetDmgRatio(Main.player[item.owner].GetWeaponDamage(item), dmgratio);
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
							if(dmgarray[i2+1] != 0&&alltypesstring.Length>0){
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
                }else if (tooltips[i].Name.Equals("CritChance"))
                {
					//String[] St = tooltips[i].text.Split();
                    //tooltips[i].text.Replace("\\d+",realcrit+"");
                    string critd = Regex.Replace(Lang.tip[5].Value.Replace("%", "x"), " [^ ]+$", "")+Regex.Replace(Lang.tip[2].Value, ".+ ", " ");
                    if(!tooltips.Exists(isCD))tooltips.Insert(i+1, new TooltipLine(mod, "CritDamage", critDMG+critd));
                }else if (tooltips[i].Name.Equals("CritDamage"))
                {
                    string ccString = Regex.Replace(Lang.tip[5].Value.Replace("%", "x")," [^ ]+$", "");
                    tooltips[i].text = critDMG+ccString+Regex.Replace(Lang.tip[2].Value, ".+ ", " ");//+Lang.tip[5].Value.Split()[1];
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
        static bool isCD(TooltipLine tl){
            return tl.Name.Equals("CritDamage");
        }

        public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit){
			EntropyPlayer modPlayer = player.GetModPlayer<EntropyPlayer>(mod);
			modPlayer.comboadd();
			float[] dmgarray = Entropy.GetDmgRatio(damage, dmgratio);
			damage = (int)Entropy.DmgCalcNPC(dmgarray, target);
			int cc = 0;
            if(item.melee){
                cc = player.meleeCrit;
            }else if(item.ranged){
                cc = player.rangedCrit;
            }else if(item.magic){
                cc = player.magicCrit;
            }else if(item.thrown){
                cc = player.thrownCrit;
            }
			if(crit){
				damage /= 2;
                crit = false;
			}
            int ccolor = 0;
			for(int i = cc; i > 0; i-=100){
				if(i < 100){
					if(i < Main.rand.Next(0,100))break;
				}
				damage = (int)(damage * critDMG);
                crit = true;
                if(++ccolor>1){
                    Dust.NewDustPerfect(target.Center, 267, new Vector2(0,16).RotatedByRandom(0.5f)).noGravity = true;
                }
			}
			Entropy.Proc(this, target, damage);
        }
        public override void GetWeaponDamage(Player player, ref int damage){
            float dmg = realdmg;
            if(item.melee){
                dmg*=player.meleeDamage;
            }if(item.ranged){
                dmg*=player.rangedDamage;
            }if(item.magic){
                dmg*=player.magicDamage;
            }if(item.thrown){
                dmg*=player.thrownDamage;
            }
            damage = (int)dmg;
        }
        public override void GetWeaponCrit(Player player, ref int crit){
            crit = realcrit-4;
            if(item.melee){
                player.meleeCrit+=crit;
            }if(item.ranged){
                player.rangedCrit+=crit;
            }if(item.magic){
                player.magicCrit+=crit;
            }if(item.thrown){
                player.thrownCrit+=crit;
            }
            if(item.melee){
                crit = player.meleeCrit;
            }else if(item.ranged){
                crit = player.rangedCrit;
            }else if(item.magic){
                crit = player.magicCrit;
            }else if(item.thrown){
                crit = player.thrownCrit;
            }
        }
        public override float MeleeSpeedMultiplier(Player player){
			EntropyPlayer modPlayer = player.GetModPlayer<EntropyPlayer>(mod);
            return combospeedmult==0?1:combospeedmult*comboMult(modPlayer.combocounter, combohits, comboDMG);
        }
        public override void UpdateInventory(Player player){
            /*
            if(pcount!=passives.Count){
                Main.NewText(pcount+"!="+passives.Count);
                pcount=passives.Count;
            }//*/
            foreach (Item i in mods)if(i!=null)if(!i.IsAir)if(i.modItem.mod==ModLoader.GetMod("ModLoader")){
                i.TurnToAir();
            }
            if(timesinceright>0){
                timesinceright--;
            }
        }
        public override bool CanRightClick(){
            if(timesinceright<=0){
                if(Entropy.mod.UI.CurrentState!=null){
                    Entropy.mod.UI.SetState(null);
                    Entropy.mod.modItemUI.Deactivate();
                    Entropy.mod.modItemUI = null;
                    timesinceright=7;
                    return false;
                }
                ((EntropyPlayer)Main.player[item.owner]).lastmoddeditem = getItemIndex(Main.player[item.owner].inventory);
                Entropy.mod.modItemUI = new ModItemsUI();
                Entropy.mod.modItemUI.Activate();
                Entropy.mod.UI.SetState(Entropy.mod.modItemUI);
            }
            timesinceright=7;
            return false;
        }
        static float comboMult(float cc, float ch, float cd){
            return (float)(Math.Max((Math.Floor(Math.Log(cc/ch, 3))*cd)+cd, 0)+1);
        }
        int getItemIndex(Item[] inventory){
            for (int i = 0; i < inventory.Length; i++){
                if(inventory[i].type!=item.type)continue;
                if((inventory[i].modItem)==null)continue;
                if(!(inventory[i].modItem is EntModItem))continue;
                if((inventory[i].modItem as EntModItem).mods[0] == mods[0])return i;
            }
            return -1;
        }
    }
}