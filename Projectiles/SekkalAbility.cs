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

namespace Entropy.Projectiles
{

    public class SekkalAbility : EntModProjectile{
		public override string Texture => "Entropy/Items/Mods/ComboSpeed";
        //3:Cold:2, 4:Electric:3, 5:Heat:1, 6:Toxic:4
        int el = -1;
        Color? elCol;
        Color elementColor {
            get {
                if(!elCol.HasValue){
                    int i = element;
                }
                return elCol.Value;
            }
        }
        int element {
            get{
                if(el!=-1)return el;
                if(dmgratio[3]>0){
                    elCol = new Color(100,235,255);
                    return el = 3;
                }
                if(dmgratio[4]>0){
                    elCol = Color.DarkViolet;
                    return el = 4;
                }
                if(dmgratio[5]>0){
                    elCol = Color.OrangeRed;
                    return el = 5;
                }
                if(dmgratio[6]>0){
                    elCol = Color.DarkGreen;
                    return el = 6;
                }
                return 0;
            }
        }
        public override void SetDefaults(){
            projectile.width = projectile.height = 4;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.timeLeft = 60;
            projectile.extraUpdates = 3;
            projectile.penetrate = -1;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
			dmgratio = dmgratiobase = new float[15]{0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f};
        }
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Sekkal");
		}
        public override void AI(){
            if(projectile.timeLeft==0)return;
            for (int y = -10; y < 10; y++){
                //Vector2 velocity = projectile.velocity.SafeNormalize(new Vector2(160/projectile.timeLeft,0)).RotatedBy(MathHelper.ToRadians(y*6));
                Dust d = Dust.NewDustPerfect(projectile.Center, 267, new Vector2(), 0, elementColor, 0.6f);
                //d.position -= d.velocity * 8;
                d.fadeIn = 0.7f;
                d.noGravity = true;
            }
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockBack, ref bool crit, ref int hitDirection){
            AddBuff(BuffBase.GetFromIndex(target, element, damage, Main.player[projectile.owner]));
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor){
            return false;
        }
    }
}