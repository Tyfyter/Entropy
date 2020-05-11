using System;
using Entropy.NPCs;
using Terraria;
using Terraria.ID;

namespace Entropy {
    public static class DmgCalcs {
        //{0:"Slash", 1:"Impact", 2:"Puncture", 3:"Cold", 4:"Electric", 5:"Heat", 6:"Toxic", 7:"Blast", 8:"Corrosive", 9:"Gas", 10:"Magnetic", 11:"Radiation", 12:"Viral", 13:"True", 14:"Void"}
        public static readonly Func<float,NPC,float>[] dmgFuncs = new Func<float,NPC,float>[]{Slash,Impact,Puncture,Cold,Electric,Heat,Toxic,Frostburn,Corrosive,Gas,Magnetic,Radiation,Viral,True,Void};
#region funcs
        public static float Slash(float dmg, NPC target){
            if(Main.rand.NextDouble()<target.GetGlobalNPC<EntropyGlobalNPC>().dmgDodge[0])return 0;
            return (dmg*target.GetGlobalNPC<EntropyGlobalNPC>().dmgResist[0])*(target.HasBuff(BuffID.Stoned)?0.85f:1);
        }
        public static float Impact(float dmg, NPC target){
            if(Main.rand.NextDouble()<target.GetGlobalNPC<EntropyGlobalNPC>().dmgDodge[1])return 0;
            return dmg*target.GetGlobalNPC<EntropyGlobalNPC>().dmgResist[1];
        }
        public static float Puncture(float dmg, NPC target){
            if(Main.rand.NextDouble()<target.GetGlobalNPC<EntropyGlobalNPC>().dmgDodge[2])return 0;
            return (dmg*target.GetGlobalNPC<EntropyGlobalNPC>().dmgResist[2])*(target.HasBuff(BuffID.Stoned)?1.25f:1);
        }
        public static float Cold(float dmg, NPC target){
            if(Main.rand.NextDouble()<target.GetGlobalNPC<EntropyGlobalNPC>().dmgDodge[3])return 0;
            return dmg*target.GetGlobalNPC<EntropyGlobalNPC>().dmgResist[3];
        }
        public static float Electric(float dmg, NPC target){
            if(Main.rand.NextDouble()<target.GetGlobalNPC<EntropyGlobalNPC>().dmgDodge[4])return 0;
            return dmg*target.GetGlobalNPC<EntropyGlobalNPC>().dmgResist[4];
        }
        public static float Heat(float dmg, NPC target){
            if(Main.rand.NextDouble()<target.GetGlobalNPC<EntropyGlobalNPC>().dmgDodge[5])return 0;
            return dmg*target.GetGlobalNPC<EntropyGlobalNPC>().dmgResist[5];
        }
        public static float Toxic(float dmg, NPC target){
            if(Main.rand.NextDouble()<target.GetGlobalNPC<EntropyGlobalNPC>().dmgDodge[6])return 0;
            return dmg*target.GetGlobalNPC<EntropyGlobalNPC>().dmgResist[6];
        }
        public static float Frostburn(float dmg, NPC target){
            if(Main.rand.NextDouble()<target.GetGlobalNPC<EntropyGlobalNPC>().dmgDodge[7])return 0;
            return dmg*target.GetGlobalNPC<EntropyGlobalNPC>().dmgResist[7];
        }
        public static float Corrosive(float dmg, NPC target){
            if(Main.rand.NextDouble()<target.GetGlobalNPC<EntropyGlobalNPC>().dmgDodge[8])return 0;
            return dmg*target.GetGlobalNPC<EntropyGlobalNPC>().dmgResist[8];
        }
        public static float Gas(float dmg, NPC target){
            if(Main.rand.NextDouble()<target.GetGlobalNPC<EntropyGlobalNPC>().dmgDodge[9])return 0;
            return dmg*target.GetGlobalNPC<EntropyGlobalNPC>().dmgResist[9];
        }
        public static float Magnetic(float dmg, NPC target){
            if(Main.rand.NextDouble()<target.GetGlobalNPC<EntropyGlobalNPC>().dmgDodge[10])return 0;
            return dmg*target.GetGlobalNPC<EntropyGlobalNPC>().dmgResist[10];
        }
        public static float Radiation(float dmg, NPC target){
            if(Main.rand.NextDouble()<target.GetGlobalNPC<EntropyGlobalNPC>().dmgDodge[11])return 0;
            return dmg*target.GetGlobalNPC<EntropyGlobalNPC>().dmgResist[11];
        }
        public static float Viral(float dmg, NPC target){
            if(Main.rand.NextDouble()<target.GetGlobalNPC<EntropyGlobalNPC>().dmgDodge[12])return 0;
            return dmg*target.GetGlobalNPC<EntropyGlobalNPC>().dmgResist[12];
        }
        public static float True(float dmg, NPC target){
            if(Main.rand.NextDouble()<target.GetGlobalNPC<EntropyGlobalNPC>().dmgDodge[13])return 0;
            return (dmg*target.GetGlobalNPC<EntropyGlobalNPC>().dmgResist[13])+(target.defense/2);
        }
        public static float Void(float dmg, NPC target){
            if(Main.rand.NextDouble()<target.GetGlobalNPC<EntropyGlobalNPC>().dmgDodge[14])return 0;
            return dmg*target.GetGlobalNPC<EntropyGlobalNPC>().dmgResist[14];
        }
#endregion funcs
    }
    /*public enum VulTypes {
        NONE,
        DRY,

    }
    public static class VulCalcs {
        
    }*/
}