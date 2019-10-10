using System;
using System.Collections.Generic;
using Entropy;
using Entropy.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RefTheGun.Projectiles
{

    public class FallenWillbreaker : ModProjectile
    {
        public override String Texture => "Entropy/Items/Willbreaker";
        public override bool CloneNewInstances => true;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Willbreaker");
		}
        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.Shuriken);
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.aiStyle = 0;
            projectile.width = 17;
            projectile.height = 17;
            projectile.extraUpdates+=4;
            projectile.hide = true;
        }
        public override void AI(){
            if(projectile.velocity.Length()>0)projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X)+(float)(Math.PI/4);
            if(Main.dayTime)projectile.Kill(); else projectile.timeLeft++;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor){
            Lighting.AddLight(projectile.Center, Color.DodgerBlue.R/500f, Color.DodgerBlue.G/500f, Color.DodgerBlue.B/500f);
            spriteBatch.Draw(mod.GetTexture("Items/Willbreaker"), projectile.Center - Main.screenPosition, new Rectangle(0, 0, 40, 40), lightColor, projectile.rotation, new Vector2(10,30), 1, SpriteEffects.None, 0f);
            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity){
            projectile.position+=oldVelocity;
            projectile.velocity = new Vector2();
            //projectile.damage = 0;
            return false;
        }
        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI){
            drawCacheProjsBehindNPCsAndTiles.Add(index);
        }
        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit){
            damage = -1;
            Item.NewItem(projectile.Center, new Vector2(), mod.ItemType<Willbreaker>(), noGrabDelay:true);
            mod.GetModWorld<EntropyWorld>().gotSword = true;
            projectile.Kill();
        }
        /* public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough){
            width/=40;
            height/=40;
            return true;
        } */
    }
}