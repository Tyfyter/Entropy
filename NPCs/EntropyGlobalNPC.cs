using System;
using System.Collections.Generic;
using Entropy.Buffs;
using Entropy.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Entropy.EntropyExt;

namespace Entropy.NPCs {
    public class EntropyGlobalNPC : GlobalNPC {
		public override bool InstancePerEntity => true;
        
        //{0:"Slash", 1:"Impact", 2:"Puncture", 3:"Cold", 4:"Electric", 5:"Heat", 6:"Toxic", 7:"Blast", 8:"Corrosive", 9:"Gas", 10:"Magnetic", 11:"Radiation", 12:"Viral", 13:"True", 14:"Void"}
        public float[] dmgResist = new float[15]{1f,1f,1f,1f,1f,1f,1f,1f,1f,1f,1f,1f,1f,1f,1f};
        public float[] dmgDodge = new float[15]{0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f};
        public List<BuffBase> Buffs = new List<BuffBase>{};
        public bool rad = false;
        public override void SetDefaults(NPC npc){
            if(!npc.TypeName.ToLower().Contains("dummy"))if(npc.HitSound!=null){
                if(npc.HitSound.SoundId==3){
                    switch(npc.HitSound.Style){
                        case 1:
                        dmgResist[0]*=1.25f;
                        dmgResist[1]*=0.95f;
                        dmgResist[5]*=1.1f;
                        break;
                        case 8:
                        dmgResist[0]*=1.25f;
                        dmgResist[1]*=0.9f;
                        dmgResist[2]*=1.25f;
                        dmgResist[5]*=1.25f;
                        break;
                        case 2:
                        dmgResist[0]*=0.85f;
                        dmgResist[1]*=1.5f;
                        dmgResist[6]*=0.85f;
                        dmgDodge[2]+=0.15f;
                        break;
                        case 4:
                        dmgResist[0]*=0.85f;
                        dmgResist[1]*=1.1f;
                        dmgResist[2]*=1.5f;
                        dmgResist[3]*=1.1f;
                        dmgResist[4]*=1.1f;
                        dmgResist[6]*=0.75f;
                        break;
                        case 24:
                        dmgResist[0]*=0.85f;
                        dmgResist[1]*=1.15f;
                        dmgResist[2]*=1.15f;
                        break;
                        case 34:
                        goto case 4;
                        case 41:
                        dmgResist[0]*=0.65f;
                        dmgResist[1]*=1.3f;
                        dmgResist[2]*=1.5f;
                        dmgResist[6]*=0.5f;
                        dmgDodge[0]+=0.15f;
                        break;
                        case 15:
                        break;
                        case 16:
                        break;
                        case 17:
                        break;
                        default:
                        goto case 1;
                    }
                }
            }
        }
        public override void AI(NPC npc){
            Buffs.RemoveAll(BuffBase.GC);
            rad = false;
			if(Buffs.Count>0){
                for(int i = 0; i<Buffs.Count; i++){
                    Buffs[i].Update(npc, i);
                }
            }
        }
        public override void PostAI(NPC npc){
            if(npc.HasBuff<RadEffect>())if(!(npc.friendly || npc.type == 46 || npc.type == 55 || npc.type == 74 || npc.type == 148 || npc.type == 149 || npc.type == 230 || npc.type == 297 || npc.type == 298 || npc.type == 299 || npc.type == 303 || npc.type == 355 || npc.type == 356 || npc.type == 358 || npc.type == 359 || npc.type == 360 || npc.type == 361 || npc.type == 362 || npc.type == 363 || npc.type == 364 || npc.type == 365 || npc.type == 366 || npc.type == 367 || npc.type == 377 || npc.type == 357 || npc.type == 374 || (npc.type >= 442 && npc.type <= 448 && npc.type != 447)) || npc.type == 538 || npc.type == 539 || npc.type == 337 || npc.type == 540 || (npc.type >= 484 && npc.type <= 487)){
                CheckMeleeCollision(npc);
            }
        }
        public override bool PreAI(NPC npc){
			bool a = true;
            Buffs.RemoveAll(BuffBase.GC);
			if(Buffs.Count>0)for(int i = 0; i<Buffs.Count; i++){
				a = a&&Buffs[i].PreUpdate(npc, !a);
            }
            if(!a)AI(npc);
			return a;
        }
        public override void ModifyHitPlayer(NPC npc, Player target, ref int damage, ref bool crit){
            Buffs.RemoveAll(BuffBase.GC);
			for(int i = 0; i<Buffs.Count; i++){
				Buffs[i].ModifyHit(npc, target, ref damage, ref crit);
            }
        }
        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit){
            List<CorrEffect> corrs = Buffs.FindAll(FindCorr).ConvertAll(MakeCorr);
            if(corrs.Count>0){
                int a = 0;
                for(int i = 0; a < npc.defense+30 && i < corrs.Count; i++)a+=corrs[0].severity/2;
                damage+=a;
                for(int i = 0; i < 5; i++)Dust.NewDustDirect(npc.position, npc.width, npc.height, 267, Alpha:100, newColor:Color.LimeGreen).noGravity = true;
            }
            List<Action> post = new List<Action>();
			for(int i = 0; i<Buffs.Count; i++){
				Buffs[i].ModifyHitItem(player, item, npc, ref damage, ref crit);
                if(Buffs[i] is IPostHitBuff b&&b.HitAction){
                    b.HitAction = false;
                    post.Add(b.postHitAction);
                }
            }
            for(int i = 0; i < post.Count; i++)post[i]();
        }
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection){
            List<CorrEffect> corrs = Buffs.FindAll(FindCorr).ConvertAll(MakeCorr);
            if(corrs.Count>0){
                int a = 0;
                for(int i = 0; a < npc.defense+30 && i < corrs.Count; i++)a+=corrs[0].severity/2;
                damage+=a;
                for(int i = 0; i < 5; i++)Dust.NewDustDirect(npc.position, npc.width, npc.height, 267, Alpha:100, newColor:Color.LimeGreen).noGravity = true;
            }
            List<Action> post = new List<Action>();
			for(int i = 0; i<Buffs.Count; i++){
                Buffs[i].ModifyHitProjectile(projectile, npc, ref damage, ref crit);
                if(Buffs[i] is IPostHitBuff b&&b.HitAction){
                    b.HitAction = false;
                    post.Add(b.postHitAction);
                }
            }
            for(int i = 0; i < post.Count; i++)post[i]();
        }
        public override void ModifyHitNPC(NPC npc, NPC target, ref int damage, ref float knockback, ref bool crit){
            Buffs.RemoveAll(BuffBase.GC);
			for(int i = 0; i<Buffs.Count; i++){
				Buffs[i].ModifyHitNPC(npc, target, ref damage, ref knockback, ref crit);
            }
        }
        public override void DrawEffects(NPC npc, ref Color drawColor){
            if(Buffs.Count<=0)return;
            int r = 0, g = 0, b = 0, a = 0;
            foreach (BuffBase bb in Buffs){
                if(bb.color==null)continue;
                r+=bb.color.Value.R;
                g+=bb.color.Value.G;
                b+=bb.color.Value.B;
                a++;
            }
            if(a>0)drawColor = Color.Lerp(drawColor, new Color(r/a,g/a,b/a), 0.75f);
        }
        public override bool? CanBeHitByItem(NPC npc, Player player, Item item){
            return /* npc.HasBuff<RadEffect>() */rad?new bool?(true):base.CanBeHitByItem(npc, player,item);
        }
        public override bool? CanBeHitByProjectile(NPC npc, Projectile projectile){
            return /* npc.HasBuff<RadEffect>() */rad?new bool?(true):base.CanBeHitByProjectile(npc, projectile);
        }
        public override bool? CanHitNPC(NPC npc, NPC target){
            if(rad)return true;
            if(target.GetGlobalNPC<EntropyGlobalNPC>().rad)return true;
            /* if(npc.HasBuff<RadEffect>()){
                return true;
            }else if(target.HasBuff<RadEffect>()){
                return true;
            } */
            return base.CanHitNPC(npc, target);
        }
        public override bool CanHitPlayer(NPC npc, Player target, ref int cooldownSlot){
            if(!npc.CanAttack())return false;
            return base.CanHitPlayer(npc, target, ref cooldownSlot);
        }
        public override void SetupShop(int type, Chest shop, ref int nextSlot){
            if(type==NPCID.TravellingMerchant&&Main.rand.NextBool(10)&&(NPC.downedMechBoss1||NPC.downedMechBoss2||NPC.downedMechBoss3)){
                shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Claw>());
            }
        }
        public static bool FindRad(BuffBase b){
            return b is RadEffect;
        }
        public static bool FindCorr(BuffBase b){
            return b is CorrEffect;
        }
        public static CorrEffect MakeCorr(BuffBase b){
            return b as CorrEffect;
        }
        public static void AddBuff(BuffBase buff){
            EntropyGlobalNPC npc = buff.npc.GetGlobalNPC<EntropyGlobalNPC>();
            npc.Buffs.Add(buff);
            npc.Buffs.Sort((x,y)=>x.priority!=y.priority?x.priority-y.priority:x.value-y.value);
            if(buff.npc.CountBuff(buff.GetType())>7){
                int i = npc.Buffs.FindIndex((x)=>{return buff.GetType()==x.GetType();});
                npc.Buffs[i].PreUpdate(buff.npc, false);
                npc.Buffs[i].Update(buff.npc);
                npc.Buffs[i].isActive = false;
            }
        }
        private static void CheckMeleeCollision(NPC npc){
            if (npc.dontTakeDamageFromHostiles)
            {
                return;
            }
            int num = 1;
            if (npc.immune[255] == 0)
            {
                int num2 = 30;
                if (npc.type == 548)
                {
                    num2 = 20;
                }
                Rectangle hitbox = npc.Hitbox;
                for (int i = 0; i < 200; i++){
                    if(i==npc.whoAmI)continue;
                    NPC nPC = Main.npc[i];
                    if (nPC.active && !nPC.friendly && nPC.damage > 0)
                    {
                        Rectangle hitbox2 = nPC.Hitbox;
                        float num3 = 1f;
                        NPC.GetMeleeCollisionData(hitbox, i, ref num, ref num3, ref hitbox2);
                        bool? flag = NPCLoader.CanHitNPC(Main.npc[i], npc);
                        if ((!flag.HasValue || flag.Value) && hitbox.Intersects(hitbox2) && ((flag.HasValue && flag.Value) || npc.type != 453 || !NPCID.Sets.Skeletons.Contains(nPC.netID)))
                        {
                            int num4 = nPC.damage;
                            float num5 = 6f;
                            int num6 = 1;
                            if (nPC.position.X + (float)(nPC.width / 2) > npc.position.X + (float)(npc.width / 2))
                            {
                                num6 = -1;
                            }
                            bool crit = false;
                            NPCLoader.ModifyHitNPC(nPC, npc, ref num4, ref num5, ref crit);
                            double num7 = npc.StrikeNPCNoInteraction(num4, num5, num6, crit, false, false);
                            if (Main.netMode != 0)
                            {
                                NetMessage.SendData(28, -1, -1, null, npc.whoAmI, (float)num4, num5, (float)num6, 0, 0, 0);
                            }
                            npc.netUpdate = true;
                            npc.immune[255] = num2;
                            NPCLoader.OnHitNPC(nPC, npc, (int)num7, num5, crit);
                            /* Main.NewText("foomf");
                            nPC.target = npc.WhoAmIToTargettingIndex;
                            nPC.targetRect = npc.Hitbox;
                            nPC.direction = (((float)nPC.targetRect.Center.X < nPC.Center.X) ? -1 : 1);
                            nPC.directionY = (((float)nPC.targetRect.Center.Y < nPC.Center.Y) ? -1 : 1);
                            npc.target = nPC.WhoAmIToTargettingIndex;
                            npc.targetRect = nPC.Hitbox;
                            npc.direction = (((float)npc.targetRect.Center.X < npc.Center.X) ? -1 : 1);
                            npc.directionY = (((float)npc.targetRect.Center.Y < npc.Center.Y) ? -1 : 1); */
                            if (npc.dryadWard)
                            {
                                num4 = (int)num7 / 3;
                                num5 = 6f;
                                num6 *= -1;
                                nPC.StrikeNPCNoInteraction(num4, num5, num6, false, false, false);
                                if (Main.netMode != 0)
                                {
                                    NetMessage.SendData(28, -1, -1, null, i, (float)num4, num5, (float)num6, 0, 0, 0);
                                }
                                nPC.netUpdate = true;
                                nPC.immune[255] = num2;
                            }
                        }
                    }
                }
            }
        }
        public static void TargetClosestNPC(NPC npc, bool faceTarget = true, Vector2? checkPosition = null){
            int num3 = -1;
            Vector2 center = npc.Center;
            if (checkPosition.HasValue)
            {
                center = checkPosition.Value;
            }
            bool flag = npc.direction == 0;
            float num4 = 9999999f;
            for (int j = 0; j < 200; j++){
                NPC nPC = Main.npc[j];
                if (nPC.active && nPC.type == 548){
                    float num6 = Vector2.Distance(center, nPC.Center);
                    if (num4 > num6){
                        num3 = j;
                        num4 = num6;
                    }
                }
            }
            if (num4 == 9999999f){
                return;
            }
            if (num3 >= 0){
                npc.target = Main.npc[num3].WhoAmIToTargettingIndex;
                npc.targetRect = Main.npc[num3].Hitbox;
                npc.direction = (((float)npc.targetRect.Center.X < npc.Center.X) ? -1 : 1);
                npc.directionY = (((float)npc.targetRect.Center.Y < npc.Center.Y) ? -1 : 1);
                return;
            }
        }
    }
}