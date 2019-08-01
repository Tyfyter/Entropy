using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Text;
using System.Reflection;

namespace Entropy.Projectiles
{
	public class EntModProjectile : ModProjectile
	{
		public float critDMG = 2;
		public float statchance = 15;
        public float[] dmgratiobase = new float[15] {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1};
		public float[] dmgratio = new float[15] {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1};
        public Item[] mods = new Item[8];
		public int critcombo = 0;
        public int wallPenProgress = 0;
        public int wallPenMax => 8;
        public virtual bool reproc => false;
        public virtual bool IsMod => false;
        public override bool CloneNewInstances => true;
        public override bool Autoload(ref string name){
            if(name == "EntModProjectile")return false;
            return true;
        }
        /*public static EntModItem New(int type = 0, int id = 0, int level = 0){
            EntModItem output;
            switch(type){
                default:
                output = new EntModItemMod(id, level);
                break;
            }
            return output;
        }
        public void ModEffect(EntModItemMod Mod){

        }*/
        public override bool OnTileCollide(Vector2 oldVelocity){
            if(projectile.penetrate>0){
                if(++wallPenProgress>=wallPenMax){
                    projectile.penetrate--;
                    wallPenProgress = 0;
                }
                projectile.velocity = oldVelocity;
            }
            return projectile.penetrate<0;
        }
        public void ModEffectobsolete(int modid, float level){
            Player player = Main.player[projectile.owner];
            EntropyPlayer modPlayer = player.GetModPlayer<EntropyPlayer>(mod);
            switch (modid){
                default:
                break;
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockBack, ref bool crit, ref int hitDirection){
            Player player = Main.player[projectile.owner];
			EntropyPlayer modPlayer = player.GetModPlayer<EntropyPlayer>(mod);
			//modPlayer.comboadd();
			float[] dmgarray = Entropy.GetDmgRatio(damage, dmgratio);
			damage = (int)Entropy.DmgCalcNPC(dmgarray, target);
            int usedcrit = 0;
            if(projectile.melee){
                usedcrit = player.meleeCrit;
            }else if(projectile.ranged){
                usedcrit = player.rangedCrit;
            }else if(projectile.magic){
                usedcrit = player.magicCrit;
            }else if(projectile.thrown){
                usedcrit = player.thrownCrit;
            }
			if(crit){
				damage /= 2;
			}
			int cc = usedcrit;
			for(int i = cc; i > 0; i-=100){
				if(i < 100){
					if(i < Main.rand.Next(0,100))break;
				}
				damage = (int)(damage * critDMG);
			}
            if(crit && critcombo!=0)modPlayer.comboadd(critcombo);
			Entropy.Proc(this, target, damage);
        }
    }
}