using System;
using System.Collections.Generic;
using Entropy.Buffs;
using Entropy.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using static Entropy.EntropyExt;
 
namespace Entropy.Items{
    public class Ice_Revolver : EntModItem
    {
        public int RoundsLeft = 9;
        public static int RoundsMax = 9;
        private int reloading = 0;
        private int passivereload = 0;
		private int reloaddelay = 0;
        public static short customGlowMask = 0;
		bool boosted = false;
		bool boosting = false;
		public override bool isGun => true;
		public override bool CloneNewInstances => true;
        public override void SetDefaults()
        {
            //item.name = "lightning";          
            item.damage = 45;//realdmg = dmgbase = 125;                        
			item.ranged = true;
            item.width = 24;
            item.height = 28;
			item.useTime = 1;
			item.useAnimation = 15;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 7.5f;        
            item.value = 1000;
            item.rare = ItemRarityID.Cyan;
			item.alpha = 100;
            item.autoReuse = false;
			item.shoot = ModContent.ProjectileType<IceShotProj>();
            item.shootSpeed = 12.5f;
			realcrit = basecrit = 25;
			dmgratio = dmgratiobase = new float[15] {0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f};
			combohits = RoundsMax;
			comboDMG = RoundsLeft>0?RoundsLeft-1:1;
            item.glowMask = customGlowMask;
        }      
		
		public override void SetStaticDefaults(){
		  DisplayName.SetDefault("Ateriâ");
		  Tooltip.SetDefault("DisplayAmmo");
          customGlowMask = Entropy.SetStaticDefaultsGlowMask(this,"");
		}
		public override void AddRecipes(){
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.StaffoftheFrostHydra, 1);
			recipe.AddIngredient(ItemID.SoulofMight, 5);
			recipe.AddIngredient(ItemID.Ruby, 2);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
		public override Vector2? HoldoutOffset(){
			return new Vector2(-1,3);
		}
		public override void HoldItem (Player player){
			base.HoldItem(player);
			addElement(3, 0.15f);
			dmgratio[3]+=0.85f;
            EntropyPlayer modPlayer = player.GetModPlayer<EntropyPlayer>();
			modPlayer.combocounter = RoundsLeft>0?RoundsMax:0;
            item.autoReuse = false;
			reloading = Math.Max(reloading, 0);   
			if(RoundsLeft <= 0 && reloading == 0)reloading = 1;
		}
		
