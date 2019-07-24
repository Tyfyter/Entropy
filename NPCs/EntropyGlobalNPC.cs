using System;
using System.Collections.Generic;
using Entropy.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Entropy.EntropyExt;

namespace Entropy.NPCs {
    public class EntropyGlobalNPC : GlobalNPC {
		public override bool InstancePerEntity => true;
        public float[] dmgResist = new float[15]{1f,1f,1f,1f,1f,1f,1f,1f,1f,1f,1f,1f,1f,1f,1f};
        public float[] dmgDodge = new float[15]{0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f};
        public List<BuffBase> Buffs = new List<BuffBase>{};
        public override void SetDefaults(NPC npc){
            if(npc.HitSound!=null){
                if(npc.HitSound.SoundId==3){
                    switch(npc.HitSound.Style){
                        case 1:
                        dmgResist[0]*=1.25f;
                        dmgResist[1]*=0.95f;
                        break;
                        case 8:
                        dmgResist[0]*=1.25f;
                        dmgResist[1]*=0.99f;
                        dmgResist[2]*=1.25f;
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
			if(Buffs.Count>0)for(int i = 0; i<Buffs.Count; i++){
				Buffs[i].Update(npc);
            }
        }
        public override bool PreAI(NPC npc){
			bool a = true;
            Buffs.RemoveAll(BuffBase.GC);
			if(Buffs.Count>0)for(int i = 0; i<Buffs.Count; i++){
				a = a&&Buffs[i].PreUpdate(npc);
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
                for(int i = 0; a < npc.defense && i < corrs.Count; i++)a+=corrs[0].severity/2;
                damage+=a;
            }
        }
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection){
            List<CorrEffect> corrs = Buffs.FindAll(FindCorr).ConvertAll(MakeCorr);
            if(corrs.Count>0){
                int a = 0;
                for(int i = 0; a < npc.defense && i < corrs.Count; i++)a+=corrs[0].severity/2;
                damage+=a;
            }
        }
        public static bool FindCorr(BuffBase b){
            return b is CorrEffect;
        }
        public static CorrEffect MakeCorr(BuffBase b){
            return b as CorrEffect;
        }
        public static void AddBuff(BuffBase buff){
            buff.npc.GetGlobalNPC<EntropyGlobalNPC>().Buffs.Add(buff);
        }
    }
}