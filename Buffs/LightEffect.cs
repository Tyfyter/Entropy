using System;
using Entropy.Buffs;
using Entropy.NPCs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace Entropy.Buffs {
    public class LightEffect : BuffBase{
        public override int priority => 15;
        public override Color? color {get; set;} = new Color(225, 255, 255, 100);
        //new string[15] {"Slash", "Impact", "Puncture", "Cold", "Electric", "Heat", "Toxic", "Blast", "Corrosive", "Gas", "Magnetic", "Radiation", "Viral", "True", "Void"}
        float dmg = 0;
        int hits = 0;
        int crits = 0;
        public override int value => (int)(duration-(npc.defense/2)+(dmg/2));
        public LightEffect(NPC npc, int duration) : base(npc){
            this.duration = duration;
        }
        public override void ModifyHitItem(Player attacker, Item item, NPC target, ref int damage, ref bool crit){
            dmg+=damage/3f;
            hits++;
            if(crit)crits++;
            Main.NewText("light");
        }
        public override void ModifyHitProjectile(Projectile projectile, NPC target, ref int damage, ref bool crit){
            dmg+=damage/5f;
            hits++;
            if(crit)crits++;
        }
        public override void Update(NPC npc){
            base.Update(npc);
            Lighting.AddLight(npc.Center, color.Value.ToVector3()*dmg/255/1024);
            if(!isActive||npc.type==NPCID.EyeofCthulhu){
                npc.StrikeNPC((int)dmg, 0, 0, Main.rand.NextFloat()<((float)crits/hits));
                return;
            }
            dmg-=0.2f;
        }
    }
}