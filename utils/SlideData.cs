using SbekuMod.patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SbekuMod.utils
{
    public class SlideData
    {

        public Texture2D slide;
        public float duration = -1;
        public AudioType backdropType = (AudioType) (-1);
        public AudioType beatType = (AudioType)(-1);

        public static SlideData CreateSlideData(Texture2D slide, float? duration, CustomAudioType? backdropAudio, CustomAudioType? beatAudio)
        {
            return new SlideData
            {
                slide = slide,
                duration = duration ?? -1,
                backdropType = (AudioType)(backdropAudio.HasValue ? backdropAudio : ((CustomAudioType)(-1))),
                beatType = (AudioType)(beatAudio.HasValue ? beatAudio : ((CustomAudioType)(-1)))
            };
        }

        public static SlideData CreateSlideData(string slidePath, float? duration, CustomAudioType? backdropAudio, CustomAudioType? beatAudio)
        {
            return CreateSlideData(AssetLibrary.GetAsset<Texture2D>(slidePath), duration, backdropAudio, beatAudio);
        }

        public Slide ToSlide()
        {
            var slideInstance = Slide.CreateSlide(slide);
            SbekuMod.Instance.ModHelper.Console.WriteLine($"SLIDE DURATION {duration}, BackdropType {backdropType} BeatAudio {beatType}");

            if (duration >= 0)
            {
                var playTimeModule = new SlidePlayTimeModule
                {
                    _duration = duration
                };

                Slide.WriteModuleToData(ref slideInstance._modulesList, ref slideInstance.lengths, ref slideInstance._modulesData, playTimeModule, overwrite: true);
            }

            if (backdropType != AudioType.None)
            {
                var audioModule = new SlideBackdropAudioModule
                {
                    _audioType = backdropType
                };

                Slide.WriteModuleToData(ref slideInstance._modulesList, ref slideInstance.lengths, ref slideInstance._modulesData, audioModule, overwrite: true);
            }

            if (beatType != AudioType.None)
            {
                var audioModule = new SlideBeatAudioModule
                {
                    _audioType = beatType
                };

                Slide.WriteModuleToData(ref slideInstance._modulesList, ref slideInstance.lengths, ref slideInstance._modulesData, audioModule, overwrite: true);
            }

            return slideInstance;
        }

        public static Slide[] DataToSlideList(SlideData[] slideDataList)
        {
            var slideList = new Slide[slideDataList.Length];
            int index = 0;

            foreach (var slideData in slideDataList)
            {
                slideList[index] = slideData.ToSlide();
                index++;
            }

            return slideList;
        }

    }
}
