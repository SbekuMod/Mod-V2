using System.Runtime.Serialization;

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
        [EnumMember(Value = "SNM_BEGINNING")]
        SNM_BEGINNING = 1029,
        //SIGNALSCOPE INDIVIDUAL SIGNALS
        [EnumMember(Value = "SIGNALSCOPE_BANJO_1")]
        SIGNALSCOPE_BANJO_1 = 1030,
        [EnumMember(Value = "SIGNALSCOPE_BANJO_2")]
        SIGNALSCOPE_BANJO_2 = 1031,
        [EnumMember(Value = "SIGNALSCOPE_BOUZOUKI_1")]
        SIGNALSCOPE_BOUZOUKI_1 = 1032,
        [EnumMember(Value = "SIGNALSCOPE_BOW_BOUZOUKI")]
        SIGNALSCOPE_BOW_BOUZOUKI = 1033,
        [EnumMember(Value = "SIGNALSCOPE_BOWS")]
        SIGNALSCOPE_BOWS = 1034,
        [EnumMember(Value = "SIGNALSCOPE_CONTRABASS")]
        SIGNALSCOPE_CONTRABASS = 1035,
        [EnumMember(Value = "SIGNALSCOPE_GUITAR_1")]
        SIGNALSCOPE_GUITAR_1 = 1036,
        [EnumMember(Value = "SIGNALSCOPE_GUITAR_2")]
        SIGNALSCOPE_GUITAR_2 = 1037,
        [EnumMember(Value = "SIGNALSCOPE_GUITAR_3")]
        SIGNALSCOPE_GUITAR_3 = 1038,
        [EnumMember(Value = "SIGNALSCOPE_GUITAR_MELODY")]
        SIGNALSCOPE_GUITAR_MELODY = 1039,
        [EnumMember(Value = "SIGNALSCOPE_PERCUSSIONS")]
        SIGNALSCOPE_PERCUSSIONS = 1040,
        [EnumMember(Value = "SIGNALSCOPE_RHODES")]
        SIGNALSCOPE_RHODES = 1041,
        [EnumMember(Value = "SIGNALSCOPE_WOODWINDS")]
        SIGNALSCOPE_WOODWINDS = 1042,
        //
    }
}
