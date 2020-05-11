using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Enrtopy.Items;
using Entropy.Buffs;
using Entropy.Items;
using Entropy.NPCs;
using Entropy.Projectiles;
using Entropy.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace Entropy{
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
        public NarulSound ns = new NarulSound();
        public override void PostDrawInterface(SpriteBatch spriteBatch){
            Player player = Main.player[Main.myPlayer];
			EntropyPlayer modPlayer = player.GetModPlayer<EntropyPlayer>();
            /* if(player.HeldItem.modItem == null){
                return;
            }else  */
            EntModItemBase MI = player.HeldItem.modItem as EntModItemBase;
            if(MI==null){
                return;
            }else if(MI.IsMod){
				return;
			}else if(Main.playerInventory){
				return;
			}
			if(modPlayer.combocounter!=0){
                string st = (modPlayer.comboget() > 1 ? modPlayer.comboget()+"/"+(float)modPlayer.combocounter : (modPlayer.combocounter+""));
                Utils.DrawBorderStringFourWay(spriteBatch, Main.fontCombatText[1], st, Main.screenWidth*0.90f, Main.screenHeight*0.85f, Color.White, Color.Black, new Vector2(0.3f), 1);
            }
            if(MI is CompModItem){
                CompModItem CMI = (MI as CompModItem);
                int i = CMI.ability;
                string a = "";
                string b = "";
                for(int i2 = 1; i2 < CMI.maxabilities; i2++){
                    a+="●";
                    b+=" ";
                }
                Color selColor = Color.Aqua;
                Sekkal Sk = CMI as Sekkal;
                if(Sk!=null){
                    switch(Sk.element){
                        case 0:
                        selColor = Color.OrangeRed;
                        break;
                        case 2:
                        selColor = Color.MediumPurple;
                        break;
                        case 3:
                        selColor = Color.DarkGreen;
                        break;
                    }
                }
                Utils.DrawBorderStringFourWay(spriteBatch, Main.fontMouseText, /* "●●●" */a.Insert(i," "), Main.MouseScreen.X, Main.MouseScreen.Y+(Main.screenHeight/40), Color.White, Color.Black, new Vector2(0.3f), 1);
                Utils.DrawBorderStringFourWay(spriteBatch, Main.fontMouseText, /* "   " */b.Insert(i,"●"), Main.MouseScreen.X, Main.MouseScreen.Y+(Main.screenHeight/40), selColor, Color.Black, new Vector2(0.3f), 1);
            }
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
        //s:❪ i: p:↞ Viral:☣ Something:⌬
		public static string[] dmgtypes = new string[15] {" Slash", " Impact", " Puncture", " ❄ Cold", "⚡Electric", "♨Heat", "⎊Toxic", " Frostburn (❄+♨)", " Corrosive (⚡+⎊)", " Gas (♨+⎊)", " Magnetic (❄+⚡)", " Light (⚡+♨)", " Viral (❄+⎊)", "⚶True", " Dark"};
		public static string[] dmgcolor = new string[15] {"DDDDDD", "DDDDDD", "DDDDDD", "64EBFF", "4700CC", "E63D00", "3CB340", "0DDFFF", "32CD32", "3CB340", "3219C8", "FFFFFF", "0E660E", "AAAAAA", "5C0073"};
        
		public const int id_slash = 0, id_impact = 1, id_puncture = 2, id_cold = 3, id_electric = 4, id_heat = 5, id_toxic = 6, id_frostburn = 7, id_corrosive = 8, id_gas = 9, id_magnetic = 10, id_light = 11, id_viral = 12, id_true = 13, id_dark = 14;

        public static float[] GetDmgRatio(int dmg, float[] ratioarr, bool outputtext = false)
        {
			//int[ratioarr.Length] dmgarr;
			float[] output = new float[15];
            //string[] namearray = {"Slash", "Impact", "Puncture", "Cold", "Electric", "Heat", "Toxic", "Blast", "Corrosive", "Gas", "Magnetic", "Radiation", "Viral", "True", "Void"};
            if(outputtext)Main.NewText("array:"+ratioarr.ToStringReal(", ")+"; damage:"+dmg, Color.White, true);
			for(int i = 0; i < ratioarr.Length; i++){
				//output.Add((int)(dmg*ratioarr[i]));
				output[i] = (dmg*ratioarr[i]);
                if(outputtext)Main.NewText(dmgtypes[i]+":"+output[i], Color.White, true);
			}
            return output;
        }
        /* public override void PostAddRecipes(){
            Valhalla.InitClaws();
        } */
        //public List<int> Currencies = new List<int>(){};
        public override void Load(){
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
            /*try{
                EntropyPlayer.FireHelm = EntropyPlayer.GetFireArmor(0, 100);
                EntropyPlayer.FireArm = EntropyPlayer.GetFireArmor(1, 100);
                EntropyPlayer.FireLegs = EntropyPlayer.GetFireArmor(2, 100);
                EntropyPlayer.FireChest = EntropyPlayer.GetFireArmor(3, 100);
            }
            catch (System.Exception e){
                mod.Logger.Warn("Exception During PlayerLayer Loading: "+e);
            }*/
            //Currencies.Add(CustomCurrencyManager.RegisterCurrency(new CustomCData(ItemID.IronBar, 999L)));
            //AddSound(SoundType.Item, "Entropy/Sounds/Items/NarulSound", ns);
        }
        public override void Unload(){
            mod = null;
            UI = null;
            //Currencies = new List<int>(){};
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
					target.AddBuff(ModContent.BuffType("SlashProc"), 600);
				}else if(stattype == 1){
					target.AddBuff(ModContent.BuffType("SlashProc"), 600);
				}else if(stattype == 2){
					target.AddBuff(ModContent.BuffType("SlashProc"), 600);
				}else{*/
				//if(Entropy.dmgtypes[stattype] == "Slash" && !target.HasBuff(ModContent.BuffType(Entropy.dmgtypes[stattype]+"Proc")))target.AddBuff(ModContent.BuffType(Entropy.dmgtypes[stattype]+"Proc"), 1);
				//Main.NewText(Entropy.dmgtypes[stattype]+"Proc");
				//target.AddBuff(ModContent.BuffType(Entropy.dmgtypes[stattype]+"Proc"), (int)(item.item.damage*0.35));
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
					target.AddBuff(ModContent.BuffType("SlashProc"), 600);
				}else if(stattype == 1){
					target.AddBuff(ModContent.BuffType("SlashProc"), 600);
				}else if(stattype == 2){
					target.AddBuff(ModContent.BuffType("SlashProc"), 600);
				}else{*/
				//if(Entropy.dmgtypes[stattype] == "Slash" && !target.HasBuff(ModContent.BuffType(Entropy.dmgtypes[stattype]+"Proc")))target.AddBuff(ModContent.BuffType(Entropy.dmgtypes[stattype]+"Proc"), 1);
				//Main.NewText(Entropy.dmgtypes[stattype]+"Proc");
				//target.AddBuff(ModContent.BuffType(Entropy.dmgtypes[stattype]+"Proc"), (int)(item.item.damage*0.35));
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
					target.AddBuff(ModContent.BuffType("SlashProc"), 600);
				}else if(stattype == 1){
					target.AddBuff(ModContent.BuffType("SlashProc"), 600);
				}else if(stattype == 2){
					target.AddBuff(ModContent.BuffType("SlashProc"), 600);
				}else{
				//if(Entropy.dmgtypes[stattype] == "Slash" && !target.HasBuff(ModContent.BuffType(Entropy.dmgtypes[stattype]+"Proc")))target.AddBuff(ModContent.BuffType(Entropy.dmgtypes[stattype]+"Proc"), 1);
				//Main.NewText(Entropy.dmgtypes[stattype]+"Proc");
				//target.AddBuff(ModContent.BuffType(Entropy.dmgtypes[stattype]+"Proc"), (int)(item.item.damage*0.35));
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

        public Entropy(){
			Properties = new ModProperties(){
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
        public static T GetBuff<T>(this NPC npc) where T : BuffBase{
            foreach (BuffBase item in npc.GetGlobalNPC<EntropyGlobalNPC>().Buffs)if(item is T i)return i;
            return null;
        }
        public static List<T> GetBuffs<T>(this NPC npc) where T : BuffBase{
            List<T> buffs = new List<T>(){};
            foreach (BuffBase item in npc.GetGlobalNPC<EntropyGlobalNPC>().Buffs)if(item is T i)buffs.Add(i);
            return buffs;
        }
        public static int CountBuff<T>(this NPC npc) where T : BuffBase{
            int o = 0;
            foreach (BuffBase item in npc.GetGlobalNPC<EntropyGlobalNPC>().Buffs)if(item is T)o++;
            return o;
        }
        public static int CountBuff(this NPC npc, Type T){
            int o = 0;
            foreach (BuffBase item in npc.GetGlobalNPC<EntropyGlobalNPC>().Buffs)if(item.GetType()==T)o++;
            return o;
        }
        public static bool CanAttack(this NPC npc){
            foreach (BuffBase item in npc.GetGlobalNPC<EntropyGlobalNPC>().Buffs)if(item is ImpactEffect || item is BlastEffect || item is SleepEffect || item is YoteEffect)return false;
            return true;
        }
        public static bool HasBuff<T>(this Player player) where T : PlayerBuffBase{
            foreach (PlayerBuffBase item in player.GetModPlayer<EntropyPlayer>().Buffs)if(item is T)return true;
            return false;
        }
        public static T GetBuff<T>(this Player player) where T : PlayerBuffBase{
            foreach (PlayerBuffBase item in player.GetModPlayer<EntropyPlayer>().Buffs)if(item is T)return (T)item;
            return null;
        }
        public static Vector2 lerp(Vector2 a, Vector2 b, float c){
            //Main.NewText(a+" and "+b+" lerped by "+c+" equal "+((a*(1-c))+(b*c)));
            return (a*(1-c))+(b*c);
        }
        public static float constrain(float i, float low, float high){
            //Main.NewText(high+">"+i+">"+low);
            return i<low?low:(i>high?high:i);
        }
        public static Vector2 constrain(Vector2 i, Vector2 low, Vector2 high){
            //Main.NewText(high+">"+i+">"+low);
            float x = i.X<low.X?low.X:(i.X>high.X?high.X:i.X);
            float y = i.Y<low.Y?low.Y:(i.Y>high.Y?high.Y:i.Y);
            return new Vector2(x, y);
        }
        public static Vector2 constrained(this Vector2 i, Vector2 low, Vector2 high){
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
        public static float[] Sum(this float[] a, float[] b){
            int i = 0;
            return a.Select(x => x + b[i++%b.Length]).ToArray();
        }
		public static Vector2 toVector2(this Vector3 i){
			return new Vector2(i.X,i.Y);
		}
        public static Rectangle ProperBox(Point a, Point b){
			int x1,x2,y1,y2;
			if(b.X>a.X){
				x1=a.X;
				x2=b.X-a.X;
			}else{
				x1=b.X;
				x2=a.X-b.X;
			}
			if(b.Y>a.Y){
				y1=a.Y;
				y2=b.Y-a.Y;
			}else{
				y1=b.Y;
				y2=a.Y-b.Y;
			}
			return new Rectangle(x1,y1,x2,y2);
        }
        public static Rectangle ProperBox(Vector2 a, Vector2 b){

			return ProperBox(a.ToPoint(),b.ToPoint());
        }
        public static Rectangle ProperBox(this Ray ray){
			return ProperBox(ray.Position.toVector2(), (ray.Position+ray.Direction).toVector2());
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
