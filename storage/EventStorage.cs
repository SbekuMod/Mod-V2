

using SbekuMod.utils;

namespace SbekuMod.storage
{
    public class EventData
    {
        public bool HasSeenWelcomeScreen { get; set; } = false;
        public bool HasSeenEnding { get; set; } = false;
        public bool HasSeenBeginning { get; set; } = false;
        public bool HasUnlockedEnding { get; set; } = false;
        public string[] SeenReels { get; set; } = new string[0];

    }

    public class EventStorage : StorageHandler<EventData>
    {

        protected override string GetFilename() => "snm_events.json";

    }
}
