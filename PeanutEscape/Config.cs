// // -----------------------------------------------------------------------
// // <copyright file="Config.cs" company="fl0w">
// // Copyright (c) fl0w#1957 (https://fl0w.dev). All rights reserved.
// // Licensed under the CC BY-SA 3.0 license.
// // </copyright>
// // -----------------------------------------------------------------------
using System.ComponentModel;
using CedMod.Addons.Events;

namespace Events.PeanutEscape
{
    public sealed class Config: IEventConfig
    {
        [Description("Indicates whether the event is enabled or not")]
        public bool IsEnabled { get; set; } = true;
        
        [Description("The text displayed when a user wins the event")]
        public string WinText { get; set; } = "<color=red>{0}</color> hat dieses Event gewonnen!";
        
        [Description("The text displayed when a player is SCP-173")]
        public string PeanutText { get; set; } = "Du bist <color=red>SCP-173</color>. Jede Person, die du tötest wird auch zu <color=red>SCP-173</color> hat vollen schild jedoch nur 1 HP. Vernichte sie alle";
        
        [Description("The text displayed when a player is D-Class")]
        public string DClassText { get; set; } = "Du bist <color=red>Class D</color>. Versuch als längstes zu überleben. SCP-173 kommt!";
    }
}