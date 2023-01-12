﻿using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Video;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        [Space] [Header("Mixer settings")] [Space]
        
        [SerializeField] private AudioMixer mixer;
        [SerializeField] private AudioMixerGroup masterMixerGroup;
        [SerializeField] private AudioMixerGroup musicMixerGroup;
        [SerializeField] private AudioMixerGroup sfxMixerGroup;

        [Space] [Header("Video player settings")] [Space]
        
        [SerializeField] private VideoPlayer videoPlayer;
        
        [Space] [Header("Audio tracks")] [Space]
        
        [SerializeField] private Music[] musics;
        [SerializeField] private SoundEffect[] soundEffects;
        
        [System.Serializable]
        private struct SoundEffect
        {
            [field:SerializeField] public AudioClip AudioClip { get; set; }
            [field:SerializeField] public SoundEffectTrack SoundEffectTrack { get; set; }
        }
        
        [System.Serializable]
        private struct Music
        {
            [field:SerializeField] public AudioClip AudioClip { get; set; }
            [field:SerializeField] public MusicTrack MusicTrack { get; set; }
            [field:SerializeField] public bool Loop { get; set; }
        }

        public static AudioManager Instance;
        
        private AudioSource _musicSource;
        private AudioSource _sfxSource;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            DontDestroyOnLoad(gameObject);

            _sfxSource = gameObject.AddComponent<AudioSource>();
            _sfxSource.outputAudioMixerGroup = sfxMixerGroup;
            _sfxSource.playOnAwake = false;

            _musicSource = gameObject.AddComponent<AudioSource>();
            _musicSource.outputAudioMixerGroup = musicMixerGroup;
            _sfxSource.playOnAwake = false;
            
            videoPlayer.SetTargetAudioSource(0, _musicSource);
            
            // Play(MusicTrack.MainMenu);
        }

        public void Play(SoundEffectTrack soundEffectTrack)
        {
            var sfx = System.Array.Find(soundEffects, fx => fx.SoundEffectTrack == soundEffectTrack);
            _sfxSource.clip = sfx.AudioClip;
            
            if (sfx.AudioClip == null)
            {
                Debug.LogError($"Could not find SFX for {soundEffectTrack}");
                return;
            }
            
            _sfxSource.Play();
        }

        public void Play(MusicTrack musicTrack)
        {
            var music = System.Array.Find(musics, m => m.MusicTrack == musicTrack);
            _musicSource.clip = music.AudioClip;
            _musicSource.loop = music.Loop;
            
            if (music.AudioClip == null)
            {
                Debug.LogError($"Could not find music for {musicTrack}");
                return;
            }
            
            _musicSource.Play();
        }

        public void ToggleMuteSoundEffects() => _sfxSource.mute = !_sfxSource.mute;
        
        public void ToggleMuteMusic() => _musicSource.mute = !_musicSource.mute;

        public void PauseMusic() => _musicSource.Pause();
        
        public void UnPauseMusic() => _musicSource.UnPause();
        
        public void StopMusic() => _musicSource.Stop();
        
        public void ToggleMusic()
        {
            if (_musicSource.isPlaying)
            {
                _musicSource.Pause();
            }
            else
            {
                _musicSource.UnPause();
            }
        }
    }
}