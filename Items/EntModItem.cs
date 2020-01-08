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
using Entropy.Projectiles;
using Entropy.Buffs;

namespace Entropy.Items
{
    public class EntModItemBase : ModItem{
        public virtual bool IsMod => false;
        public override bool CloneNewInstances => true;
        public override bool Autoload(ref string name){return false;}
    }
	public class EntModItem : EntModItemBase
	{
		public float critDMG = 1.5f;
		public float baseCD = 1.5f;
		public float basestat = 15;
		public float statchance = 15;
        public float comboDMG = 0.5f;
        public int combohits = 8;
        public float combotime = 1800;
        public float[] dmgratiobase = new float[15] {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1};
		public float[] dmgratio = new float[15] {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1};
        public int dmgbase = 50;
        public int realdmg = 50;
        public int basecrit = 15;
        public int realcrit = 15;
        public float normcritmult = 1;
        public float combospeedmult = 0;
        public float combostatmult = 0;
        public float combocritmult = 0;
        public int critcomboboost = 0;
        public int punchthrough = 0;
        public Item[] mods = new Item[8];
        public virtual bool reproc => false;
        public virtual bool realCombo => true;
        public virtual bool isGun => !realCombo;
        public float realstat {get{return combostatmult==0?statchance:statchance*(combostatmult+1)*comboMult(Main.player[item.owner].GetModPlayer<EntropyPlayer>(mod).combocounter, combohits, comboDMG);}}
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
            #pragma warning disable 0612
            for(int i = 0; i < 8; i++)if(tag.HasTag("mod"+i))mods[i] = tag.Get<Item>("mod"+i);
            #pragma warning restore 0612
        }
        public void ModEffect(EntModItemMod mod){
            if(mod!=null)ModEffectobsolete(mod.type, mod.level);
        }
        public virtual void PostSetDefaults(Player player){}
        public override void HoldItem(Player player){
            try{
                critcomboboost = 0;
                combocritmult = 0;
                combostatmult = 0;
                punchthrough = 0;
                //combospeedmult = 0;
                SetDefaults();
                PostSetDefaults(player);
                for(int i = 0; i < mods.Length; i++){
                    /*Entropy.*/ModEffect(mods[i]?.modItem as EntModItemMod);
                }
            }
            catch (System.Exception e){
                Main.NewText(e);
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
                case -1://no damage
                realdmg = 1;
                break;
                case 1://base damage
                realdmg += (int)(dmgbase * (level * 0.20));
                break;
                case 2://base cc
                realcrit += (int)(usedcrit * (level * 0.10));
                break;
                case 3://base cd
                critDMG += (baseCD * (float)(level * 0.15));
                break;
                case 4://br
                combocritmult = (float)(level * 0.15);
                //realcrit = (float)((item.crit + usedcrit) * (1 + (((1 + level) * 0.15)*(1+(comboDMG*modPlayer.comboget())))));
                //realcrit = (int)(basecrit * (1 + ((level * 0.15)*comboMult(modPlayer.combocounter, combohits, comboDMG))));
                break;
                case 5://ww
                combostatmult = (float)(level * 0.075);
                //statchance = (float)(basestat * (1 + ((level * 0.075)*comboMult(modPlayer.combocounter, combohits, comboDMG))));
                break;
                case 6://speed
                if(modPlayer.combocounter < combohits){
                    combospeedmult = 0;
                    break;
                }
                //item.useAnimation = (int)(item.useAnimation / (1 + (((1 + level) * 0.15)*(1+(comboDMG*modPlayer.comboget())))));
                //item.useTime = (int)(item.useTime / (1 + (((1 + level) * 0.15)*(1+(comboDMG*modPlayer.comboget())))));
                combospeedmult = (float)(level * 0.0001);
                break;
                case 7://base speed
                item.useTime = (int)(item.useTime*(1-(0.1*level)));
                item.useAnimation = (int)(item.useAnimation*(1-(0.1*level)));
                break;
                case 8://slash
                dmgratio[0] += (float)(dmgratiobase[0]*(0.15*level));
                break;
                case 9://impact
                dmgratio[1] += (float)(dmgratiobase[1]*(0.15*level));
                break;
                case 10://puncure
                dmgratio[2] += (float)(dmgratiobase[2]*(0.15*level));
                break;
                case 11://slash
                dmgratio[0] += (float)(dmgratiobase[0]*(0.2*level));
                break;
                case 12://impact
                dmgratio[1] += (float)(dmgratiobase[1]*(0.2*level));
                break;
                case 13://puncture
                dmgratio[2] += (float)(dmgratiobase[2]*(0.2*level));
                break;
                case 14://
                addElement(3, level*0.15f);
                break;
                case 15://
                addElement(5, level*0.15f);
                break;
                case 16://
                addElement(4, level*0.15f);
                break;
                case 17://
                addElement(6, level*0.15f);
                break;
                case 18://
                critcomboboost = (int)level;
                break;
                case 19://
                punchthrough += (int)level;
                break;
                default:
                break;
            }
        }
        //{3:"Cold", 4:"Electric", 5:"Heat", 6:"Toxic", 7:"Blast", 8:"Corrosive", 9:"Gas", 10:"Magnetic", 11:"Radiation", 12:"Viral"}
        public class elementCombo {
            public static elementCombo[] combos = new elementCombo[]{
                new elementCombo(7, new int[]{3,5}),
                new elementCombo(8, new int[]{4,6}),
                new elementCombo(9, new int[]{5,6}),
                new elementCombo(10, new int[]{3,4}),
                new elementCombo(11, new int[]{4,5}),
                new elementCombo(12, new int[]{3,6})
                };
            public readonly int result;
            public readonly int[] components;
            elementCombo(int r, int[] comps){
                result = r;
                components = comps;
            }
            /* public static elementCombo[] GetCombos(int element){
                elementCombo[] output = new elementCombo[3];
                int a = 0;
                for (int i = 0; i < combos.Length; i++){
                    if(combos[i].components[0]!=element && combos[i].components[1]!=element)continue;
                    output[a++] = combos[i];
                    if(a>3)break;
                }
                return output;
            } */
        }
        ///<summary>3:Cold, 4:Electric, 5:Heat, 6:Toxic, 7:Blast, 8:Corrosive, 9:Gas, 10:Magnetic, 11:Radiation, 12:Viral</summary>
        public void addElement(int element, float amount){
            for(int c = 0; c < elementCombo.combos.Length; c++){
                elementCombo ec = elementCombo.combos[c];
                int i = -1;
                if(ec.components[0]==element){
                    i = 0;
                }else if(ec.components[1]==element){
                    i = 1;
                }
                if(i<0)continue;
                if(dmgratio[ec.components[1-i]]>0){
                    dmgratio[ec.result]+=amount+dmgratio[ec.components[1-i]];
                    dmgratio[ec.components[1-i]] = 0;
                    return;
                }
                if(dmgratio[ec.result]>0){
                    dmgratio[ec.result]+=amount;
                    return;
                }
            }
            dmgratio[element]+=amount;
        }
		public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
			//HoldItem(Main.player[item.owner]);
			float[] dmgarray = Entropy.GetDmgRatio(Main.player[item.owner].GetWeaponDamage(item), dmgratio);
            #pragma warning disable 0618
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
							alltypesstring = alltypesstring + (Math.Round(dmgarray[i2], 2)+" "+Entropy.dmgtypes[i2].ToLower()+" damage");
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
                    if(!tooltips.Exists(isStat))tooltips.Insert(i+2, new TooltipLine(mod, "StatChance", Math.Round(realstat,2)+Lang.tip[5].Value.Replace("critical strike", "status")));
                }else if (tooltips[i].Name.Equals("CritDamage"))
                {
                    string ccString = Regex.Replace(Lang.tip[5].Value.Replace("%", "x")," [^ ]+$", "");
                    tooltips[i].text = critDMG+ccString+Regex.Replace(Lang.tip[2].Value, ".+ ", " ");//+Lang.tip[5].Value.Split()[1];
                }else if (tooltips[i].Name.Equals("StatChance")){
                    tooltips[i].text = Math.Round(realstat,2)+Lang.tip[5].Value.Replace("critical strike", "status");//+Lang.tip[5].Value.Split()[1];
                }
            }//*/
            #pragma warning restore 0618
        }
        static bool isCD(TooltipLine tl){
            return tl.Name.Equals("CritDamage");
        }
        static bool isStat(TooltipLine tl){
            return tl.Name.Equals("StatChance");
        }

        public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit){
			EntropyPlayer modPlayer = player.GetModPlayer<EntropyPlayer>(mod);
			modPlayer.comboadd(1);
            modPlayer.Buffs.RemoveAll(PlayerBuffBase.GC);
            foreach (PlayerBuffBase i in modPlayer.Buffs){
                i.ModifyHitItem(player, this, target, ref damage, ref crit, ref dmgratio);
            }
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
            if(crit && critcomboboost!=0)modPlayer.comboadd(critcomboboost);
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
            if(realCombo)dmg*= comboMult(((EntropyPlayer)player).combocounter, combohits, comboDMG);
            dmg/=3;
            damage = (int)dmg;
        }
        public override void GetWeaponCrit(Player player, ref int crit){
            crit = (int)(realcrit*(combocritmult==0?1:(combospeedmult+1)*comboMult(Main.player[item.owner].GetModPlayer<EntropyPlayer>(mod).combocounter, combohits, comboDMG)))-4;
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
            return combospeedmult==0?1:(combospeedmult+1)*comboMult(modPlayer.combocounter, combohits, comboDMG);
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
                    Entropy.mod.modItemUI.Deactivate();
                    Entropy.mod.modItemUI = null;
                    Entropy.mod.UI.SetState(null);
                }
                ((EntropyPlayer)Main.player[item.owner]).lastmoddeditem = getItemIndex(Main.player[item.owner].inventory);
                Entropy.mod.modItemUI = new ModItemsUI();
                Entropy.mod.modItemUI.Activate();
                Entropy.mod.UI.SetState(Entropy.mod.modItemUI);
            }
            timesinceright=7;
            return false;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
            int proj = Projectile.NewProjectile(position, new Vector2(speedX,speedY), type, damage, knockBack, item.owner);
            Main.projectile[proj].friendly = true;
            Main.projectile[proj].hostile = false;
            Main.projectile[proj].usesLocalNPCImmunity = true;
            Main.projectile[proj].localNPCHitCooldown = 9;
            Main.projectile[proj].maxPenetrate += punchthrough;
            Main.projectile[proj].penetrate += punchthrough;
            EntModProjectile p = Main.projectile[proj].modProjectile as EntModProjectile;
            if(p!=null){
                p.critDMG = critDMG;
                p.statchance = statchance;
                p.dmgratio = dmgratio;
                if(critcomboboost!=0)p.critcombo += Math.Sign(critcomboboost);
            }
            PostShoot(Main.projectile[proj]);
            return false;
        }
        public virtual void PostShoot(Projectile p){}
        static float comboMult(float cc, float ch, float cd){
            return (float)(Math.Max((Math.Floor(Math.Log(cc/ch, 4))*cd)+cd, 0)+1);
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