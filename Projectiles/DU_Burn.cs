namespace Entropy.Projectiles {
    public class DU_Burn : EntModProjectile {
        public override string Texture => "Terraria/Projectile_694";
        public override void SetDefaults(){
            projectile.CloneDefaults(694);
			dmgratio = dmgratiobase = new float[15] {0f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f};
        }
    }
}