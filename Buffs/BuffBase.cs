using System;
using System.Reflection;
using Entropy.NPCs;
using Microsoft.Xna.Framework;
using Terraria;

namespace Entropy.Buffs {
    public class BuffBase{
        public virtual bool isActive{
            get{return duration>0&&npc.active;}
            set{if(!value)duration=0;}
        }
        public string Name{
            get{return this.GetType().Name;}
        }
        public int duration;
        public NPC npc;
        public Entity cause;
        public virtual Color? color {get{return null;} set{}}
        public BuffBase(NPC npc, Entity cause = null){
            this.npc = npc;
            this.cause = cause;
        }
        public virtual bool PreUpdate(NPC npc, bool canceled){return true;}
        public virtual void Update(NPC npc){duration--;}
        public virtual void ModifyHit(NPC npc, Player target, ref int damage, ref bool crit){}
        public virtual void ModifyHitItem(Player attacker, Item item, NPC target, ref int damage, ref bool crit){}
        public virtual void ModifyHitProjectile(Projectile proj, NPC target, ref int damage, ref bool crit){}
        public virtual void ModifyHitNPC(NPC npc, NPC target, ref int damage, ref float knockback, ref bool crit){}
        public virtual void OnDeath(NPC npc){duration=-1;}
        public override string ToString(){return Name;}
        public override bool Equals(object a2){
            return this.GetType() == a2.GetType();
        }
        public static bool GC(BuffBase buff){return !buff.isActive;}
        //{0:"Slash", 1:"Impact", 2:"Puncture", 3:"Cold", 4:"Electric", 5:"Heat", 6:"Toxic", 7:"Blast", 8:"Corrosive", 9:"Gas", 10:"Magnetic", 11:"Radiation", 12:"Viral", 13:"True", 14:"Void"}
        public static BuffBase GetFromIndex(NPC npc, int type, int dmg, Player player){
            switch (type){
                case 0:
                return new BleedEffect(npc, (int)(dmg*0.35f), 360, cause:player);
                case 1:
                return new ImpactEffect(npc, 10);
                case 2:
                npc.StrikeNPC(dmg, 0, 0, false);
                return new PuncEffect(npc, 360);
                case 3:
                return new ColdEffect(npc, (npc.boss?60:360)/(npc.HasBuff<ColdEffect>()?2:1), 3-(Math.Max((npc.CountBuff<ColdEffect>()),1)));
                case 4:
                return new ElecEffect(npc, dmg/2, 1);
                case 5:
                return new HeatEffect(npc, dmg/(1+npc.CountBuff<HeatEffect>()), 360);
                case 6:
                return new ToxicEffect(npc, dmg/3, 360);
                case 7:
                return new BlastEffect(npc, npc.boss?10:30);
                case 8:
                return new CorrEffect(npc, 30);
                case 9:
                return new GasEffect(npc, dmg/3, 360);
                case 10:
                return new MagEffect(npc, 360);
                case 11:
                return new RadEffect(npc, 360);
                case 12:
                return npc.HasBuff<ViralEffect>()?new BuffBase(npc):new ViralEffect(npc, 360);
                case 14:
                return new VoidEffect(npc, 360);
                default:
                return new BuffBase(npc);
            }
        }
    }
}