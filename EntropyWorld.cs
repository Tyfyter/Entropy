using System;
using Microsoft.Xna.Framework;
using RefTheGun.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Entropy {
    public class EntropyWorld : ModWorld {
        internal bool gotSword = false;

        public override void PostUpdate(){
            if(!Main.dayTime && Main.hardMode && !Main.fastForwardTime && (!gotSword||Main.rand.Next(24)==0) && (int)Main.time == 1){
                int num145 = 12;
                int num146 = Main.rand.Next(Main.maxTilesX - 50) + 100;
                num146 *= 16;
                int num147 = Main.rand.Next((int)((double)Main.maxTilesY * 0.05));
                num147 *= 16;
                Vector2 vector = new Vector2((float)num146, (float)num147);
                float num148 = (float)Main.rand.Next(-100, 101);
                float num149 = (float)(Main.rand.Next(200) + 100);
                float num150 = (float)Math.Sqrt((double)(num148 * num148 + num149 * num149));
                num150 = (float)num145 / num150;
                num148 *= num150;
                num149 *= num150;
                Projectile.NewProjectile(vector.X, vector.Y, num148, num149, mod.ProjectileType<FallenWillbreaker>(), 1000, 10f, Main.myPlayer, 0f, 0f);
                //Main.NewText((vector.X-(Main.rightWorld/2))/8);
            }
        }
    }
}