// // -----------------------------------------------------------------------
// // <copyright file="Pathfinder.cs" company="fl0w">
// // Copyright (c) fl0w#1957 (https://fl0w.dev). All rights reserved.
// // Licensed under the CC BY-SA 3.0 license.
// // </copyright>
// // -----------------------------------------------------------------------
using CedMod.Addons.Events;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using EventManager = PluginAPI.Events.EventManager;

namespace Events.Pathfinder
{
    public class Pathfinder : IEvent
    {
        [PluginConfig] public Config EventConfig;

        public PluginHandler Handler;
        public static Pathfinder Singleton { get; set; }


        public string EventName { get; } = "Pathfinder Event";
        public string EvenAuthor { get; } = "fl0w#1957";

        public string EventDescription { get; set; } =
            "All players spawn as D-Class. All doors are opened. The first to reach the exit wins.";

        public string EventPrefix { get; } = "pf";
        public bool BulletHolesAllowed { get; set; } = true;
        public IEventConfig Config => EventConfig;


        public void PrepareEvent()
        {
            Log.Info("Pathfinder Event by fl0w#1957 is preparing");
            Log.Info("Pathfinder Event by fl0w#1957 is prepared ");
            EventManager.RegisterEvents<EventHandler>(this);
        }

        public void StopEvent()
        {
            Singleton = null;
            EventManager.UnregisterEvents<EventHandler>(this);
        }

        [PluginUnload]
        public void OnDisabled()
        {
            StopEvent();
        }

        [PluginEntryPoint("Pathfinder Event", "0.1.0",
            "All players spawn as D-Class. All doors are opened. The first to reach the exit wins", "fl0w#1957")]
        public void OnEnabled()
        {
            Singleton = this;
            Handler = PluginHandler.Get(this);
        }
    }
}