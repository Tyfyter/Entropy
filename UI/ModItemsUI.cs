using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;
using Entropy.Items;
using System;
using Entropy.Items.Mods;

namespace Entropy.UI{
	// This class represents the UIState for the "Gungeon Bandoleer"
	public class ModItemsUI : UIState {
        public List<VanillaItemSlotWrapper> itemSlots = new List<VanillaItemSlotWrapper>(){};
        public List<EntModItemMod> items{
            get{
                List<EntModItemMod> mods = new List<EntModItemMod>(){};
                foreach (VanillaItemSlotWrapper Is in itemSlots){
                    mods.Add(Is.Item.modItem as EntModItemMod);
                }
                return mods;
            }
        }
        public Item item{
            get{return Main.LocalPlayer.inventory[((EntropyPlayer)Main.LocalPlayer).lastmoddeditem];}
            set{if(((EntropyPlayer)Main.LocalPlayer).lastmoddeditem>=0)Main.LocalPlayer.inventory[((EntropyPlayer)Main.LocalPlayer).lastmoddeditem] = value;}
        }//*/
        //public List<Item> passives;
        public int itemId;
        public override void OnInitialize(){
            for (int i = 0; i < 8; i++){
                if(i>=itemSlots.Count)itemSlots.Add(null);
                itemSlots[i] = new VanillaItemSlotWrapper(scale:0.575f, index:i){
				Left = { Pixels = 570+((i-(i%4))*8.25f) },
                Top = { Pixels = 105+((i%4)*33) },
                ValidItemFunc = (it, index) => it.IsAir||(validItemFunc(it, items, index)&&canUseCheck(it, item))
				};
                if(item!=null)if(!item.IsAir){
                    if(((EntModItem)item.modItem).mods.Length>i){
                        if(((EntModItem)item.modItem).mods[i]!=null)itemSlots[i].Item = ((EntModItem)item.modItem).mods[i];
                    }
                }
                Append(itemSlots[i]);
            }
            //if(((EntropyPlayer)Main.LocalPlayer).lastmoddeditem==-1)this.Deactivate();
            //Main.NewText(((GunItemBelt)RefTheGun.mod.gunItemUI.item.modItem).passives.Count+":"+passives.Count);
        }
        static int scanIndex = 0;
        public static bool validItemFunc(Item item, List<EntModItemMod> items, int index){
            int? c = (item?.modItem as EntModItemMod)?.type;
            if(c==null)return false;
            //EntModItemMod.compared = c.Value;
            //(item?.modItem as EntModItemMod)?.type
            scanIndex = 0;
            return !items.Exists((it) => (scanIndex++!=index)&&(c == it?.type));
        }
        static bool canUseCheck(Item it, Item item){
            if(it.IsAir)return true;
            EntModItemMod mit = it?.modItem as EntModItemMod;
            EntModItem mitem = item?.modItem as EntModItem;
            return mit.CanApply(mitem);
        }
        static T writeVal<T>(T i, string id){
            Main.NewText(id+i);
            return i;
        }
        public override void Update(GameTime gameTime){
            base.Update(gameTime);
            if(item.IsAir){
                this.Deactivate();
                //Entropy.mod.modItemUI = null;
                Entropy.mod.UI.SetState(null);
            }
            /*for (int i = 0; i < itemSlots.Count; i++)if(item!=null)if(!item.IsAir)if(itemSlots[i].Item.modItem!=null){
                //((HeartItemBase)heartSlots[i].Item.modItem).index = i;
            }*/
        } 
        /* public void a(ModItem b){
            item = b.item;
        } */
		public override void OnDeactivate(){
            if(item.IsAir||item.modItem==null||item.modItem.mod.Name!=Entropy.mod.Name){
                if(!Main.LocalPlayer.HeldItem.IsAir)UpdateFloatingItem();
                return;
            }
			UpdateItem();
		}
		public void UpdateItem(){
            List<Item> mods = new List<Item>(){};
            for(int i = 0; i < itemSlots.Count; i++)mods.Add(null);
			for(int i = 0; i < itemSlots.Count; i++)mods[i] = itemSlots[i].Item;
            if(itemSlots.Count<((EntModItem)item.modItem).mods.Length)for (int i = itemSlots.Count; i < ((EntModItem)item.modItem).mods.Length; i++){
                mods.Add(((EntModItem)item.modItem).mods[i]);
            }
            ((EntModItem)item.modItem).mods = mods.ToArray();
            //((GunItemBelt)item.modItem).pcount = ((GunItemBelt)item.modItem).passives.Count;
            //Main.NewText(passives.Count+";"+((GunItemBelt)item.modItem).passives[0].Name+";"+((GunItemBelt)item.modItem).pcount);
		}
		public void UpdateFloatingItem(){
            if(item!=null&&!item.IsAir)return;
            Item item2 = Main.LocalPlayer?.HeldItem;
            if(item2==null||item2.IsAir||item2.modItem==null||item2.modItem.mod.Name!=Entropy.mod.Name)return;
            List<Item> mods = new List<Item>(){};
            for(int i = 0; i < itemSlots.Count; i++)mods.Add(null);
            for(int i = 0; i < itemSlots.Count; i++)mods[i] = itemSlots[i].Item;
            if(itemSlots.Count<((EntModItem)item2.modItem).mods?.Length)for(int i = itemSlots.Count; i < ((EntModItem)item2.modItem).mods.Length; i++){
                mods.Add(((EntModItem)item2.modItem).mods[i]);
            }
            ((EntModItem)item2.modItem).mods = mods.ToArray();
            //((GunItemBelt)item.modItem).pcount = ((GunItemBelt)item.modItem).passives.Count;
            //Main.NewText(passives.Count+";"+((GunItemBelt)item.modItem).passives[0].Name+";"+((GunItemBelt)item.modItem).pcount);
		}
    }
}