// // -----------------------------------------------------------------------
// // <copyright file="EventHandler.cs" company="fl0w">
// // Copyright (c) fl0w#1957 (https://fl0w.dev). All rights reserved.
// // Licensed under the CC BY-SA 3.0 license.
// // </copyright>
// // -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.Attachments;
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
        
        public static List<string> lockList = new List<string>{"CHECKPOINT_LCZ_A", "CHECKPOINT_LCZ_B", "LCZ_ARMORY", "LCZ_WC", "914", "330", "173_ARMORY", "173_CONNECTOR", "GR18"};

        private static void SetupWeapon(Firearm firearm)
        {
            if (!AttachmentsServerHandler.PlayerPreferences.TryGetValue(firearm.Owner, out var value) || !value.TryGetValue(firearm.ItemTypeId, out uint attachments))
                attachments = AttachmentsUtils.GetRandomAttachmentsCode(firearm.ItemTypeId);

            FirearmStatusFlags firearmStatusFlags = FirearmStatusFlags.MagazineInserted;
            if (firearm.HasAdvantageFlag(AttachmentDescriptiveAdvantages.Flashlight))
                firearmStatusFlags |= FirearmStatusFlags.FlashlightEnabled;

            firearm.ApplyAttachmentsCode(attachments, true);
            firearm.Status = new FirearmStatus(firearm.AmmoManagerModule.MaxAmmo, firearmStatusFlags, firearm.GetCurrentAttachmentsCode());
        }

        //on enable
        [PluginEvent(ServerEventType.RoundStart)]
        public void OnRoundStart()
        {
            //disable LCZ decontamination
            DecontaminationController.Singleton.NetworkDecontaminationOverride = DecontaminationController.DecontaminationStatus.Disabled;
            
            //disable FF
            Server.FriendlyFire = false;
            
            //lock all checkpoint doors
            foreach (var checkpoint in lockList)
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
                    Firearm firearm = (Firearm)playerList[i].AddItem(ItemType.GunE11SR);
                    SetupWeapon(firearm);
                    playerList[i].SetAmmo(ItemType.Ammo556x45, 200);
                }
                else
                {
                    playerList[i].Role = RoleTypeId.ClassD;
                    playerList[i].ClearInventory();
                    //give scientist a weapon and ammo (AK)
                    Firearm firearm = (Firearm)playerList[i].AddItem(ItemType.GunAK);
                    SetupWeapon(firearm);
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
            List<Player> playerList = Player.GetPlayers();
            List<RoleTypeId> classdRoles = new List<RoleTypeId> {RoleTypeId.ClassD, RoleTypeId.Spectator, RoleTypeId.None, RoleTypeId.CustomRole, RoleTypeId.Overwatch, RoleTypeId.Tutorial};
            List<RoleTypeId> scientistRoles = new List<RoleTypeId> {RoleTypeId.Scientist, RoleTypeId.Spectator, RoleTypeId.None, RoleTypeId.CustomRole, RoleTypeId.Overwatch, RoleTypeId.Tutorial};
            //check if one team is dead
            if (playerList.All(p => classdRoles.Contains(p.Role)))
            {
                Log.Info("classD win");
                //scientists win
                Server.SendBroadcast(string.Format(TDM.Singleton.EventConfig.WinText, "Class-D's"), 100,
                    Broadcast.BroadcastFlags.Normal, true);
            }
            else if (playerList.All(p => scientistRoles.Contains(p.Role)))
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