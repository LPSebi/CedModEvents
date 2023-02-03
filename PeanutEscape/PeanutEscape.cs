﻿// // -----------------------------------------------------------------------
// // <copyright file="PeanutEscape.cs" company="fl0w">
// // Copyright (c) fl0w#1957 (https://fl0w.dev). All rights reserved.
// // Licensed under the CC BY-SA 3.0 license.
// // </copyright>
// // -----------------------------------------------------------------------
using CedMod.Addons.Events;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using EventManager = PluginAPI.Events.EventManager;

namespace Events.PeanutEscape
{
    public class PeanutEscape : IEvent
    {
        
        public static bool IsRunning = false;
        public static PeanutEscape Singleton { get; private set; }

        [PluginUnload]
        public void OnDisabled()
        {
            StopEvent();
        }

        [PluginConfig] 
        public Config EventConfig;
        
        [PluginEntryPoint("PeanutEscape Event", "0.1.0",
            "Locked in the Light Containment, one player becomes Peanut and all other D-boi. Every D-boi that dies becomes Peanut. The last D-Class person wins.", "fl0w#1957")]
        public void OnEnabled()
        {
            Singleton = this;
            Handler = PluginHandler.Get(this);
        }
        public PluginHandler Handler;
        
        

        public string EventName { get; } = "PeanutEscape Event";
        public string EvenAuthor { get; } = "fl0w#1957";

        public string EventDescription { get; set; } =
            "Locked in the Light Containment, one player becomes Peanut and all other D-boi. Every D-boi that dies becomes Peanut. The last D-Class person wins.";
        public string EventPrefix { get; } = "pe";
        public bool BulletHolesAllowed { get; set; } = true;
        public IEventConfig Config => EventConfig;
        
        
        public void PrepareEvent()
        {
            Log.Info("PeanutEscape Event is preparing");
            IsRunning = true;
            Log.Info("PeanutEscape Event is prepared");
            EventManager.RegisterEvents<EventHandler>(this);
        }

        public void StopEvent()
        {
            IsRunning = false;
            EventManager.UnregisterEvents<EventHandler>(this);
        }
    }
}