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
    }
}