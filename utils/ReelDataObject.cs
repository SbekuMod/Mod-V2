using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SbekuMod.components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using static SbekuMod.patches.AudioSignalPatch;

namespace SbekuMod.utils
{
    public enum ReelType
    {
        [EnumMember(Value = "SLIDE")]
        SLIDE,
        [EnumMember(Value = "PROJECTION")]
        PROJECTION
    }

    public class PointData
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }

    public class SlideDataObject
    {
        public string Path { get; set; }
        public float Duration { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public CustomAudioType BackdropAudio { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public CustomAudioType BeatAudio { get; set; }
    }

    public class ReelDataObject
    {

        public string Name { get; set; }
        public string Prefab { get; set; }
        public string Parent { get; set; }
        public string? Sector { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public CustomSignalName? Signal;
        [JsonConverter(typeof(StringEnumConverter))]
        public CustomAudioType? SignalAudio { get; set; }
        public PointData InitialRotation { get; set; }
        public PointData InitialPosition { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ReelType Type { get; set; }

        public SlideDataObject[] Slides;

    }
}
