using System;
using System.Reflection;
using Entropy.Items;
using Entropy.NPCs;
using Microsoft.Xna.Framework;
using Terraria;

namespace Entropy.Buffs {
    public class PlayerBuffBase{
        public virtual bool isActive{
            get{return duration>0&&victim.active;}
            set{if(!value)duration=0;}
        }
        public string Name{
            get{return this.GetType().Name;}
        }
        public int duration;
        public Player victim;
        public Entity cause;
        public virtual Color? color {get{return null;} set{}}
        public PlayerBuffBase(Player victim, Entity cause = null){
            this.victim = victim;
            this.cause = cause;
        }
        public virtual void ModifyHitItem(Player attacker, EntModItem item, NPC target, ref int damage, ref bool crit, ref float[] dr){}
        public virtual void Update(Player player){
            duration--;
            if(color.HasValue){
                Color c = color.Value;
                Lighting.AddLight(player.Center, c.R/200, c.G/200, c.B/200);
            }
        }
        public virtual void OnDeath(Player player){duration=-1;}
        public override string ToString(){return Name;}
        public override bool Equals(object a2){
            return this.GetType() == a2.GetType();
        }
        public static bool GC(PlayerBuffBase buff){return !buff.isActive;}
    }
}