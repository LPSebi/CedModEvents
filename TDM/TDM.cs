using CedMod.Addons.Events;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using EventManager = PluginAPI.Events.EventManager;

namespace Events.TDM
{
    public class TDM : IEvent
    {
        [PluginConfig] public Config EventConfig;

        public EventHandler Handler;

        public static TDM Singleton { get; private set; }


        public string EventName { get; } = "TDM Event";
        public string EvenAuthor { get; } = "fl0w#1957";

        public string EventDescription { get; set; } =
            "50 % of the players are D-Class and the other 50 % are Scientists. They have to kill each other. The last team standing wins.";

        public string EventPrefix { get; } = "tdm";
        public bool BulletHolesAllowed { get; set; } = true;
        public IEventConfig Config => EventConfig;


        public void PrepareEvent()
        {
            Log.Info("TDM Event is preparing");
            Singleton = this;
            Log.Info("TDM Event is prepared");
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

        [PluginEntryPoint("TDM Event", "0.1.0",
            "50 % of the players are D-Class and the other 50 % are Scientists. They have to kill each other. The last team standing wins.",
            "fl0w#1957")]
        public void OnEnabled()
        {
            Singleton = this;
            Handler = new EventHandler();
        }
    }
}