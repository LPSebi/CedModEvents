using CedMod;
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
            "Abgeriegelt in der Light Containment wird ein Spieler zu Peanut und alle anderen D-boi. Jeder D-boi der stirbt wird zu Peanut. Die letzte D-Class person gewinnt.", "fl0w#1957")]
        public void OnEnabled()
        {
            Singleton = this;
            Handler = PluginHandler.Get(this);
        }
        public PluginHandler Handler;
        
        

        public string EventName { get; } = "PeanutEscape Event";
        public string EvenAuthor { get; } = "fl0w#1957";

        public string EventDescription { get; set; } =
            "Abgeriegelt in der Light Containment wird ein Spieler zu Peanut und alle anderen D-boi. Jeder D-boi der stirbt wird zu Peanut. Die letzte D-Class person gewinnt.";
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