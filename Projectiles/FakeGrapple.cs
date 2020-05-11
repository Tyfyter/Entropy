using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Entropy.Projectiles{

    public class FakeGrapple : ModProjectile
    {
        public override void SetDefaults()
        {
            //projectile.name = "Wind Shot";  //projectile name
            projectile.width = 0;       //projectile width
            projectile.height = 0;  //projectile height
            projectile.penetrate = 0;      //how many npcs will penetrate
            projectile.timeLeft = 0;   //how many time this projectile has before it expipires
            projectile.extraUpdates = 1;
            projectile.aiStyle = 7;
            projectile.Kill();
        }
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Fake Grappling Hook");
		}
        public override void AI()           //this make that the projectile will face the corect way
        {                                                           // |
           projectile.Kill(); 
        }
    }
}