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
using Terraria.Graphics.Shaders;

namespace Entropy {
    public class EntropyPlayer : ModPlayer {
		public int combocounter = 0;
		public int combocountertime = 0;
        public int lastmoddeditem = 0;
		public int WorldonFiretime = 0;
		public const float InfernoMax = 3000;
		public float inferno = -1;
		public float infernorate = 0.5f;
        public List<PlayerBuffBase> Buffs = new List<PlayerBuffBase>{};
        public override bool Autoload(ref string name) {
            return true;
        }
        public float comboget(float ch, float cd){
            return EntModItem.comboMult(combocounter, ch, cd);
        }
        public float comboget(){
            EntModItem emi = player.HeldItem.modItem as EntModItem;
            float ch = 5;
            float cd = 0.5f;
            if(emi!=null){
                ch = emi.combohits;
                cd = emi.comboDMG;
            }
            return EntModItem.comboMult(combocounter, ch, cd);
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
            if(inferno>0){
                if(inferno>infernorate){
                    inferno-=infernorate;
                }else if(!player.CheckMana(player.manaRegenBuff?5:1, true)){
                    inferno = -1;
                }
            }
        }
        public override void SetControls(){
            if(WorldonFiretime>0){
                player.itemAnimation = 2;
                player.controlLeft = false;
                player.controlRight = false;
                player.controlJump = false;
                WorldonFiretime--;
            }
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
            if(player.controlUseTile&&player.HeldItem.type==ModContent.ItemType<CorrSniper>())zoom+=1.7f;
        }
        public override void ModifyDrawLayers(List<PlayerLayer> layers){
            if(inferno>0){
                if(player.Male){
                    FireHelm.visible = true;
                    layers.Add(FireHelm);
                }else{
                    FireHelm.visible = true;
                    int hi = layers.IndexOf(PlayerLayer.Head);
                    layers[hi] = FireHelm;
                }
                FireArm.visible = true;
                layers.Add(FireArm);
                FireLegs.visible = true;
                layers.Add(FireLegs);
                FireChest.visible = true;
                layers.Add(FireChest);
            }
        }
        public override void ModifyDrawInfo(ref PlayerDrawInfo drawInfo){
            if(inferno>0 && !player.Male){
                drawInfo.drawHair = true;
                drawInfo.drawPlayer.head = 0;
            }
        }
#region PlayerLayers
        public static PlayerLayer FireHelm = new PlayerLayer("Entropy", "FireArmorHead", null, delegate(PlayerDrawInfo drawInfo2){
            Player drawPlayer = drawInfo2.drawPlayer;
            if(drawPlayer.shadow!=0)return;
            Vector2 Position = new Vector2((float)((int)(drawInfo2.position.X - Main.screenPosition.X - (float)drawPlayer.bodyFrame.Width / 2f + (float)drawPlayer.width / 2f)), (float)((int)(drawInfo2.position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.headPosition + drawInfo2.headOrigin;
            Rectangle? Frame = new Rectangle?(drawPlayer.bodyFrame);
            Texture2D Texture = drawPlayer.Male?Main.armorHeadTexture[134]:Main.armorHeadTexture[181];
            if(!drawPlayer.Male)drawPlayer.head = 191;
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (drawPlayer.direction == -1){
                spriteEffects |= SpriteEffects.FlipHorizontally;
            }
            if (drawPlayer.gravDir == -1f){
                spriteEffects |= SpriteEffects.FlipVertically;
            }
            int a = (int)Math.Min(((EntropyPlayer.InfernoMax-drawPlayer.GetModPlayer<EntropyPlayer>().inferno*0.9)*drawPlayer.stealth*255)/EntropyPlayer.InfernoMax, 255);
            DrawData item = new DrawData(Texture, Position, Frame, new Color(a,a,a,a), drawPlayer.headRotation, drawInfo2.headOrigin, 1f, spriteEffects, 0);
            item.shader = GameShaders.Armor.GetShaderIdFromItemId(ItemID.SolarDye);
            Main.playerDrawData.Add(item);
        });
        public static PlayerLayer FireArm = new PlayerLayer("Entropy", "FireArmorArm", PlayerLayer.Arms, delegate(PlayerDrawInfo drawInfo2){
            Player drawPlayer = drawInfo2.drawPlayer;
            if(drawPlayer.shadow!=0)return;
            Vector2 Position = new Vector2((float)((int)(drawInfo2.position.X - Main.screenPosition.X - (float)drawPlayer.bodyFrame.Width / 2f + (float)drawPlayer.width / 2f)), (float)((int)(drawInfo2.position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + drawInfo2.bodyOrigin;
            Rectangle? Frame = new Rectangle?(drawPlayer.bodyFrame);
            Texture2D Texture = Main.armorArmTexture[177];
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (drawPlayer.direction == -1){
                spriteEffects |= SpriteEffects.FlipHorizontally;
            }
            if (drawPlayer.gravDir == -1f){
                spriteEffects |= SpriteEffects.FlipVertically;
            }
            int a = (int)Math.Min(((EntropyPlayer.InfernoMax-drawPlayer.GetModPlayer<EntropyPlayer>().inferno*0.75)*drawPlayer.stealth*190)/EntropyPlayer.InfernoMax, 255);
            DrawData item = new DrawData(Texture, Position, Frame, new Color(a,a,a,a), drawPlayer.bodyRotation, drawInfo2.bodyOrigin, 1f, spriteEffects, 0);
            item.shader = GameShaders.Armor.GetShaderIdFromItemId(ItemID.SolarDye);
            Main.playerDrawData.Add(item);
        });
        public static PlayerLayer FireLegs = new PlayerLayer("Entropy", "FireArmorLegs", PlayerLayer.Legs, delegate(PlayerDrawInfo drawInfo2){
            Player drawPlayer = drawInfo2.drawPlayer;
            if(drawPlayer.shadow!=0)return;
            Vector2 Position = new Vector2((float)((int)(drawInfo2.position.X - Main.screenPosition.X - (float)drawPlayer.bodyFrame.Width / 2f + (float)drawPlayer.width / 2f)), (float)((int)(drawInfo2.position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.legPosition + drawInfo2.legOrigin;
            Rectangle? Frame = new Rectangle?(drawPlayer.legFrame);
            Texture2D Texture = Main.armorLegTexture[130];
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (drawPlayer.direction == -1){
                spriteEffects |= SpriteEffects.FlipHorizontally;
            }
            if (drawPlayer.gravDir == -1f){
                spriteEffects |= SpriteEffects.FlipVertically;
            }
            int a = (int)Math.Min(((EntropyPlayer.InfernoMax-drawPlayer.GetModPlayer<EntropyPlayer>().inferno*0.75)*drawPlayer.stealth*190)/EntropyPlayer.InfernoMax, 255);
            DrawData item = new DrawData(Texture, Position, Frame, new Color(a,a,a,a), drawPlayer.legRotation, drawInfo2.legOrigin, 1f, spriteEffects, 0);
            item.shader = GameShaders.Armor.GetShaderIdFromItemId(ItemID.SolarDye);
            Main.playerDrawData.Add(item);
        });
        public static PlayerLayer FireChest = new PlayerLayer("Entropy", "FireArmorBody", PlayerLayer.Body, delegate(PlayerDrawInfo drawInfo2){
            Player drawPlayer = drawInfo2.drawPlayer;
            if(drawPlayer.shadow!=0)return;
            Vector2 Position = new Vector2((float)((int)(drawInfo2.position.X - Main.screenPosition.X - (float)drawPlayer.bodyFrame.Width / 2f + (float)drawPlayer.width / 2f)), (float)((int)(drawInfo2.position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + drawInfo2.bodyOrigin;
            Rectangle? Frame = new Rectangle?(drawPlayer.bodyFrame);
            Texture2D Texture = drawPlayer.Male?Main.armorBodyTexture[177]:Main.femaleBodyTexture[177];
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (drawPlayer.direction == -1){
                spriteEffects |= SpriteEffects.FlipHorizontally;
            }
            if (drawPlayer.gravDir == -1f){
                spriteEffects |= SpriteEffects.FlipVertically;
            }
            int a = (int)Math.Min(((EntropyPlayer.InfernoMax-drawPlayer.GetModPlayer<EntropyPlayer>().inferno*0.75)*drawPlayer.stealth*190)/EntropyPlayer.InfernoMax, 255);
            DrawData item = new DrawData(Texture, Position, Frame, new Color(a,a,a,a), drawPlayer.bodyRotation, drawInfo2.bodyOrigin, 1f, spriteEffects, 0);
            item.shader = GameShaders.Armor.GetShaderIdFromItemId(ItemID.SolarDye);
            Main.playerDrawData.Add(item);
        });
#endregion
        /*
        internal static PlayerLayer GetFireArmor(int type, int alpha){
            string name = "";
            switch (type){
                    case 0:
                    name = "Head";//currently 134 (spooky helmet), maybe 181 (Lazure's Valkyrie Circlet) or (Loki's helmet)
                    break;
                    case 1:
                    name = "Arms";//same as body
                    break;
                    case 2:
                    name = "Legs";//currently 130 (Stardust Legs)
                    break;
                    default:
                    name = "Body";//drawPlayer.Male?"Body":"Body";//currently 171 (Solar Armor), Maybe 175 (Vortex) for female
                    break;
                }
            //Texture2D Texture = Entropy.mod.GetTexture("Effects/FireArmor"+name);
            return new PlayerLayer("Entropy", "FireArmor"+name, delegate(PlayerDrawInfo drawInfo2){
	            Player drawPlayer = drawInfo2.drawPlayer;
                Vector2 Position = new Vector2((float)((int)(drawInfo2.position.X - Main.screenPosition.X - (float)drawPlayer.bodyFrame.Width / 2f + (float)drawPlayer.width / 2f)), (float)((int)(drawInfo2.position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + drawInfo2.bodyOrigin;
                Rectangle? Frame;
                Texture2D Texture;
                switch (type){
                    case 0:
                    Frame = new Rectangle?(drawPlayer.headFrame);
                    //name = "Head";//currently 134 (spooky helmet), maybe 181 (Lazure's Valkyrie Circlet) or (Loki's helmet)
                    Texture = Main.armorHeadTexture[drawPlayer.Male?134:181];
                    break;
                    case 1:
                    Frame = new Rectangle?(drawPlayer.bodyFrame);
                    //name = "Arms";//same as body
                    Texture = Main.armorArmTexture[177];
                    break;
                    case 2:
                    Frame = new Rectangle?(drawPlayer.legFrame);
                    //name = "Legs";//currently 130 (Stardust Legs)
                    Texture = Main.armorLegTexture[130];
                    break;
                    default:
                    Frame = new Rectangle?(drawPlayer.bodyFrame);
                    //name = "Body";//drawPlayer.Male?"Body":"Body";//currently 171 (Solar Armor), Maybe 175 (Vortex) for female
                    Texture = drawPlayer.Male?Main.armorBodyTexture[177]:Main.femaleBodyTexture[177];
                    break;
                }
                if (drawInfo2.shadow != 0f){
					return;
				}
				SpriteEffects spriteEffects = SpriteEffects.None;
				if (drawPlayer.direction == -1){
					spriteEffects |= SpriteEffects.FlipHorizontally;
				}
				if (drawPlayer.gravDir == -1f){
					spriteEffects |= SpriteEffects.FlipVertically;
				}
				DrawData item = new DrawData(Texture, Position, Frame, new Color(155,155,155,155), drawPlayer.bodyRotation, drawInfo2.bodyOrigin, 1f, spriteEffects, 0);
				item.shader = GameShaders.Armor.GetShaderIdFromItemId(ItemID.SolarDye);
				Main.playerDrawData.Add(item);
			});
        }//*/
        public static explicit operator EntropyPlayer(Player player){
            return player.GetModPlayer<EntropyPlayer>();
        }
        public static void AddBuff(PlayerBuffBase buff){
            buff.victim.GetModPlayer<EntropyPlayer>().Buffs.Add(buff);
        }
    }
}
