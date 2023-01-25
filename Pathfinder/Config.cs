using System.ComponentModel;
using CedMod.Addons.Events;

namespace Events.Pathfinder
{
    public sealed class Config: IEventConfig
    {
        [Description("Indicates whether the event is enabled or not")]
        public bool IsEnabled { get; set; } = true;

        [Description("The text displayer when a player has won the event")]
        public string WinText { get; set; } = $"<color=red>{0}</color> hat dieses Event gewonnen!";

        [Description("The text used in the plugin description")]
        public string DescText { get; set; } =
            "Alle spawnen als D-Class. Alle Türen werden geöffnet. Der erste der den Ausgang erreicht gewinnt."; //todo
    }
}