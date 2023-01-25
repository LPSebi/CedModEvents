using CedMod.Addons.Events;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using EventManager = PluginAPI.Events.EventManager;

namespace Events.Pathfinder
{
    public class Pathfinder : IEvent
    {
        public static Pathfinder Singleton { get; private set; }

        [PluginUnload]
        public void OnDisabled()
        {
            StopEvent();
        }

        [PluginConfig] 
        public Config EventConfig;
        
        
        [PluginEntryPoint("Pathfinder Event", "0.1.0", "Alle spawnen als D-Class. Alle Türen werden geöffnet. Der erste der den Ausgang erreicht gewinnt.", "fl0w#1957")]
        public void OnEnabled()
        {
            Singleton = this;
            Handler = PluginHandler.Get(this);
        }
        public PluginHandler Handler;
        
        

        public string EventName { get; } = "Pathfinder Event";
        public string EvenAuthor { get; } = "fl0w#1957";

        public string EventDescription { get; set; } =
            "Alle spawnen als D-Class. Alle Türen werden geöffnet. Der erste der den Ausgang erreicht gewinnt.";
        public string EventPrefix { get; } = "pf";
        public bool BulletHolesAllowed { get; set; } = true;
        public IEventConfig Config => EventConfig;
        
        
        public void PrepareEvent()
        {
            Log.Info("Pathfinder Event is preparing");
            Log.Info("Pathfinder Event is prepared");
            EventManager.RegisterEvents<EventHandler>(this);
        }

        public void StopEvent()
        {
            Singleton = null;
            EventManager.UnregisterEvents<EventHandler>(this);
        }
    }
}