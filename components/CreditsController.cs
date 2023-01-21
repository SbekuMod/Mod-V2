using System;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.Video;

namespace SbekuMod.components
{
    public class CreditsController: MonoBehaviour
    {
        private VideoPlayer _videoPlayer;
        private GameObject _titleTop;
        private GameObject _titleBottom;
        private GameObject _titleMenu;
        private OWAudioSource _ambienceSource;
        private OWAudioSource _musicSource;
        private PostProcessingBehaviour _postProcessingBehaviour;

        public bool IsPlaying { 
            get
            {
                return _videoPlayer.isPlaying;
            } 
        }

        public void Initialize(VideoClip video)
        {
            _videoPlayer = gameObject.AddComponent<VideoPlayer>();
            _videoPlayer.clip = video;
            _videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
            _videoPlayer.playOnAwake = false;
            _videoPlayer.Stop();
            _titleTop = GameObject.Find("TitleCanvasHack/TitleLayoutGroup/OW_Logo_Anim/OW_Logo_Anim/OUTER");
            _titleBottom = GameObject.Find("TitleCanvasHack/TitleLayoutGroup/OW_Logo_Anim/OW_Logo_Anim/WILDS");
            _titleMenu = GameObject.Find("TitleCanvas");
            _postProcessingBehaviour = gameObject.GetComponent<PostProcessingBehaviour>();

            var musicObject = GameObject.Find("AudioSource_Music");
            var ambienceObject = GameObject.Find("AudioSource_Ambience");

            _ambienceSource = ambienceObject.GetComponent<OWAudioSource>();
            _musicSource = musicObject.GetComponent<OWAudioSource>();
        }

        public void Play()
        {
            var profile = StandaloneProfileManager.SharedInstance.currentProfile;
            if(profile != null )
                _videoPlayer.SetDirectAudioVolume(0, profile.settingsSave.musicVolume);
            
            _videoPlayer.Play();
        }

        public void Stop()
        {
            _videoPlayer.Stop();
        }

        //private void PlayVideo()
        //{
        //    _active = true;
        //    SbekuMod.Instance.ModHelper.Console.WriteLine($"PLAYING");
        //    Time.timeScale = 0;
        //    _titleMenu.SetActive(false);
        //    _titleTop.transform.localScale = Vector3.zero;
        //    _titleBottom.transform.localScale = Vector3.zero;
        //    _videoPlayer.Play();
        //}

        //private void StopVideo()
        //{
        //    _active = false;
        //    Time.timeScale = 1;
        //    _titleMenu.SetActive(true);
        //    _titleTop.transform.localScale = Vector3.one;
        //    _titleBottom.transform.localScale = Vector3.one;
        //    _videoPlayer.Stop();
        //}

        void Update()
        {

            if (_videoPlayer.isPlaying && Time.timeScale == 1) {
                Time.timeScale = 0;
                _titleMenu.SetActive(false);
                _postProcessingBehaviour.enabled = false;
                _titleTop.transform.localScale = Vector3.zero;
                _titleBottom.transform.localScale = Vector3.zero;
                _ambienceSource.Stop();
                _musicSource.Stop();
                return;
            }

            if (!_videoPlayer.isPlaying && Time.timeScale == 0)
            {
                Time.timeScale = 1;
                _titleMenu.SetActive(true);
                _postProcessingBehaviour.enabled = true;
                _titleTop.transform.localScale = Vector3.one;
                _titleBottom.transform.localScale = Vector3.one;
                _ambienceSource.Play();
                _musicSource.Play();
                _videoPlayer.Stop();
                return;
            }

        }

    }
}
