using CedMod;
using CedMod.Addons.Events;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using EventManager = PluginAPI.Events.EventManager;

namespace Events.TDM
{
    public class TDM: IEvent
    {
        
        public static TDM Singleton { get; private set; }

        [PluginUnload]
        public void OnDisabled()
        {
            StopEvent();
        }

        [PluginConfig] 
        public Config EventConfig;
        
        [PluginEntryPoint("PeanutEscape Event", "0.1.0",
            "50 % of the players are D-Class and the other 50 % are Scientists. They have to kill each other. The last team standing wins.", "fl0w#1957")]
        public void OnEnabled()
        {
            Singleton = this;
            Handler = PluginHandler.Get(this);
        }
        public PluginHandler Handler;
        
        

        public string EventName { get; } = "PeanutEscape Event";
        public string EvenAuthor { get; } = "fl0w#1957";

        public string EventDescription { get; set; } =
            "50 % of the players are D-Class and the other 50 % are Scientists. They have to kill each other. The last team standing wins.";
        public string EventPrefix { get; } = "pe";
        public bool BulletHolesAllowed { get; set; } = true;
        public IEventConfig Config => EventConfig;
        
        
        public void PrepareEvent()
        {
            Log.Info("PeanutEscape Event is preparing");
            Singleton = this;
            Log.Info("PeanutEscape Event is prepared");
            EventManager.RegisterEvents<EventHandler>(this);
        }

        public void StopEvent()
        {
            Singleton = null;
            EventManager.UnregisterEvents<EventHandler>(this);
        }
        
    }
}