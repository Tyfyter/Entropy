using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework.Graphics;
using Entropy.Items;
using Terraria.GameInput;
using Entropy.Buffs;

namespace Entropy {
    public class EntropyPlayer : ModPlayer {
		public int combocounter = 0;
        
		public int combocountertime = 0;

        public float QTeff = 1;
        public int lastmoddeditem = 0;
        public List<PlayerBuffBase> Buffs = new List<PlayerBuffBase>{};
        public override bool Autoload(ref string name) {
            return true;
        }
        static float comboMult(float cc, float ch, float cd){
            return (float)(Math.Max((Math.Floor(Math.Log(cc/ch, 4))*cd)+cd, 0)+1);
        }
        public float comboget(float ch, float cd){
            return comboMult(combocounter, ch, cd);
        }
        public float comboget(){
            EntModItem emi = player.HeldItem.modItem as EntModItem;
            float ch = 5;
            float cd = 0.5f;
            if(emi!=null){
                ch = emi.combohits;
                cd = emi.comboDMG;
            }
            return comboMult(combocounter, ch, cd);
        }
        public float comboadd(int amount = 1, int duration = 180, float ch = 5, float cd = 0.5f){
            combocounter+=amount;
            combocountertime = Math.Max(combocountertime, duration);
            return comboget(ch, cd);//(float)(Math.Floor(Math.Max((Math.Log(combocounter/5, 3)/2)+0.5f, 0)*2)/2)+1;//Math.Floor(Math.Max(Math.Log(combocounter/5,3)*2, 0))/2
        }

        public override void ResetEffects()
        {
            combocountertime = Math.Max(combocountertime-1, 0);
            if(combocountertime == 0 && combocounter > 0){
                combocounter--;
                //combocounter = 0;
            }
            foreach (PlayerBuffBase i in Buffs){
                i.Update(player);
            }
            Buffs.RemoveAll(PlayerBuffBase.GC);
        }
        public override void SetControls(){
            if(!player.controlTorch)return;
            CompModItem item = player.HeldItem?.modItem as CompModItem;
            if(item==null)return;
            player.controlTorch = false;
            if(Math.Abs(PlayerInput.ScrollWheelDelta)>=60){
                Main.PlaySound(12, player.Center);
                item.tryScroll(PlayerInput.ScrollWheelDelta / -120);
                PlayerInput.ScrollWheelDelta = 0;
            }
            for(int i = 1; i <= item.maxabilities; i++){
                string s = "Hotbar"+(i==10?0:i);
                if(PlayerInput.Triggers.JustPressed.KeyStatus[s]){
                    item.ability = i-1;
                    //int h = player.selectedItem;
                    PlayerInput.Triggers.Old.KeyStatus[s] = false;
                    PlayerInput.Triggers.Current.KeyStatus[s] = false;
                    PlayerInput.Triggers.JustPressed.KeyStatus[s] = false;
                    PlayerInput.Triggers.JustReleased.KeyStatus[s] = false;
                    //player.controlUseTile = true;
                    Main.PlaySound(12, player.Center);
                    //player.selectedItem = h;
                    break;
                }
            }
        }
        public override void ModifyZoom(ref float zoom){
            if(player.controlUseTile&&player.HeldItem.type==mod.ItemType<CorrSniper>())zoom+=1.7f;
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource){
            if(damage >= player.statLife-1 && damage < player.statLife + player.statMana && QTeff != 0){
                if(QTeff > 0){
                    if(player.CheckMana((int)(damage/QTeff), true)){
                        damage = 0;
                    }else if(player.statMana >= 1){
                        damage -= (int)(player.statMana*QTeff);
                        player.CheckMana(player.statMana, true);
                    }
                }else{
                    player.CheckMana((int)(damage/QTeff), true);
                }
            }
            return true;
        }
        public static explicit operator EntropyPlayer(Player player){
            return player.GetModPlayer<EntropyPlayer>();
        }
        public static void AddBuff(PlayerBuffBase buff){
            buff.victim.GetModPlayer<EntropyPlayer>().Buffs.Add(buff);
        }
    }
}
