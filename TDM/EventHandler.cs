﻿// // -----------------------------------------------------------------------
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

        private static TDM _pl;

        public EventHandler(TDM plugin)
        {
            _pl = plugin;
        }

        //on enable
        [PluginEvent(ServerEventType.RoundStart)]
        public void OnRoundStart()
        {
            //disable LCZ decontamination
            DecontaminationController.Singleton.DecontaminationOverride =
                DecontaminationController.DecontaminationStatus.Disabled;

            //disable FF
            Server.FriendlyFire = false;


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
                    playerList[i].AddItem(ItemType.Ammo556x45);
                }
                else
                {
                    playerList[i].Role = RoleTypeId.ClassD;
                    playerList[i].ClearInventory();
                    //give scientist a weapon and ammo (AK)
                    playerList[i].AddItem(ItemType.GunAK);
                    playerList[i].AddItem(ItemType.Ammo762x39);
                }
                
                //clear inventory
                playerList[i].Health = _pl.EventConfig.SpawnHealth;
                
            }
        }

        [PluginEvent(ServerEventType.PlayerDeath)]
        public void OnPlayerDeath(Player player, Player attacker, DamageHandlerBase damageHandler)
        {
            if (attacker == null) return;
            if (player == null) return;
            List<Player> player_list = Player.GetPlayers();
            //check if only 1 player is left of any team
            if (player_list.Count(x => x.Role == RoleTypeId.ClassD) == 1)
            {
                //get the last player
                Player lastPlayer = player_list.First(x => x.Role == RoleTypeId.ClassD);
                //announce the winner
            }
            else if (player_list.Count(x => x.Role == RoleTypeId.Scientist) == 1)
            {
                //get the last player
                Player lastPlayer = player_list.First(x => x.Role == RoleTypeId.Scientist);
                //announce the winner
                Server.SendBroadcast(string.Format(_pl.EventConfig.WinText, player.Nickname), 100,
                    Broadcast.BroadcastFlags.Normal, true);

            }
            else if (player_list.Count(x => x.Role == RoleTypeId.ClassD) == 0)
            {
                //announce the winner
            }
            else if (player_list.Count(x => x.Role == RoleTypeId.Scientist) == 0)
            {
                //announce the winner
            }
        }
    }
}