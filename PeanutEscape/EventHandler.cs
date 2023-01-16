using System.Collections.Generic;
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
        public static List<string> checkpoints = new List<string>{"CHECKPOINT_LCZ_A", "CHECKPOINT_LCZ_B"};
        static Random rnd = new Random();
        
        
        //on enable
        [PluginEvent(ServerEventType.RoundStart)]
        public void OnRoundStart()
        {
            
            //check if 3 or more players are in the server
            //if (PluginAPI.Core.Player.GetPlayers().Count < 3)
            //{
            //    Log.Info("Not enough players to start the event");
            //    PluginAPI.Core.Round.End();
            //    return;
            //}
            
            //disable LCZ decontamination
            PluginAPI.Core.Server.RunCommand("/DECONTAMINATION DISABLE");
            
            //disable all checkpoints
            foreach (var checkpoint in checkpoints)
            {
                PluginAPI.Core.Server.RunCommand("/lock " + checkpoint);
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
                        "Du bist <color=red>SCP-173</color>. Jede Person, die du tötest wird auch zu <color=red>SCP-173</color> hat vollen schild jedoch nur 1 HP. Vernichte sie alle!",
                        30,
                        Broadcast.BroadcastFlags.Normal,
                        true);
                }
                else
                {
                    //set player role
                    player.SetRole(RoleTypeId.ClassD);
                    player.SendBroadcast(
                        "Du bist <color=red>Class D</color>. Versuch als längstes zu überleben. SCP-173 kommt!",
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
                                $"<color=red>{_players[player].Nickname}</color> hat dieses Event gewonnen!",
                                30, 
                                Broadcast.BroadcastFlags.Normal, 
                                true);
                            PluginAPI.Core.Server.RunCommand("/SERVER_EVENT DETONATION_INSTANT");
                            Timing.RunCoroutine(tploop(_players[player]));
                            hasWon = true;
                            
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
           player.SendBroadcast( "Du bist jetzt <color=red>SCP-173</color>. Jede Person, die du tötest wird auch zu <color=red>SCP-173</color> hat vollen schild jedoch nur 1 HP.", 30, Broadcast.BroadcastFlags.Normal, true);
            
        }



    }
}