using Newtonsoft.Json;
using SbekuMod.utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace SbekuMod.components
{
    class SubtitleColor
    {
        public int R;
        public int G;
        public int B;
    }
    class SubtitleData
    {
        public float Time = 0;
        public float Timeout = 0;
        public string Subtitle = "";
        public SubtitleColor color = new SubtitleColor();
    }

    public class VideoSubtitles: MonoBehaviour
    {

        private VideoPlayer _video;
        private GameObject _canvas;
        private Text _text;
        private SubtitleData[] subtitleData = new SubtitleData[0];
        IEnumerator UpdateSubtitleCoroutine()
        {
            while (true)
            {
                UpdateSubtitle();
                yield return new WaitForSeconds(.5f);
            }
        }

        public void Initialize(VideoPlayer videoPlayer, string jsonPath)
        {
            _video = videoPlayer;

            var subtitlesPath = Path.Combine(UtilityHelper.GetProjectBasePath(), "assets", jsonPath);

            if (!File.Exists(subtitlesPath))
            {
                SbekuMod.Instance.ModHelper.Console.WriteLine($"Subtitle File Missing: {subtitlesPath}", OWML.Common.MessageType.Warning);
                return;
            }

            var json = File.ReadAllText(subtitlesPath);

            subtitleData = JsonConvert.DeserializeObject<SubtitleData[]>(json);

            GameObject canvasPrefab = AssetLibrary.GetAsset<GameObject>("Assets/SubtitleCanvas.prefab");
            _canvas = Instantiate(canvasPrefab);
            _text = _canvas.GetComponentInChildren<Text>();
            _text.font = TextTranslation.GetFont(false);
            _text.alignment = TextAnchor.MiddleCenter;
            _text.fontStyle = FontStyle.Bold;
            _text.text = "";

            StartCoroutine(UpdateSubtitleCoroutine());
        }

        void UpdateSubtitle()
        {
            try
            {
                var currentTime = _video.clockTime;

                foreach (var subtitle in subtitleData)
                {

                    var startTime = subtitle.Time;
                    var endTime = subtitle.Time + subtitle.Timeout;

                    if (startTime <= currentTime && currentTime < endTime)
                    {
                        _text.text = subtitle.Subtitle;
                        _text.color = new Color(subtitle.color.R, subtitle.color.G, subtitle.color.B);
                        return;
                    }

                }

                _text.text = "";
            }catch(Exception e) {
                SbekuMod.Instance.ModHelper.Console.WriteLine($"Subtitles Error: {e.Message}", OWML.Common.MessageType.Error);
            }
        }

    }
}
