using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Entropy.Dusts{
    class SlashProcDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.noLight = true;
            dust.rotation = 0;
            dust.scale = 2;
            dust.alpha /= 2;
        }

        public override bool Update(Dust dust)
        {
            dust.scale += 0.5f;
            if (dust.scale >= 1.5f)
            {
                dust.active = false;
            }
            return false;
        }
    }
}