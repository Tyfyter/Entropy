using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;

namespace Entropy.Items{
	public class Willbreaker : EntModItem{
		int[] modsobsolete = new int[8] {6,3,0,0,0,0,0,0};
		int[] modlevelsobsolete = new int[8] {0,0,0,0,0,0,0,0};
		/*float[] dmgratiobase = new float[15] {0.47f,0.47f,0.06f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f};
		float[] dmgratio = new float[15] {0.47f,0.47f,0.06f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f};
		float critDMG = 2;
		float statchance = 15;
        int basecrit = 15;
		int realcrit = 15;
		//public EntModItem[] mods = new EntModItem[8] {EntModItem.New(), EntModItem.New(), EntModItem.New(), EntModItem.New(), EntModItem.New(), EntModItem.New(), EntModItem.New(), EntModItem.New()};
		int dmgbase = 30;*/
		//public EntModItem entmoditem;
		//Chest mods = new Chest();
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Willbreaker");
			Tooltip.SetDefault("Break your enemy's will! (and bones)");
		}
		public override void SetDefaults(){
            //Player owner = Main.player[item.owner];
			//item.name = "Willbreaker";
			//entmoditem = ((EntModItem)this);
            item.damage = realdmg = dmgbase = 125;
			item.melee = true;
			item.width = 40;
			item.height = 40;
			//item.toolTip = owner.meleeDamage+"\nBreak your enemy's will! (and bones) \n "+(int)(14*owner.meleeDamage)+" slash damage \n " +(int)(14*owner.meleeDamage)+ " impact damage \n " +(int)(2*owner.meleeDamage)+ " puncture damage";
            //item.toolTip2 = "" + Entropy.SlashCalcNPC((int)(14 * owner.meleeDamage)) + Entropy.ImpactCalcNPC((int)(14 * owner.meleeDamage)) + Entropy.PunctureCalcNPC((int)(2 * owner.meleeDamage));
			item.crit = 1;
			realcrit = basecrit;
            item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = 1;
			item.knockBack = 9;
			item.value = 10000;
			item.rare = 2;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.useTurn = true;
			dmgratio = dmgratiobase = new float[15] {0.47f,0.47f,0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.06f, 0f};
			statchance = basestat = 15;

			//item.shoot = ModContent.ProjectileType("FakeGrapple");
		}

		/*void ME(){
			for(int i = 0; i < modsobsolete.Length; i++){
				/*Entropy.ModEffectobsolete(modsobsolete[i], modlevelsobsolete[i]);
			}
		}

		public override void HoldItem(Player player){
			SetDefaults();
			ME();
			return;
            item.damage = dmgbase;
			item.melee = true;
			item.width = 40;
			item.height = 40;
			item.crit = 0;
            item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = 1;
			item.knockBack = 9;
			item.value = 10000;
			item.rare = 2;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			dmgratio = dmgratiobase = new float[15] {0.47f,0.47f,0.06f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f};
			critDMG = 2;
			statchance = 15;
			for(int i = 0; i < modsobsolete.Length; i++){
				/*Entropy.ModEffectobsolete(modsobsolete[i], modlevelsobsolete[i]);
			}
		}*/
		
		/*public void ModEffect(int modid, float level){
            switch (modid)
            {
                case 1:
                item.damage = (int)(item.damage * (1 + ((1 + level) * 20)));
                break;
                default:
                break;
            }
        }*/
		/*

		public override bool CanRightClick(){
			return true;
		}//*/

		/*
		public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
			//HoldItem(Main.player[item.owner]);
			float[] dmgarray = Entropy.GetDmgRatio((int)(item.damage * Main.player[item.owner].meleeDamage), dmgratio);
            for (int i = 0; i < tooltips.Count; i++)
            {
                if (tooltips[i].text.Contains("melee"))
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
					tip = new TooltipLine(mod, "damage", alltypesstring);
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
            }//*
		}*/

        /*public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
			EntropyPlayer modPlayer = player.GetModPlayer<EntropyPlayer>();
            //Main.NewText("precalc:"+damage, Color.White, true);
			//Main.NewText("ratio:"+Entropy.ToStringReal(dmgratio, ", "));
			float[] dmgarray = Entropy.GetDmgRatio(damage, dmgratio);
			damage = (int)Entropy.DmgCalcNPC(dmgarray, target);//(int)(Entropy.SlashCalcNPC(dmgarray[0], target) + Entropy.ImpactCalcNPC(dmgarray[1], target) + Entropy.PunctureCalcNPC(dmgarray[2], target));
            //Main.NewText("precrit:"+damage, Color.White, true);
			//damage = (int)(damage * ((modPlayer.comboadd()*0.5)+1));
			//Main.NewText("(("+statchance+") * (+1 + ((10 * 0.15)*(1+("+comboDMG+"*"+modPlayer.comboget()+")))))"+"="+(statchance * (1 + (((1 + 9) * 0.15)*(1+(comboDMG*modPlayer.comboget()))))), Color.White, true);
			//Main.NewText(damage+"*("+(modPlayer.comboadd()*0.5)+"+1)="+damage * ((modPlayer.comboadd()*0.5)+1));
			if(crit){
				damage /= 2;
				//damage = (int)(damage * critDMG);
			}
			Entropy.Proc(this, target, damage);
			/*if(crit){
				damage = (int)(damage/2);
			}*
			int cc = item.crit+player.meleeCrit;
			for(int i = cc; i > 0; i-=100){
				if(i < 100){
					if(i < Main.rand.Next(0,100))break;
				}
				damage = (int)(damage * critDMG);
			}
            //damage = Entropy.SlashCalcNPC((int)(14 * player.meleeDamage), target) + Entropy.ImpactCalcNPC((int)(14 * player.meleeDamage), target) + Entropy.PunctureCalcNPC((int)(2 * player.meleeDamage), target);
            //Main.NewText(""+Entropy.SlashCalcNPC((int)(14 * player.meleeDamage), target) + Entropy.ImpactCalcNPC((int)(14 * player.meleeDamage), target) + Entropy.PunctureCalcNPC((int)(2 * player.meleeDamage), target));
            /*if () {
                Lighting.AddLight(target.Center, 102, 0, 102);

            }*
            //base.ModifyHitNPC(player, target, ref damage, ref knockBack, ref crit);
        }*/
    }
}
