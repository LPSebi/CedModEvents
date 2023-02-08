// // -----------------------------------------------------------------------
// // <copyright file="EventHandler.cs" company="fl0w">
// // Copyright (c) fl0w#1957 (https://fl0w.dev). All rights reserved.
// // Licensed under the CC BY-SA 3.0 license.
// // </copyright>
// // -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using LightContainmentZoneDecontamination;
using MEC;
using PlayerRoles;
using PlayerStatsSystem;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;

namespace Events.TDM
{
    public class EventHandler
    {
        
        public static List<string> checkpoints = new List<string>{"CHECKPOINT_LCZ_A", "CHECKPOINT_LCZ_B"};

        //on enable
        [PluginEvent(ServerEventType.RoundStart)]
        public void OnRoundStart()
        {
            //disable LCZ decontamination
            /*
            DecontaminationController.Singleton.DecontaminationOverride =
                DecontaminationController.DecontaminationStatus.Disabled;
            */
            //temporary fix because the upper isn't working
            Server.RunCommand("/DECONTAMINATION DISABLE");
            

            //disable FF
            Server.FriendlyFire = false;
            
            //lock all checkpoint doors
            foreach (var checkpoint in checkpoints)
            {
                PluginAPI.Core.Server.RunCommand("/lock " + checkpoint);
            }


            //enable round lock
            if (!Round.IsLocked) Round.IsLocked = true;

            List<Player> playerList = Player.GetPlayers();
            for (int i = 0; i < playerList.Count; i++)
            {
                //set role to scientist or classD
                if (i % 2 == 0)
                {
                    playerList[i].Role = RoleTypeId.Scientist;
                    playerList[i].ClearInventory();
                    //give scientist a weapon and ammo (GunE11SR)
                    playerList[i].AddItem(ItemType.GunE11SR);
                    playerList[i].SetAmmo(ItemType.Ammo556x45, 200);
                }
                else
                {
                    playerList[i].Role = RoleTypeId.ClassD;
                    playerList[i].ClearInventory();
                    //give scientist a weapon and ammo (AK)
                    playerList[i].AddItem(ItemType.GunAK);
                    playerList[i].AddItem(ItemType.Ammo762x39);
                    playerList[i].SetAmmo(ItemType.Ammo762x39, 200);
                }
                
                //clear inventory
                playerList[i].Health = TDM.Singleton.EventConfig.SpawnHealth;
                
            }
        }

        [PluginEvent(ServerEventType.PlayerDeath)]
        public void OnPlayerDeath(Player player, Player attacker, DamageHandlerBase damageHandler)
        {
            if (attacker == null) return;
            if (player == null) return;
            List<Player> player_list = Player.GetPlayers();
            List<RoleTypeId> classd_roles = new List<RoleTypeId> {RoleTypeId.ClassD, RoleTypeId.Spectator, RoleTypeId.None, RoleTypeId.CustomRole, RoleTypeId.Overwatch, RoleTypeId.Tutorial};
            List<RoleTypeId> scientist_roles = new List<RoleTypeId> {RoleTypeId.Scientist, RoleTypeId.Spectator, RoleTypeId.None, RoleTypeId.CustomRole, RoleTypeId.Overwatch, RoleTypeId.Tutorial};
            //check if one team is dead
            if (player_list.All(p => classd_roles.Contains(p.Role)))
            {
                Log.Info("classD win");
                //scientists win
                Server.SendBroadcast(string.Format(TDM.Singleton.EventConfig.WinText, "Class-D's"), 100,
                    Broadcast.BroadcastFlags.Normal, true);
            }
            else if (player_list.All(p => scientist_roles.Contains(p.Role)))
            {
                Log.Info("scientist win");
                //classD win
                Server.SendBroadcast(string.Format(TDM.Singleton.EventConfig.WinText, "Scientist's"), 100,
                    Broadcast.BroadcastFlags.Normal, true);
            }
            else
            {
                
                Log.Info("no one won yet");
                //no one won yet
                
            }

        }
    }
}