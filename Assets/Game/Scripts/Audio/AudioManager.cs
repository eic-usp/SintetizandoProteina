using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
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
            [field:SerializeField] public GameSceneManagement.Loader.Scene Scene { get; set; }
            [field:SerializeField] public bool Loop { get; set; }
        }

        public static AudioManager Instance { get; private set; }
        
        private Dictionary<GameSceneManagement.Loader.Scene, MusicTrack> _sceneMusics;
        private AudioSource _musicSource;
        private AudioSource _sfxSource;
        private VideoPlayer _videoPlayer;

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnLoadMenu;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnLoadMenu;
        }

        private void OnLoadMenu(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (scene.buildIndex != (int) GameSceneManagement.Loader.Scene.UIBeg) return;
            _videoPlayer = FindObjectOfType<VideoPlayer>();
            _videoPlayer.SetTargetAudioSource(0, _musicSource);
        }

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
            _musicSource.playOnAwake = false;

            SetupSceneMusics();
            Play((GameSceneManagement.Loader.Scene) SceneManager.GetActiveScene().buildIndex);
        }

        private void SetupSceneMusics()
        {
            _sceneMusics = new Dictionary<GameSceneManagement.Loader.Scene, MusicTrack>();

            foreach (var m in musics)
            {
                if (m.Scene == GameSceneManagement.Loader.Scene.None) continue;
                _sceneMusics[m.Scene] = m.MusicTrack;
                // Debug.Log($"Adding {m.MusicTrack} as music track for scene {m.Scene}");
            }
        }

        public void Play(SoundEffectTrack soundEffectTrack, bool oneShot = false, float oneShotVolumeScale = 1f)
        {
            var sfx = System.Array.Find(soundEffects, fx => fx.SoundEffectTrack == soundEffectTrack);

            if (oneShot)
            {
                _sfxSource.PlayOneShot(sfx.AudioClip, oneShotVolumeScale);
                return;
            }
            
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

        public void Play(GameSceneManagement.Loader.Scene scene)
        {
            if (_sceneMusics.TryGetValue(scene, out var musicTrack))
            {
                Play(musicTrack);
                return;
            }
            
            Debug.LogError($"Could not find music track for scene {scene}");
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