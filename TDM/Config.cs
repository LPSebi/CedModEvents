﻿// // -----------------------------------------------------------------------
// // <copyright file="Config.cs" company="fl0w">
// // Copyright (c) fl0w#1957 (https://fl0w.dev). All rights reserved.
// // Licensed under the CC BY-SA 3.0 license.
// // </copyright>
// // -----------------------------------------------------------------------

using CedMod.Addons.Events;

namespace Events.TDM
{
    public class Config: IEventConfig
    {
        public bool IsEnabled { get; set; }
        
        public string WinText { get; set; } = "<color=red>{0}</color> hat dieses Event gewonnen!";

        
    }
}