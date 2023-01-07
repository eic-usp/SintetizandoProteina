using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    [System.Serializable]
    internal struct SoundEffect
    {
        [SerializeField] internal AudioClip audioClip;
        [SerializeField] internal SoundEffectTrack soundEffectTrack;
    }
        
    [System.Serializable]
    internal struct Music
    {
        [SerializeField] internal AudioClip audioClip;
        [SerializeField] internal MusicTrack musicTrack;
        [SerializeField] internal bool loop;
    }
    
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

            _musicSource = gameObject.AddComponent<AudioSource>();
            _musicSource.outputAudioMixerGroup = musicMixerGroup;
            
            // Play(MusicTrack.MainMenu);
        }

        public void Play(SoundEffectTrack soundEffectTrack)
        {
            var sfx = System.Array.Find(soundEffects, fx => fx.soundEffectTrack == soundEffectTrack);
            _sfxSource.clip = sfx.audioClip;
            
            if (sfx.audioClip == null)
            {
                Debug.LogError($"Could not find SFX for {soundEffectTrack}");
                return;
            }
            
            _sfxSource.Play();
        }

        public void Play(MusicTrack musicTrack)
        {
            var music = System.Array.Find(musics, m => m.musicTrack == musicTrack);
            _musicSource.clip = music.audioClip;
            _musicSource.loop = music.loop;
            
            if (music.audioClip == null)
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