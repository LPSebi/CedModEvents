// // -----------------------------------------------------------------------
// // <copyright file="Config.cs" company="fl0w">
// // Copyright (c) fl0w#1957 (https://fl0w.dev). All rights reserved.
// // Licensed under the CC BY-SA 3.0 license.
// // </copyright>
// // -----------------------------------------------------------------------

using System.ComponentModel;
using CedMod.Addons.Events;

namespace Events.TDM
{
    public class Config : IEventConfig
    {
        [Description("The text displayed when a user wins the event")]
        public string WinText { get; set; } = "<color=red>{0}</color> haben dieses Event gewonnen!";
        
        [Description("The amount of health the player spawns with")]
        public int SpawnHealth { get; set; } = 200;

        [Description("Is the event enabled?")] 
        public bool IsEnabled { get; set; } = true;
    }
}