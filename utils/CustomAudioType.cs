using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SbekuMod.utils
{
    public enum CustomAudioType
    {
        [EnumMember(Value = "CUSTOM_REEL_SLIDE_BG")]
        CUSTOM_REEL_SLIDE_BG = 1011,
        [EnumMember(Value = "SBK_SIGNALSCOPE")]
        SBK_SIGNALSCOPE = 1012,
        [EnumMember(Value = "TEST_ENDING")]
        TEST_ENDING = 1013,
        [EnumMember(Value = "DREAM")]
        DREAM = 1014,
        [EnumMember(Value = "BELLS")]
        BELLS = 1015,
        [EnumMember(Value = "CAMPFIRE")]
        CAMPFIRE = 1016,
        [EnumMember(Value = "ELK_KILL")]
        ELK_KILL = 1017,
        [EnumMember(Value = "SPLASH")]
        SPLASH = 1018,
        [EnumMember(Value = "WAKE")]
        WAKE = 1019,
        [EnumMember(Value = "BELLS_MUFFLED")]
        BELLS_MUFFLED = 1020,
        [EnumMember(Value = "CLOCK")]
        CLOCK = 1021,
        [EnumMember(Value = "CRUNCH")]
        CRUNCH = 1022,
        [EnumMember(Value = "THE_GRATE_FILTER")]
        THE_GRATE_FILTER = 1023,
        [EnumMember(Value = "BLOW")]
        BLOW = 1024,
        [EnumMember(Value = "GHOST_IN_THE_MACHINE")]
        GHOST_IN_THE_MACHINE = 1025,
        [EnumMember(Value = "GHOST_IN_THE_MACHINE_SIGNAL")]
        GHOST_IN_THE_MACHINE_SIGNAL = 1026,
        [EnumMember(Value = "SNM_ENDING")]
        SNM_ENDING = 1027,
        [EnumMember(Value = "SNM_MAIN_MENU")]
        SNM_MAIN_MENU = 1028,
    }
}
