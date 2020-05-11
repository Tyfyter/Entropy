using System;
using System.Collections.Generic;
using Entropy.Buffs;
using Entropy.Items;
using Entropy.NPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Entropy.NPCs.EntropyGlobalNPC;

namespace Entropy.Projectiles{

    public class VizenSmite : EntModProjectile {
        public override string Texture => "Terraria/Projectile_55";
        int lasthit1 = -1;
        int lasthit2 = -1;
        int lasthit3 = -1;
        int lasthit4 = -1;
        public override void SetDefaults(){
            projectile.CloneDefaults(ProjectileID.Bullet);
            projectile.Size = new Vector2(48, 48);
            projectile.aiStyle = 0;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.ranged = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 5;
            projectile.extraUpdates = 4;
            projectile.ignoreWater = true;
			dmgratio = dmgratiobase = new float[15]{0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f};
        }
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Smite");
		}/* 
        public override bool OnTileCollide(Vector2 oldVelocity){
            return true;
        } */
        public override bool PreKill(int timeLeft){
            return false;
        }
        public override void AI(){
            //projectile.velocity.Y-=0.025f;
            Dust d = Dust.NewDustPerfect(projectile.Center, 267, null, 0, new Color(0, 255, 128), 0.6f);
            d.velocity = new Vector2(projectile.velocity.X*0.9f, projectile.velocity.Y*0.9f);
            d.fadeIn = 0.7f;
            d.noGravity = true;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit){
            if(projectile.ai[0]==1){
                target.velocity*= 0;
                lasthit4 = lasthit3;
                lasthit3 = lasthit2;
                lasthit2 = lasthit1;
                lasthit1 = target.whoAmI;
                int dist = 160,targ = -1; 
                for(int i = 0; i < Main.npc.Length; i++){
                    if(!(i==lasthit1||i==lasthit2||i==lasthit3||i==lasthit4)&&Main.npc[i].active&&!Main.npc[i].immortal){
                        float d2 = Main.npc[i].Distance(target.Center);
                        if(d2<dist&&d2<16){
                            targ = i;
                        }
                    }
                }
                if(targ!=-1){
                    projectile.Center = target.Center;
                    projectile.velocity = (Main.npc[targ].Center-target.Center).SafeNormalize(Vector2.Zero)*12.5f;
                    Dust d = Dust.NewDustPerfect(projectile.Center, 267, projectile.velocity, 0, new Color(0, 255, 128), 0.6f);
                    d.velocity = new Vector2(projectile.velocity.X*0.9f, projectile.velocity.Y*0.9f);
                    d.fadeIn = 0.7f;
                    d.noGravity = true;
                    if(!target.GetGlobalNPC<EntropyGlobalNPC>().Buffs.Exists((b)=>b is SmiteEffect))projectile.timeLeft = 60;
                    projectile.Size = new Vector2(12, 12);
                }
                AddBuff(new SmiteEffect(target, 15));
                return;
            }
            target.velocity = projectile.velocity;
            AddBuff(new RadEffect(target, 600));
            AddBuff(new YoteEffect(target, 180));
        }
        public override bool? CanHitNPC(NPC target){
            int i = target.whoAmI;
            if((i==lasthit1||i==lasthit2||i==lasthit3||i==lasthit4))return false;
            return null;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor){
            return false;
        }
    }
    public class SmiteEffect : BuffBase{
        int baseduration = 1;
        float kbr = 1;
        bool kb = false;
        public override Color? color {get{
            return new Color(125, 0, 155);
        }}
        public SmiteEffect(NPC npc, int duration) : base(npc){
            baseduration = this.duration = duration;
            if(npc.knockBackResist>=0.9f)return;
            kb = true;
            kbr = npc.knockBackResist;
            npc.knockBackResist = 0.9f;
        }
        public override void Update(NPC npc){
            if(npc.boss||npc.collideX||npc.collideY||npc.noTileCollide||npc.wet||npc.noGravity){
                base.Update(npc);
            }else if(duration < baseduration){
                duration++;
            }
            if(!isActive&&kb){
                npc.knockBackResist = kbr;
            }
        }
        public override bool PreUpdate(NPC npc, bool canceled){
            return false;
        }
    }
}