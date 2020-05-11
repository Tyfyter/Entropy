using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using Entropy.Projectiles;
using Entropy.Buffs;
using static Entropy.NPCs.EntropyGlobalNPC;
using Microsoft.Xna.Framework.Audio;

namespace Entropy.Items{
	public class Sekkal : CompModItem{
		public override string Texture => "Entropy/Items/Sekkal";
		int time = 0;
		int charge = 0;
		int combo = 0;
		public int element = 0;
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Sekkal");
			Tooltip.SetDefault("Untold Destruction\n-Aurdeorum");
		}
		public override void SetDefaults() {
			item.damage = 160;//realdmg = dmgbase = 160;
			item.knockBack = 8;
			//item.ranged = mode == 0;
			//item.melee = !item.ranged;
			item.width = 64;
			item.height = 64;
			//item.useTime = mode==2?15:mode==0?17:20;
			//item.useAnimation = mode==2?15:mode==0?17:20;
			//item.useStyle = mode == 0?5:1;
			//item.knockBack = mode==1?12:6;
			item.value = 10000;
			item.rare = 2;
			//item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.useTurn = false;
			item.shootSpeed = 13.5f;
			item.useTime = 1;
			item.useAnimation = 12;
			//item.useAmmo = AmmoID.Bullet;
			item.useStyle = 5;
			item.melee = true;
			item.ranged = false;
			item.noMelee = false;
			item.noUseGraphic = false;
			item.useStyle = 1;
			item.shoot = ModContent.ProjectileType<SekkalWave>();
			item.useAmmo = 0;
			realcrit = basecrit = 25;
			statchance = basestat = 25;
			dmgratio = dmgratiobase = new float[15] {0.75f,0.2f,0.05f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f};
			//dmgratio = dmgratiobase = new float[15] {0.80f,0.1f,0.1f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f};
		}
		public override void HoldStyle(Player player){
			charge = 0;
			combo = 0;
		}
		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat){
			base.ModifyWeaponDamage(player, ref add, ref mult, ref flat);
			if(charge>0)flat+=charge;
		}
		/* public override Vector2? HoldoutOffset(){
			return new Vector2(4,4);
		} */
		public override void HoldItem(Player player){
			base.HoldItem(player);
			if(time>0)time--;
		}
		public override void tryScroll(int dir){
			ability=(ability+dir+4)%4;
		}
		public override bool AltFunctionUse(Player player){
			return true;
		}
		public override void MeleeEffects(Player player, Rectangle hitbox){
            for (int i = 0; i < (charge<=-30?3:2); i++){
                Dust d = Dust.NewDustDirect(hitbox.Location.ToVector2(), hitbox.Width, hitbox.Height, 267, newColor:i==1?Color.DarkRed:Color.Goldenrod, Scale:0.6f);
                d.fadeIn = 0.7f;
                d.noGravity = true;
            }
        }
		public override bool? CanHitNPC(Player player, NPC target){
			bool? bass = base.CanHitNPC(player, target);
			return bass??(charge>=0||combo==2);
		}
		public override bool CanUseItem(Player player){
			if(!base.CanUseItem(player))return false;
			if(player.altFunctionUse==2){
				if(time==0)CastAbility(ability);
				if(ability!=1)time = 3;
				if(ability==1&&time<=0){
					time = element==2?3:16;
				}
				return false;
			}
			return true;
		}
		public void CastAbility(int i){
			Player player = Main.player[item.owner];
			int el = 13;
			if(element==0){
				el = 5;
			}else if(element==1){
				el = 3;
			}else if(element==2){
				el = 4;
			}else if(element==3){
				el = 6;
			}
			switch(i){
				case 3:
				if(i==3&&!player.CheckMana(75, true))return;
				EntModProjectile emp2 = Projectile.NewProjectileDirect(player.Center, new Vector2(), ModContent.ProjectileType<SekkalAbility2>(), (int)(player.GetWeaponDamage(item)*1.5f), 0, player.whoAmI, el).modProjectile as EntModProjectile;
				if(emp2!=null){
					emp2.dmgratio[el] = 1;
				}
				SoundEffectInstance eff = Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y, 20);
				eff.Volume = 1f;
				eff.Pitch =-1f;
				break;
				case 2:
				ElementBuff eb = player.GetBuff<ElementBuff>();
				if(eb!=null){
					eb.isActive = false;
					el = eb.type;
					if(player.CheckMana(15, true))goto case 3;
					return;
				}
				if(!player.CheckMana(50, true))return;
				EntropyPlayer.AddBuff(new ElementBuff(player, 1800, el));
				break;
				case 1:
				if(!player.CheckMana(element==2?2:25, true))return;
				//int el = 13;
				int dmg = player.GetWeaponDamage(item);
				float speed = 9.5f;
				float spread = 0.06f;
				if(element==0){
                    //el = 5;
					dmg = (int)(dmg*1.25f);
                }else if(element==1){
                    //el = 3;
					dmg = (int)(dmg*1.75f);
                }else if(element==2){
                    //el = 4;
					dmg = (int)(dmg*2f);
					spread=0.04f;
                }else if(element==3){
                    //el = 6;
					dmg = (int)(dmg*0.95f);
                }
				dmg/=4;
				Vector2 vec = (Main.MouseWorld-(player.Top+new Vector2(player.direction*3, 14)));
				if(vec.X!=0)player.direction = vec.X>1?1:-1;
				for(float i2 = -2; i2 < 3; i2++){
					if(element==2)i2 = (Main.rand.NextFloat(4)-2);
					EntModProjectile emp = (Projectile.NewProjectileDirect(player.Top+new Vector2(player.direction*3, 14), vec.SafeNormalize(new Vector2()).RotatedBy(i2*spread)*speed, ModContent.ProjectileType<SekkalAbility>(), dmg, 1, player.whoAmI).modProjectile as EntModProjectile);
					if(emp!=null){
						emp.dmgratio[el] = 1;
					}
					if(element==2)break;
				}
				break;
				default:
				element=(element+1)%4;
				break;
			}
		}
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
			//Main.NewText(Main.time);
			if(charge<=0&&combo!=2){
				if(player.controlUseItem){
					if(charge>-30)charge--;
					player.itemAnimation = player.itemAnimationMax-1;	
				}else{
					charge = Math.Abs(charge);
					Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y, 1).Volume = 0.3f;
					Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y, 71, pitchOffset:-0.75f);
				}
			}else{
				if(combo==2){
					if(charge>0&&player.itemAnimation>=player.itemAnimationMax-1){
						charge = 0;
						player.itemAnimation = 1;
					}
					player.itemAnimation+=3;
					if(player.itemAnimation>=player.itemAnimationMax){
						combo = 0;
						player.itemAnimation = 0;
						item.reuseDelay = 10;
					}
					return false;
				}
				if(player.itemAnimation<2){
					if(charge>=30){
						Vector2 vec = new Vector2(speedX, speedY);
						speedY = 0;
						speedX = vec.Length()*Math.Sign(speedX);
						base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
					}
					if(player.controlUseItem){
						combo = (combo+1)%3;
					}
				}
			}
			return false;
		}
		public override void GetWeaponKnockback(Player player, ref float knockback){
			knockback*=combo==2?3:0.4f;
		}
		public override float MeleeSpeedMultiplier(Player player){
			return base.MeleeSpeedMultiplier(player)*(combo>0?0.7f:charge>5?1:0.85f);
		}
        public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit){
			if(combo == 2){
				AddBuff(new BlastEffect(target, 30));
			}
            if(target.HasBuff<BlastEffect>()||target.HasBuff<ImpactEffect>())damage*=2;
			base.ModifyHitNPC(player, target, ref damage, ref knockBack, ref crit);
        }
	}
	/* public class Valhalla2 : Valhalla{
		int[] modsobsolete = new int[8] {6,3,0,0,0,0,0,0};
		int[] modlevelsobsolete = new int[8] {0,0,0,0,0,0,0,0};
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Valhalla");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults() {
			item.type = ModContent.ItemType("Trinity1");
			item.SetDefaults(item.type);
			item.damage = 50;
			item.melee = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = 1;
			item.knockBack = 6;
			item.value = 10000;
			item.rare = 2;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			dmgratio = dmgratiobase = new float[15] {0.06f,0.88f,0.06f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f};
			critDMG = 2;
			statchance = basestat = 25;
		}

		public override void HoldItem(Player player){
			SetDefaults();
			for(int i = 0; i < modsobsolete.Length; i++){
				Entropy.ModEffectobsolete(modsobsolete[i], modlevelsobsolete[i]);
			}
		}
		public override bool CanRightClick(){
			item.type = ModContent.ItemType<Trinity3>();
			return false;
		}
	}
	public class Valhalla3 : Valhalla{
		int[] modsobsolete = new int[8] {6,3,0,0,0,0,0,0};
		int[] modlevelsobsolete = new int[8] {0,0,0,0,0,0,0,0};
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Valhalla");
			Tooltip.SetDefault("");
			Item.staff[item.type] = true;
		}
		public override void SetDefaults() {
			item.type = ModContent.ItemType("Trinity1");
			item.SetDefaults(item.type);
			item.damage = 50;
			item.melee = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = 1;
			item.knockBack = 6;
			item.value = 10000;
			item.rare = 2;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			dmgratio = dmgratiobase = new float[15] {0.06f,0.06f,0.88f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f};
			critDMG = 2;
			statchance = basestat = 25;
		}

		public override bool CanRightClick(){
			item.type = ModContent.ItemType<Trinity1>();
			return false;
		}
	} */
}