		public override void HoldStyle (Player player){
            //Main.dust[dust3].noGravity = true;
            //Main.dust[dust3].velocity = new Vector2(0, 0);
			boosting = false;
			boosted = false;
			reloading = Math.Max(reloading, 0);
			if(RoundsLeft < RoundsMax){
				if(++passivereload>=180){
					RoundsLeft++;
					passivereload = 0;
				}else if(passivereload>=228){
					for(int i = 0; i < (passivereload/2)-113; i++){
						//Dust d = 
						Dust.NewDustPerfect(player.getHandPos()+new Vector2(6,0).RotatedBy(MathHelper.ToRadians(i*60)), 263, new Vector2(), 0, new Color(75, 255, 255, 200), 0.5f).noGravity = true;
						//d.noGravity = true;
					}
				}
			}
			if(reloading > 0){
				reloading = reloading = Math.Max(reloading + 1, 0);
				//Dust.NewDustDirect(player.itemLocation, 1, 1, 264, 0, 0, 0, new Color(75, 255, 255, 200), 0.5f).noGravity = true;
				item.holdStyle = 1;
				item.noUseGraphic = true;
				player.itemLocation.X = player.position.X + (float)player.width * 0.5f + 20f * (float)player.direction;
				player.itemLocation.Y = player.MountedCenter.Y;
				///*item.Center*/player.itemLocation = ((player.direction>0?player.Right:player.Left)-new Vector2(16,8))-(new Vector2(24,2).RotatedBy(player.itemRotation)*player.direction);//player.itemLocation-new Vector2(player.direction>0?16:0,0);
				if(reloading>=10)for(int i = 0; i < 60; i++){
					//(player.direction==1?player.Right:player.Left)
					Dust.NewDustDirect((player.itemLocation-new Vector2(player.direction==1?16:-4,2))+new Vector2(reloading>20?(25-reloading)*32:160,0).RotatedBy(MathHelper.ToRadians(i*6)), 1, 1, 264, 0, 0, 0, new Color(75, 255, 255, 200), 0.5f).noGravity = true;
				}
				if(reloading >= 25){
					reloading = 0;
					item.holdStyle = 0;
					item.noUseGraphic = false;
					foreach (Projectile p in Main.projectile)if(p.active && p.CanHit(player) && (player.itemLocation-constrain(player.itemLocation, p.position, p.position+new Vector2(p.width,p.height))).Length() < 160){
						if((p.damage-=30)<=0)p.Kill();
						if(RoundsLeft<RoundsMax)RoundsLeft++;
						for(int i = 0; i < 6; i++){
							Dust.NewDustDirect(p.Center+new Vector2((p.width+p.height)/2, 0).RotatedBy(MathHelper.ToRadians(i*60)), 1, 1, 264, 0, 0, 0, new Color(75, 255, 255, 200), 0.5f).noGravity = true;
						}
					}
				}
			}else if(reloaddelay>0)if(++reloaddelay>=15)reloaddelay = 0;
		}
		
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
		
 
        public override bool CanUseItem(Player player)
        {
            EntropyPlayer modPlayer = player.GetModPlayer<EntropyPlayer>();
			//item.shoot = bullets[0].id;
			if(RoundsLeft <= 0){
				player.itemAnimation = 0;
				return false;
			}
			if(reloading > 0 && RoundsLeft >= RoundsMax){
				reloading = 0;
			}
			item.noUseGraphic = player.altFunctionUse == 2;
            item.autoReuse = false;
			item.useTime = 1;
			item.useAnimation = 15;
            return base.CanUseItem(player);
        }
		readonly string white = Colors.RarityNormal.Hex3();
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
			bool a = false;
			if(dmgratio[3]<=0.15f)a=true;
			if(a)dmgratio[3]+=1f;
			base.ModifyTooltips(tooltips);
			if(a)dmgratio[3]-=1f;
            for (int i = 0; i < tooltips.Count; i++)
            {
                TooltipLine tip;
                if (tooltips[i].text.Contains("DisplayAmmo"))
                {
                    tip = new TooltipLine(mod, "DisplayAmmo",
                        "Rounds Left:" + RoundsLeft);
                    tip.overrideColor = new Color(75, 255, 255, 200);
                    tooltips.RemoveAt(i);
                    tooltips.Insert(i, tip);
                }else if (tooltips[i].Name.Equals("ItemName")){
                    tooltips[i].overrideColor = new Color(75, 255, 255, 200);
				}else if (tooltips[i].Name.Equals("Damage")){
                    tooltips[i].overrideColor = new Color(75, 255, 255, 200);
				}
            }
        }
		
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
			passivereload = 0;
			if(player.altFunctionUse == 2){
				for(int i = 0; i < 14-player.itemAnimation; i++)Dust.NewDustPerfect(position+new Vector2(6,0).RotatedBy(MathHelper.ToRadians(i*60)), 263, new Vector2(), 0, new Color(75, 255, 255, 200), 0.5f).noGravity = true;
				if(player.itemAnimation<=4){
					if(player.controlUseTile){
						if(RoundsLeft<RoundsMax && player.CheckMana((int)(35*player.manaCost), true))RoundsLeft++;
						player.itemAnimation = 0;
						return false;
					}
					if(reloading == 0 && reloaddelay <= 0)reloading = 10;
					reloaddelay = 1;
					player.itemAnimation = 0;
				}
				return false;
			}
			if(!CanUseItem(player))return false;
			item.noUseGraphic = false;
			if(boosted&&(player.itemAnimation==10||player.itemAnimation==11||player.itemAnimation==13||player.itemAnimation==14)){
				Main.PlaySound(2, (int)position.X, (int)position.Y, 38).Volume*=0.5f;
				Main.PlaySound(2, (int)position.X, (int)position.Y, 30);
				SoundEffectInstance a = Main.PlaySound(13, (int)position.X, (int)position.Y);
				a.Volume*=0.15f;
				a.Pitch*=0.5f;
				RoundsLeft--;
				for(int i = 0; i < 3; i++){
					//DustID.PortalBoltTrail DustID.FlameBurst
					Dust.NewDustDirect(position, 0, 0, DustID.PortalBoltTrail, speedX/0.4f, speedY/0.4f, 0, new Color(75, 255, 255, 200), 0.5f).noGravity = true;
				}
			}else if(!boosted&&(player.itemAnimation==13||player.itemAnimation==14)){
				Main.PlaySound(2, (int)position.X, (int)position.Y, 38).Volume*=0.5f;
				Main.PlaySound(2, (int)position.X, (int)position.Y, 30);
				SoundEffectInstance a = Main.PlaySound(13, (int)position.X, (int)position.Y);
				a.Volume*=0.15f;
				a.Pitch*=0.5f;
				RoundsLeft--;
				for(int i = 0; i < 3; i++){
					//DustID.PortalBoltTrail DustID.FlameBurst
					Dust.NewDustDirect(position, 0, 0, DustID.PortalBoltTrail, speedX/0.4f, speedY/0.4f, 0, new Color(75, 255, 255, 200), 0.5f).noGravity = true;
				}
			}else if(!boosted&&player.itemAnimation>1&&player.itemAnimation<8){
				item.noUseGraphic = true;
				if(player.controlUseItem&&player.itemAnimation<6){
					boosting = true;
				}
				if(boosting){
					Vector2 speed = new Vector2(speedX, speedY);
					if(player.controlDown||player.controlUp||player.controlLeft||player.controlRight){
						speed*=0;
						if(player.controlLeft||player.controlRight)speed.X = player.controlRight?1:-1;
						if(player.controlDown||player.controlUp)speed.Y = player.controlDown?1:-1;
					}
					player.velocity+=player.velocity.Y==0&&!player.controlUp?new Vector2(10*(Math.Sign(speed.X)),0):new Vector2(speed.X*2,speed.Y*8);
					if((speed.X!=0||(speed.Y<0||player.velocity.Y!=0))||player.itemAnimation>5)player.immuneTime = player.immuneTime>2?player.immuneTime:2;
					if((speed.X!=0||(speed.Y<0||player.velocity.Y!=0))||player.itemAnimation>5)player.immune = true;
					player.direction = (int)(Math.Sign(speedX))*(player.itemAnimation<7&&player.itemAnimation>3?-1:1);
					if((speed.X!=0||(speed.Y<0||player.velocity.Y!=0))||player.itemAnimation>5)player.stealth = 1;
					if(player.itemAnimation<=2){
						player.velocity*=player.velocity.Y==0?0.05f:0.3f;
						boosted = true;
						player.itemAnimation=14;
					}
				}
				return false;
			}else{
				if(player.itemAnimation<8)item.noUseGraphic = true;
				return false;
			}
			return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
		}
    }
}