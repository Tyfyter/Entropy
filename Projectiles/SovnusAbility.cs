using System;
using System.Collections.Generic;
using Entropy.Buffs;
using Entropy.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Entropy.NPCs.EntropyGlobalNPC;
using static Entropy.EntropyExt;

namespace Entropy.Projectiles
{

    public class SovnusAbility : EntModProjectile {
        static readonly Color[] colors = new Color[]{Color.DodgerBlue,Color.LightGoldenrodYellow};
        public override string Texture => "Terraria/Projectile_684";
        Ray HB = new Ray();
        public override void SetDefaults(){
            projectile.CloneDefaults(ProjectileID.DD2SquireSonicBoom);
            projectile.width = 48;
            projectile.height = 80;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.melee = false;
            projectile.ranged = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 60;
            projectile.extraUpdates = 4;
            projectile.ignoreWater = true;
            projectile.aiStyle = 0;
            projectile.light = 0;
			dmgratio = dmgratiobase = new float[15]{0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 1f, 0f};
        }
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Sovnus");
		}
        public override void AI(){
            bool dir = true;
            if(projectile.ai[0]==1)dir=false;
            //projectile.velocity.Y-=0.025f;
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f; 
            Vector2 pos2d2 = (new Vector2(0, constrain(((76-projectile.timeLeft)*5+(-projectile.timeLeft))/5, 16, 160)).RotatedBy(projectile.rotation+(Math.PI/2)));
            HB.Position = new Vector3((pos2d2/2)+projectile.Center,0);
            HB.Direction = new Vector3(-pos2d2,0);
            for(int i = 0; i < 10; i++){
                Dust d = Dust.NewDustPerfect((HB.Position+(HB.Direction*i/15f)+(HB.Direction/10f)).toVector2(), 267, newColor:dir?colors[i%2]:Color.Red, Scale:0.6f);
                d.velocity = new Vector2(projectile.velocity.X*2, projectile.velocity.Y*2).RotatedBy(MathHelper.ToRadians((5-i)*5));
                d.position -= !dir?d.velocity * 8 : projectile.velocity * 8;
                //if(dir)d.velocity = d.velocity.RotatedBy(MathHelper.ToRadians((5-i)*-10));
                d.fadeIn = 0.7f;
                d.noGravity = true;
            }
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockBack, ref bool crit, ref int hitDirection){
            if(projectile.ai[0]==1){
                bool flag = target.HasBuff<SleepEffect>();
                damage = flag?damage*2:damage/2;
                if(flag)AddBuff(new VoidEffect(target, 300));
                return;
            }
            damage/=2;
            AddBuff(new SleepEffect(target, 300, 1));
        }
        public override bool? CanHitNPC(NPC target){
            if(!projectile.Hitbox.Intersects(target.Hitbox))return false;
            Player player = Main.player[projectile.owner];
            BoundingBox THB = new BoundingBox();
            THB.Min = new Vector3(target.Hitbox.X, target.Hitbox.Y, -1);
            THB.Max = new Vector3(target.Hitbox.X+target.Hitbox.Width, target.Hitbox.Y+target.Hitbox.Height, 1);
            return (HB.Intersects(THB)!=null)&&(!target.friendly);
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox){
            //hitbox.X-=Main.player[projectile.owner].width;
            hitbox = HB.ProperBox();
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor){
            return false;
        }
    }
}