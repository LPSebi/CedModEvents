// // -----------------------------------------------------------------------
// // <copyright file="EventHandler.cs" company="fl0w">
// // Copyright (c) fl0w#1957 (https://fl0w.dev). All rights reserved.
// // Licensed under the CC BY-SA 3.0 license.
// // </copyright>
// // -----------------------------------------------------------------------
using System.Collections.Generic;
using System.Linq;
using Interactables.Interobjects.DoorUtils;
using LightContainmentZoneDecontamination;
using MEC;
using PlayerRoles;
using PlayerStatsSystem;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using UnityEngine;
using Log = PluginAPI.Core.Log;
using Random = System.Random;


namespace Events.PeanutEscape
{
    public class EventHandler
    {
        
        //TODO: Peanut kann türen checkpoint mit bypass aufmachen (nur checkpoints locken und alles andere immer auf lassen (loop)): fixx!!
        
        private static bool hasWon = false;
        public static int dclasscount = 0;
        public static bool initial_spawned = false;
        public static List<string> lockList = new List<string>{"CHECKPOINT_LCZ_A", "CHECKPOINT_LCZ_B", "LCZ_ARMORY", "LCZ_WC", "914", "330", "173_ARMORY"};
        static Random rnd = new Random();


        //on enable
        [PluginEvent(ServerEventType.RoundStart)]
        public void OnRoundStart()
        {

            //disable LCZ decontamination
            DecontaminationController.Singleton.NetworkDecontaminationOverride = DecontaminationController.DecontaminationStatus.Disabled;
            
            
            
            //lock all doors in lockList
            foreach (var door in lockList)
            {
                PluginAPI.Core.Server.RunCommand("/lock " + door);
            }
            

            //disable FF
            PluginAPI.Core.Server.FriendlyFire = false;

            //enable round lock
            if (!PluginAPI.Core.Round.IsLocked)
            {
                PluginAPI.Core.Round.IsLocked = true;
            }


            List<PluginAPI.Core.Player> playerlist = PluginAPI.Core.Player.GetPlayers();
            PluginAPI.Core.Player randomplayer = playerlist.ToArray().RandomItem();
            foreach (var player in playerlist)
            {
                //select random player
                if (player != randomplayer)
                {
                    //set player role
                    player.SetRole(RoleTypeId.Scp173);
                    player.SendBroadcast(
                        PeanutEscape.Singleton.EventConfig.PeanutText,
                        30,
                        Broadcast.BroadcastFlags.Normal,
                        true);
                }
                else
                {
                    //set player role
                    player.SetRole(RoleTypeId.ClassD);
                    player.SendBroadcast(
                        PeanutEscape.Singleton.EventConfig.DClassText,
                        30,
                        Broadcast.BroadcastFlags.Normal,
                        true);
                    initial_spawned = true;
                }

            }

            Timing.RunCoroutine(_loop());
        }

        private static IEnumerator<float> _loop()
        {
            
            yield return Timing.WaitForSeconds(10f);

            while (!hasWon)
            {
                yield return Timing.WaitForSeconds(0.01f);
                //check if there is only one d-class player left
                dclasscount = 0;
                List<PluginAPI.Core.Player> _players = PluginAPI.Core.Player.GetPlayers();
                for (int player = 0; player < _players.Count; player++)
                {
                    if (_players[player].Role == RoleTypeId.ClassD)
                    {
                        dclasscount++;
                    }

                }
                if (dclasscount == 1)
                {
                    //get the last player
                    for (int player = 0; player < _players.Count; player++)
                    {
                        if (_players[player].Role == RoleTypeId.ClassD)
                        {
                            //set player role
                            _players[player].Position = new Vector3(29.56641f, 991.885f, -25.23828f);
                            PluginAPI.Core.Server.SendBroadcast(
                                string.Format(PeanutEscape.Singleton.EventConfig.WinText, _players[player].Nickname),
                                30, 
                                Broadcast.BroadcastFlags.Normal, 
                                true);
                            Timing.RunCoroutine(tploop(_players[player]));
                            hasWon = true;
                            //set warhead to 10 seconds
                            PluginAPI.Core.Warhead.DetonationTime = 10f;
                            
                        }
                    }
                }
                


            }
        }
        
        private static IEnumerator<float> tploop(PluginAPI.Core.Player player)
        {
            yield return Timing.WaitForSeconds(0.01f);
            while (true)
            {
                yield return Timing.WaitForSeconds(0.01f);
                player.Position = new Vector3(29.56641f, 991.885f, -25.23828f);
                
            }
        }

        //on player death
        [PluginEvent(ServerEventType.PlayerDeath)]
        public void OnPlayerDeath(PluginAPI.Core.Player player, PluginAPI.Core.Player attacker, DamageHandlerBase damageHandler)
        {
           if (attacker == null) 
           { 
               Log.Info("Attacker is null");
               return;
           }
           if (player == null) 
           { 
               Log.Info("Player is null");
               return;
           }
           if (damageHandler == null)
           { 
               Log.Info("damageHandler is null");
               return;
           }
           Log.Info(player.Nickname + " has died");
           Log.Info(attacker.Nickname + " has killed " + player.Nickname);
           Log.Info("Player role: " + player.Role);
           Log.Info("Attacker role: " + attacker.Nickname);

           Timing.CallDelayed(1f, () => { });
           
           player.SetRole(RoleTypeId.Scp173);
           player.Health = 1;
           player.SendBroadcast( PeanutEscape.Singleton.EventConfig.PeanutText, 30, Broadcast.BroadcastFlags.Normal, true);
            
        }



    }
}