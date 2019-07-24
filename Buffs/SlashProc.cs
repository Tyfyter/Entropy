using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Entropy.Buffs
{
	public class SlashProc : ModBuff
	{
		//int realtime = 360;

        List<int> times = new List<int>{};

        List<int> procs = new List<int>{};
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Bleeding");
			Description.SetDefault("");
            Main.pvpBuff[Type] = false;  //Tells the game if pvp buff or not. 
			canBeCleared = false;
			//Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(NPC npc, ref int buffIndex){
            for (int i = 0; i < procs.Count; i++)
            {
                npc.lifeRegen -= procs[i];
                times[i]--;
                if(times[i] <= 0){
                    times.RemoveAt(i);
                    procs.RemoveAt(i);
                }
            }
			if(times.Count <= 0){
                npc.buffTime[buffIndex] = Math.Min(npc.buffTime[buffIndex],1);
				//npc.DelBuff(buffIndex);
			}else{
                npc.buffTime[buffIndex]++;
            }
		}
		public override bool ReApply(NPC npc, int time, int buffIndex){
			//realtime = 360;
            times.Add(360);
			procs.Add(time);
			return false;
		}
	}
}