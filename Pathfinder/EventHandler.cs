// // -----------------------------------------------------------------------
// // <copyright file="EventHandler.cs" company="fl0w">
// // Copyright (c) fl0w#1957 (https://fl0w.dev). All rights reserved.
// // Licensed under the CC BY-SA 3.0 license.
// // </copyright>
// // -----------------------------------------------------------------------
using System.Collections.Generic;
using System.Linq;
using CommandSystem.Commands.RemoteAdmin.Decontamination;
using Interactables.Interobjects;
using Interactables.Interobjects.DoorUtils;
using LightContainmentZoneDecontamination;
using MEC;
using PlayerRoles;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using UnityEngine;

namespace Events.Pathfinder
{
    public class EventHandler
    {
        private static CoroutineHandle doorCoroutine;

        private static readonly List<RoleTypeId> chaosList = new List<RoleTypeId>
        {
            RoleTypeId.ChaosMarauder,
            RoleTypeId.ChaosConscript,
            RoleTypeId.ChaosRepressor,
            RoleTypeId.ChaosRifleman
        };

        private static bool _hasWon;

        private static Pathfinder _pl;

        public EventHandler(Pathfinder plugin)
        {
            _pl = plugin;
        }

        //on enable
        [PluginEvent(ServerEventType.RoundStart)]
        public void OnRoundStart()
        {
            //disable LCZ decontamination
            DecontaminationController.Singleton.DecontaminationOverride = DecontaminationController.DecontaminationStatus.Disabled;

            //disable FF
            Server.FriendlyFire = false;

            //enable round lock
            if (!Round.IsLocked) Round.IsLocked = true;

            //if (doorCoroutine != null && doorCoroutine.IsRunning)
            //{
            //    Timing.KillCoroutines(doorCoroutine);
            //}


            foreach (var player in Player.GetPlayers())
            {
                //set all players d class
                Log.Debug("Set role to Class D");
                player.SetRole(RoleTypeId.ClassD);
            }

            doorCoroutine = Timing.RunCoroutine(DoorTimer());
        }

        [PluginEvent(ServerEventType.PlayerChangeRole)]
        public void OnPlayerChangeRole(Player player, PlayerRoleBase roleBase, RoleTypeId newRole,
            RoleChangeReason reason)
        {
            //check if winner has been decided
            
            if (chaosList.Contains(newRole))
            {
                Timing.KillCoroutines(doorCoroutine);

                Server.RunCommand("/close **");
                foreach (var door in Object.FindObjectsOfType<DoorVariant>())
                {
                    // close the door
                    door.NetworkTargetState = false; 
                }

                _hasWon = true;

                Server.SendBroadcast(string.Format(_pl.EventConfig.WinText, player.Nickname), 100,
                    Broadcast.BroadcastFlags.Normal, true);
            }
        }

        private static IEnumerator<float> DoorTimer()
        {
            Log.Debug("Started Coroutine");

            if (_hasWon)
            {
                Log.Debug("Stopping DoorTimer Coroutine: hasWon = true");
                yield break;
            }

            while (true)
            {
                //lock all doors
                //open all doors
                foreach (var door in Object.FindObjectsOfType<DoorVariant>()
                             .Where(x => !(x is ElevatorDoor)))
                {
                    door.NetworkTargetState = true; // opens the door

                    door.ServerChangeLock(DoorLockReason.AdminCommand, true);
                }

                yield return Timing.WaitForSeconds(7.5f);
            }
        }
    }
}