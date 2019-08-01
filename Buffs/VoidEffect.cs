using Entropy.Buffs;
using Microsoft.Xna.Framework;
using Terraria;

namespace Entropy.Buffs {
    public class VoidEffect : BuffBase{
        public override Color? color => new Color(235,255,245,100);
        public VoidEffect(NPC npc, int duration) : base(npc){
            this.duration = duration;
        }
        public override void ModifyHitItem(Player attacker, Item item, NPC target, ref int damage, ref bool crit){
            damage+=(int)(target.life*(crit?0.05f:0.01f));
        }
        public override void ModifyHitProjectile(Projectile proj, NPC target, ref int damage, ref bool crit){
            damage+=(int)(target.life*(crit?0.05f:0.01f));
        }
    }
}