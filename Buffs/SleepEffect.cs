using Entropy.Buffs;
using Terraria;
using Terraria.Graphics.Shaders;

namespace Entropy.Buffs {
    public class SleepEffect : BuffBase{
        int durab = 0;
        public SleepEffect(NPC npc, int duration, int count = 0) : base(npc){
            this.duration = duration;
            durab = count;
        }
        public override void ModifyHitItem(Player attacker, Item item, NPC target, ref int damage, ref bool crit){
            damage*=2;
            if(durab-->0)return;
            duration = duration>0?1:0;
        }
        public override void ModifyHitProjectile(Projectile proj, NPC target, ref int damage, ref bool crit){
            damage*=2;
            if(durab-->0)return;
            duration = duration>0?1:0;
        }
        public override void Update(NPC npc){
            if((int)Main.time%2!=0){
                Dust.NewDustDirect(npc.position, npc.width, npc.height, 267).shader = GameShaders.Armor.GetShaderFromItemId(3530);
            }
            base.Update(npc);
        }
        public override bool PreUpdate(NPC npc, bool canceled){
            return false;
        }
    }
}