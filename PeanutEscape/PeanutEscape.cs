// // -----------------------------------------------------------------------
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
        
        public static PeanutEscape Singleton { get; private set; }
        public EventHandler Handler;


        [PluginConfig] 
        public Config EventConfig;
        



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
            Singleton = this;
            Log.Info("PeanutEscape Event is prepared");
            EventManager.RegisterEvents(this, Handler);
        }

        public void StopEvent()
        {
            Singleton = null;
            EventManager.UnregisterEvents(this, Handler);
        }

        [PluginUnload]
        public void OnDisabled()
        {
            StopEvent();
        }
        
        [PluginEntryPoint("PeanutEscape Event", "0.1.0",
            "Locked in the Light Containment, one player becomes Peanut and all other D-boi. Every D-boi that dies becomes Peanut. The last D-Class person wins.", "fl0w#1957")]
        public void OnEnabled()
        {
            Singleton = this;
            Handler = new EventHandler();
        }
    }
}
