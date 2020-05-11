using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.ModLoader;

namespace Enrtopy.Items{
    public class NarulSound : ModSound
    {
        public override SoundEffectInstance PlaySound(ref SoundEffectInstance soundInstance, float volume, float pan, SoundType type)
        {
			soundInstance = sound.CreateInstance();
			soundInstance.Volume = volume * 0.2f;
			soundInstance.Pan = pan;
			soundInstance.Pitch = Main.rand.Next(-6, 7) /30f;
			return soundInstance;
        }
    }
}
