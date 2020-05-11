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

namespace Entropy.Items{
	public class CompModItem : EntModItem{
		internal int ability = 0;
		public virtual int maxabilities => 4;
		public override bool isGun => true;
		public virtual void tryScroll(int dir){}
        public override bool Autoload(ref string name){
            if(name == "CompModItem")return false;
            return true;
        }
	}
	public class Valhalla : CompModItem{
		public override string Texture => "Entropy/Items/Valhalla";
		int[] modsobsolete = new int[8] {6,3,0,0,0,0,0,0};
		int[] modlevelsobsolete = new int[8] {0,0,0,0,0,0,0,0};
		internal int mode = 0;
		int lastmode = 0;
		int time = 0;
		static bool initialized = false;
		static int? clawType = null;
		static Func<Player, int, int, Rectangle> UseItemGraphicbox;
		static Func<Player, Vector2> GetFistVelocity;
		public static UIH MPFUseItemHitbox;
		//static Func<ModPlayer, float, float, float, float, bool, int, bool> SetDashOnMovement;
		static MethodInfo DashOnMovement;

		public delegate bool UIH(Player player, ref Rectangle hitbox, int distance, float jumpSpeed = 9, float fallSpeedXmod = 0.5F, float fallSpeedY = 8, bool disableBounce = false);
		delegate bool SDOM(float dashSpeed = 14.5F, float dashMaxSpeedThreshold = 12, float dashMaxFriction = 0.992F, float dashMinFriction = 0.96F, bool forceDash = false, int dashEffect = 0);
		public override bool realCombo => true;
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Valhalla");
			Tooltip.SetDefault("Trapped and tortured"/*\n-Aurdeorum*/);
		}
		public override void SetDefaults() {
			switch (mode){
				case 0:
				realdmg = dmgbase = 40;
				item.useStyle = 5;
				statchance = basestat = 22;
				item.ranged = true;
				item.melee = false;
				item.noMelee = true;
				item.noUseGraphic = false;
				item.shoot = ModContent.ProjectileType<ValhallaArrow>();
				item.useTime = 20;
				item.useAnimation = 20;
				break;
				case 1:
				realdmg = dmgbase = 60;
				statchance = basestat = 18;
				item.useTime = 15;
				item.useAnimation = 15;
				goto default;
				case 2:
				realdmg = dmgbase = 90;
				statchance = basestat = 28;
				item.useTime = 12;
				item.useAnimation = 12;
				goto default;
				default:
				item.melee = true;
				item.ranged = false;
				item.noMelee = false;
				item.noUseGraphic = true;
				item.useStyle = clawType.Value;
				item.shoot = 0;
				break;
			}
			item.damage = 1;//realdmg = dmgbase = mode==0?60:50;
			//item.ranged = mode == 0;
			//item.melee = !item.ranged;
			item.width = 40;
			item.height = 40;
			//item.useTime = mode==2?15:mode==0?17:20;
			//item.useAnimation = mode==2?15:mode==0?17:20;
			//item.useStyle = mode == 0?5:1;
			//item.knockBack = mode==1?12:6;
			item.value = 10000;
			item.rare = 2;
			//item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.useTurn = mode != 0;
			item.shootSpeed = 13.5f;
			item.useAmmo = AmmoID.Arrow;
			dmgratio = dmgratiobase = new float[15] {0.80f,0.1f,0.1f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f};
		}
		public override Vector2? HoldoutOffset(){
			return new Vector2(4,4);
		}
		public override void HoldItem(Player player){
			base.HoldItem(player);
			if(time>0)time--;
			if(mode == 2){
				player.manaRegenBuff = false;
				if((Main.time%2==0)&&!player.CheckMana(1, true))mode = lastmode;
			}
		}
		public override void tryScroll(int dir){
			if(initialized&&MPFUseItemHitbox==null){
				ability=dir<0?1:2;
				return;
			}
			ability=(ability+dir+4)%4;
		}
		public override bool AltFunctionUse(Player player){
			return true;
		}
		public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox){
			MPFUseItemHitbox(player, ref hitbox, 22, 11.7f, 3f*0.25f, 12f);
		}
		public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Rectangle r = UseItemGraphicbox(player, 1, 8);
            Vector2 velocity = GetFistVelocity(player);
            Vector2 perpendicular = velocity.RotatedBy(Math.PI / 2);//clever, I would have just used MathHelper.ToRadians(90)
            Vector2 pVelo = (player.position - player.oldPosition);
            // Claw like effect
            for (int y = -1; y < 2; y++)
            {
                Dust d = Dust.NewDustPerfect(r.TopLeft() + perpendicular * y * 5, 267, null, 0, mode==2?Color.OrangeRed:Color.GhostWhite, 0.6f);
                d.velocity = new Vector2(velocity.X * -2, velocity.Y * -2);
                d.position -= d.velocity * 8;
                d.velocity += pVelo;
                d.fadeIn = 0.7f;
                d.noGravity = true;
            }
        }
		public override bool CanUseItem(Player player){
			if(!base.CanUseItem(player))return false;
			if(player.altFunctionUse==2){
				if(time==0)CastAbility(ability);
				time = 3;
				return false;
			}
            if(mode!=0){
				Main.PlaySound(2, player.Center, 1);
				Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y, 71, pitchOffset:0.55f).Volume=0.3f;
				DashOnMovement.Invoke(player.GetModPlayer(ModLoader.GetMod("WeaponOut"), "ModPlayerFists"), new object[]{10, 24f, 0.992f, 0.96f, true, 0});
			}else{
				Main.PlaySound(2, player.Center, 5);
				Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y, 71, pitchOffset:0.55f).Volume=0.3f;
			}
			return true;
		}
		public static void InitClaws(){
			if(initialized)return;
			initialized = true;
			Type MPF = Main.LocalPlayer.GetModPlayer(ModLoader.GetMod("WeaponOut"), "ModPlayerFists")?.GetType();
			if(MPF==null)return;
			clawType = MPF.GetField("useStyle").GetValue(null) as int?;
            //MPF.GetMethod("RegisterComboEffectID").Invoke(null, new Action<Player, Item, bool>[]{ComboEffects});//ComboEffects
			MPFUseItemHitbox = Delegate.CreateDelegate(typeof(UIH),MPF.GetMethod("UseItemHitbox")) as UIH;//UseItemHitbox
			UseItemGraphicbox = Delegate.CreateDelegate(typeof(Func<Player, int, int, Rectangle>),MPF.GetMethod("UseItemGraphicbox")) as Func<Player, int, int, Rectangle>;//UseItemGraphicbox
			GetFistVelocity = Delegate.CreateDelegate(typeof(Func<Player,Vector2>),MPF.GetMethod("GetFistVelocity")) as Func<Player,Vector2>;//GetFistVelocity
			DashOnMovement = MPF.GetMethod("SetDashOnMovement");
			//SetDashOnMovement = Delegate.CreateDelegate(typeof(Func<ModPlayer, float, float, float, float, bool, int, bool> ),MPF.GetMethod("SetDashOnMovement")) as Func<ModPlayer, float, float, float, float, bool, int, bool>;//SetDashOnMovement
			//ModifyTooltips
		}
		//public static void ComboEffects(Player player, Item item, bool initial){}
		public void CastAbility(int i){
			if(!initialized)InitClaws();
			Player player = Main.player[item.owner];
			switch(i){
				case 3:
				if(mode!=2)lastmode = mode;
				mode = mode==2?lastmode:2;
				break;
				case 2:
				if(player.statMana<3)return;
				int dmg = player.statMana;
				Projectile.NewProjectile(player.Center, new Vector2(), ModContent.ProjectileType<ValhallaAbility>(), dmg/2+1, 10, player.whoAmI);
				player.CheckMana(dmg/3, true);
				Main.PlaySound(15, (int)player.Center.X, (int)player.Center.Y, 0, pitchOffset:0.55f);
				break;
				case 1:
				if(!player.CheckMana(150, true))return;
				Projectile.NewProjectile(player.Center, new Vector2(), ModContent.ProjectileType<ValhallaAbility>(), 1, 0, player.whoAmI);
				Main.PlaySound(29, (int)player.Center.X, (int)player.Center.Y, 8, pitchOffset:-0.25f);
				break;
				default:
				mode = mode==1?0:1;
				break;
			}
		}
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
			type = ModContent.ProjectileType<ValhallaArrow>();
			Vector2 vec = new Vector2(speedX, speedY).SafeNormalize(new Vector2()).RotatedBy(Math.PI/2)*8;
			position+=vec;
			base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
			position-=vec;
			base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
			position-=vec;
			return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
		}
        public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit){
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
