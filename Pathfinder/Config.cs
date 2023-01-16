using System.ComponentModel;
using CedMod.Addons.Events;

namespace Events.Pathfinder
{
    public sealed class Config: IEventConfig
    {
        [Description("Indicates whether the event is enabled or not")]
        public bool IsEnabled { get; set; } = true;
    }
}