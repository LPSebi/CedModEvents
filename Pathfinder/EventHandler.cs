
using System.Collections.Generic;
using System.Linq;
using Interactables.Interobjects;
using Interactables.Interobjects.DoorUtils;
using PlayerRoles;
using MEC;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using Log = PluginAPI.Core.Log;


namespace Events.Pathfinder
{
    public class EventHandler
    {
        private static CoroutineHandle doorCoroutine;
        private static List<RoleTypeId> chaosList = new List<RoleTypeId>{RoleTypeId.ChaosMarauder,
            RoleTypeId.ChaosConscript,
            RoleTypeId.ChaosRepressor,
            RoleTypeId.ChaosRifleman};
        private static bool _hasWon = false;

        //on enable
        [PluginEvent(ServerEventType.RoundStart)]
        public void OnRoundStart()
        {
            //disable LCZ decontamination
            PluginAPI.Core.Server.RunCommand("/DECONTAMINATION DISABLE");
            
            //disable FF
            PluginAPI.Core.Server.FriendlyFire = false;
            
            //enable round lock
            if (!PluginAPI.Core.Round.IsLocked)
            {
                PluginAPI.Core.Round.IsLocked = true;
            }

            //if (doorCoroutine != null && doorCoroutine.IsRunning)
            //{
            //    Timing.KillCoroutines(doorCoroutine);
            //}
            

            foreach (var player in PluginAPI.Core.Player.GetPlayers())
            {
                //set all players d class
                if (player.Role != RoleTypeId.ClassD)
                {
                    Log.Info("Set role to Class D");
                    player.SetRole(RoleTypeId.ClassD);
                }
                else
                {
                    Log.Info("Player is already Class D");
                    
                }
            }

            doorCoroutine = Timing.RunCoroutine(DoorTimer());
        }

        [PluginEvent(ServerEventType.PlayerChangeRole)]
        public void OnPlayerChangeRole(PluginAPI.Core.Player player, PlayerRoleBase roleBase, RoleTypeId newRole, RoleChangeReason reason)
        {
            //check if winner has been decided
            
            
            if (chaosList.Contains(newRole))
            {
                Timing.KillCoroutines(doorCoroutine);
                
                PluginAPI.Core.Server.RunCommand("/close **");
                
                _hasWon = true;
                
                PluginAPI.Core.Server.SendBroadcast($"<color=red>{player.Nickname}</color> hat dieses Event gewonnen!", 100, Broadcast.BroadcastFlags.Normal, true);
                
                
                
                
            }

        }

        private static IEnumerator<float> DoorTimer()
        {
            Log.Info("Started Coroutine");
            
            if (_hasWon) {Log.Info("Stopping DoorTimer Coroutine: hasWon = true"); yield break;}
            while (true)
            { 
                //lock all doors
                //open all doors
                foreach (var door in UnityEngine.Object.FindObjectsOfType<DoorVariant>().Where(x => !(x is ElevatorDoor))) {
                    door.NetworkTargetState = true; // opens the door
                    
                    door.ServerChangeLock(DoorLockReason.AdminCommand, true);
                    
                }
                
                yield return Timing.WaitForSeconds(7.5f);
                
            }

        }

    }
}