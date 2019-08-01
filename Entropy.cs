using System;
using System.Collections.Generic;
using Entropy.Buffs;
using Entropy.Items;
using Entropy.NPCs;
using Entropy.Projectiles;
using Entropy.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace Entropy
{
	public class Entropy : Mod {
        /*public static void ModEffect(Items.EntModItem item, int modid, float level){
            switch (modid)
            {
                case 1:
                item.ModifyStats("item.damage = (int)(item.damage * (1 + ((1 + level) * 20)));");
                break;
                default:
                break;
            }
        }*/
        public static Entropy mod;
		internal UserInterface UI;
		public ModItemsUI modItemUI;
        public override void PostDrawInterface(SpriteBatch spriteBatch){
            Player player = Main.player[Main.myPlayer];
			EntropyPlayer modPlayer = player.GetModPlayer<EntropyPlayer>(mod);
            /* if(player.HeldItem.modItem == null){
                return;
            }else  */if(player.HeldItem.modItem?.mod!=mod){
                return;
            }else if(((EntModItemBase)player.HeldItem.modItem).IsMod){
				return;
			}else if(Main.playerInventory){
				return;
			}
			if(modPlayer.combocounter!=0)Utils.DrawBorderStringFourWay(spriteBatch, Main.fontCombatText[1], (modPlayer.comboget() > 1 ? modPlayer.comboget()+"/"+(float)modPlayer.combocounter : (modPlayer.combocounter+"")), Main.screenWidth*0.90f, Main.screenHeight*0.85f, Color.White, Color.Black, new Vector2(0.3f), 1);
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
			int inventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
			if (inventoryIndex != -1) {
				layers.Insert(inventoryIndex + 1, new LegacyGameInterfaceLayer(
					"Entropy: ModUI",
					delegate {
						// If the current UIState of the UserInterface is null, nothing will draw. We don't need to track a separate .visible value.
						UI.Draw(Main.spriteBatch, new GameTime());
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}
		public override void UpdateUI(GameTime gameTime) {
            if(mod.UI.CurrentState!=null&&!Main.playerInventory){
				UI.SetState(null);
			}
			UI?.Update(gameTime);
		}
		public static string[] dmgtypes = new string[15] {"Slash", "Impact", "Puncture", "Cold", "Electric", "Heat", "Toxic", "Blast", "Corrosive", "Gas", "Magnetic", "Radiation", "Viral", "True", "Void"};

        public static float[] GetDmgRatio(int dmg, float[] ratioarr, bool outputtext = false)
        {
			//int[ratioarr.Length] dmgarr;
			float[] output = new float[15];
            string[] namearray = {"Slash", "Impact", "Puncture", "Cold", "Electric", "Heat", "Toxic", "Blast", "Corrosive", "Gas", "Magnetic", "Radiation", "Viral", "True", "Void"};
            if(outputtext)Main.NewText("array:"+ratioarr.ToStringReal(", ")+"; damage:"+dmg, Color.White, true);
			for(int i = 0; i < ratioarr.Length; i++){
				//output.Add((int)(dmg*ratioarr[i]));
				output[i] = (dmg*ratioarr[i]);
                if(outputtext)Main.NewText(namearray[i]+":"+output[i], Color.White, true);
			}
            return output;
        }
        public override void Load()
        {
            mod = this;
            Properties = new ModProperties()
            {
                Autoload = true,
                AutoloadGores = true,
                AutoloadSounds = true
            };
			if (!Main.dedServ){
				UI = new UserInterface();
			}
        }
        public static short SetStaticDefaultsGlowMask(ModItem modItem, string suffix = "_Glow")
        {
            if (!Main.dedServ)
            {
                Texture2D[] glowMasks = new Texture2D[Main.glowMaskTexture.Length + 1];
                for (int i = 0; i < Main.glowMaskTexture.Length; i++)
                {
                    glowMasks[i] = Main.glowMaskTexture[i];
                }
                glowMasks[glowMasks.Length - 1] = mod.GetTexture("Items/" + modItem.GetType().Name + suffix);
                Main.glowMaskTexture = glowMasks;
                return (short)(glowMasks.Length - 1);
            }
            else return 0;
        }
        public static void Proc(EntModItem item, NPC target, int damage){
            int statloss = 0;
            float basestat = item.realstat;
            reproc:
            float stat = statloss==0?basestat:(basestat-(100*statloss))/(statloss+1);
			if(stat >= Main.rand.NextFloat(0, 100)){
				int stattype = Entropy.StatTypeCalc(item.dmgratio);
				/*if(stattype == 0){
					target.AddBuff(mod.BuffType("SlashProc"), 600);
				}else if(stattype == 1){
					target.AddBuff(mod.BuffType("SlashProc"), 600);
				}else if(stattype == 2){
					target.AddBuff(mod.BuffType("SlashProc"), 600);
				}else{*/
				//if(Entropy.dmgtypes[stattype] == "Slash" && !target.HasBuff(mod.BuffType(Entropy.dmgtypes[stattype]+"Proc")))target.AddBuff(mod.BuffType(Entropy.dmgtypes[stattype]+"Proc"), 1);
				//Main.NewText(Entropy.dmgtypes[stattype]+"Proc");
				//target.AddBuff(mod.BuffType(Entropy.dmgtypes[stattype]+"Proc"), (int)(item.item.damage*0.35));
                BuffBase buff = BuffBase.GetFromIndex(target, stattype, damage, Main.player[item.item.owner]);
                EntropyGlobalNPC.AddBuff(buff);
                //Main.NewText(buff);
				//Dust.NewDust(target.Center - new Vector2(0,target.height/2),0,0,mod.DustType(Entropy.dmgtypes[stattype]+"ProcDust"));
				//}

			}
            if(item.reproc&&(stat-100)>0){
                statloss++;
                goto reproc;
            }
        }
        public static void Proc(EntModProjectile proj, NPC target, int damage){
            int statloss = 0;
            float basestat = proj.statchance;
            reproc:
            float stat = statloss==0?basestat:(basestat-(100*statloss))/(statloss+1);
			if(stat >= Main.rand.NextFloat(0, 100)){
				int stattype = Entropy.StatTypeCalc(proj.dmgratio);
				/*if(stattype == 0){
					target.AddBuff(mod.BuffType("SlashProc"), 600);
				}else if(stattype == 1){
					target.AddBuff(mod.BuffType("SlashProc"), 600);
				}else if(stattype == 2){
					target.AddBuff(mod.BuffType("SlashProc"), 600);
				}else{*/
				//if(Entropy.dmgtypes[stattype] == "Slash" && !target.HasBuff(mod.BuffType(Entropy.dmgtypes[stattype]+"Proc")))target.AddBuff(mod.BuffType(Entropy.dmgtypes[stattype]+"Proc"), 1);
				//Main.NewText(Entropy.dmgtypes[stattype]+"Proc");
				//target.AddBuff(mod.BuffType(Entropy.dmgtypes[stattype]+"Proc"), (int)(item.item.damage*0.35));
                BuffBase buff = BuffBase.GetFromIndex(target, stattype, damage, Main.player[proj.projectile.owner]);
                EntropyGlobalNPC.AddBuff(buff);
                //Main.NewText(buff);
				//Dust.NewDust(target.Center - new Vector2(0,target.height/2),0,0,mod.DustType(Entropy.dmgtypes[stattype]+"ProcDust"));
				//}

			}
            if(proj.reproc&&(stat-100)>0){
                statloss++;
                goto reproc;
            }
        }
        /* public static void Proc(EntModProjectile proj, NPC target, int damage){
			if(proj.statchance >= Main.rand.NextFloat(0, 100)){
				int stattype = Entropy.StatTypeCalc(proj.dmgratio);
				/*if(stattype == 0){
					target.AddBuff(mod.BuffType("SlashProc"), 600);
				}else if(stattype == 1){
					target.AddBuff(mod.BuffType("SlashProc"), 600);
				}else if(stattype == 2){
					target.AddBuff(mod.BuffType("SlashProc"), 600);
				}else{
				//if(Entropy.dmgtypes[stattype] == "Slash" && !target.HasBuff(mod.BuffType(Entropy.dmgtypes[stattype]+"Proc")))target.AddBuff(mod.BuffType(Entropy.dmgtypes[stattype]+"Proc"), 1);
				//Main.NewText(Entropy.dmgtypes[stattype]+"Proc");
				//target.AddBuff(mod.BuffType(Entropy.dmgtypes[stattype]+"Proc"), (int)(item.item.damage*0.35));
                BuffBase buff = BuffBase.GetFromIndex(target, stattype, damage);
                EntropyGlobalNPC.AddBuff(buff);
                //Main.NewText(buff);
				Dust.NewDust(target.Center - new Vector2(0,target.height/2),0,0,mod.DustType(Entropy.dmgtypes[stattype]+"ProcDust"));
				//}

			}
        } */
        public static int StatTypeCalc(float[] ratioarr){
            float totalweight = 0;
            float[] ratioarr2 = (float[])ratioarr.Clone();
            ratioarr2[0] *= 4;
            ratioarr2[1] *= 4;
            ratioarr2[2] *= 4;
            for (int i = 0; i < ratioarr2.Length; i++)
            {
                totalweight += ratioarr2[i];
            }
            float rando = Main.rand.NextFloat(0, totalweight);
            totalweight = 0;
            for (int i = 0; i < ratioarr2.Length; i++)
            {
                if(rando >= totalweight && rando < totalweight + ratioarr2[i]){
                    //Main.NewText(rando+";"+totalweight+";"+ratioarr2[i]);
                    return i;
                }
                totalweight += ratioarr2[i];
            }
            return 0;
        }
        public static float DmgCalcNPC(float dmg, NPC target, int type){
            dmg = DmgCalcs.dmgFuncs[type].Invoke(dmg,target);
            return Math.Max(dmg, 0);
        }
        public static float DmgCalcNPC(float[] dmg, NPC target){
            float damage = 0;
            for(int i = 0; i<15; i++)damage+=DmgCalcs.dmgFuncs[i].Invoke(dmg[i],target);//target.GetGlobalNPC<EntropyGlobalNPC>().dmgResist[i];
            return Math.Max(damage, 0);
        }/*
        public static float SlashCalcNPC(float dmg, NPC target)
        {
            dmg = dmg * target.GetGlobalNPC<EntropyGlobalNPC>().dmgResist[0];
            return Math.Max(target.HasBuff(BuffID.Stoned)?dmg/2:dmg, 0);
            float a = dmg;
            //Main.NewText("dmg1a:"+a, Color.White, true);
            for (int i = 0; i < target.buffType.Length; i++)
            {
                if (target.buffType[i] == 156)
                {
                    a /= 3;
                };
            }
            if (target.type == Terraria.ID.NPCID.BoneSerpentHead || target.type == Terraria.ID.NPCID.BoneSerpentBody || target.type == Terraria.ID.NPCID.BoneSerpentTail)
            {
                a = (float)(a / 1.3);
            }else if(target.TypeName.ToLower().Contains("slime")){
                a = (float)(a * 1.3);
            }
            //Main.NewText("dmg1b:"+a, Color.White, true);
            return Math.Max(a, 0);
        }
        public static float ImpactCalcNPC(float dmg, NPC target)
        {
            dmg = dmg * target.GetGlobalNPC<EntropyGlobalNPC>().dmgResist[0];
            return Math.Max(dmg, 0);
            float a = dmg;
            //Main.NewText("dmg2a:"+a, Color.White, true);
            for (int i = 0; i < target.buffType.Length; i++)
            {
                if (target.buffType[i] == 156)
                {
                    a = (float)(a / 1.3);
                };
            }
            //Main.NewText("dmg2b:"+a, Color.White, true);
            return Math.Max(a, 0);
        }
        public static float PunctureCalcNPC(float dmg, NPC target)
        {
            dmg = dmg * target.GetGlobalNPC<EntropyGlobalNPC>().dmgResist[0];
            return Math.Max(target.HasBuff(BuffID.Stoned)?dmg*1.2f:dmg, 0);
            float a = dmg;
            //Main.NewText("dmg3a:"+a, Color.White, true);
            for (int i = 0; i < target.buffType.Length; i++)
            {
                if (target.buffType[i] == 156)
                {
                    a = (float)(a * 1.3);
                };
            }
            if (target.type == Terraria.ID.NPCID.BoneSerpentHead || target.type == Terraria.ID.NPCID.BoneSerpentBody || target.type == Terraria.ID.NPCID.BoneSerpentTail)
            {
                a = (float)(a * 1.3);
            }
            //Main.NewText("dmg3b:"+a, Color.White, true);
            return Math.Max(a, 0);
        }*/

        public Entropy()
		{
			Properties = new ModProperties()
			{
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
		}
	}
    public static class EntropyExt{
        public static string ToStringReal<T>(this T[] input, string seperator = ""){
            string output = "";
            for(int i = 0; i < input.Length; i++){
                output = output + (i != 0 ? seperator : "") + input[i];
            }
            return output;
        }
        public static bool HasBuff<T>(this NPC npc) where T : BuffBase{
            foreach (BuffBase item in npc.GetGlobalNPC<EntropyGlobalNPC>().Buffs)if(item is T)return true;
            return false;
        }
        public static int CountBuff<T>(this NPC npc) where T : BuffBase{
            int o = 0;
            foreach (BuffBase item in npc.GetGlobalNPC<EntropyGlobalNPC>().Buffs)if(item is T)o++;
            return o;
        }
        public static Vector2 constrain(Vector2 i, Vector2 low, Vector2 high){
            //Main.NewText(high+">"+i+">"+low);
            float x = i.X<low.X?low.X:(i.X>high.X?high.X:i.X);
            float y = i.Y<low.Y?low.Y:(i.Y>high.Y?high.Y:i.Y);
            return new Vector2(x, y);
        }
        public static Vector2 getHandPos(this Player player){
            if(((!(player.velocity.Y > 0))&&!(player.wingTime!=player.wingTimeMax)) || player.sliding){
                return (player.direction==1?player.Left:player.Right)+new Vector2(0,8);
            }else{
                return (player.direction==1?player.TopLeft:player.TopRight)+new Vector2(0,8);
            }
        }
    }
    public class TrueNullable<T> {
        public T value;
        public TrueNullable(T value){
            this.value=value;
        }
        public static implicit operator T(TrueNullable<T> input){
            return input.value;
        }
        public static implicit operator TrueNullable<T>(T input){
            return new TrueNullable<T>(input);
        }
        public override string ToString(){
            return value.ToString()+"?";
        }
    }
}
