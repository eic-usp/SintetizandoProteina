using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using Cysharp.Threading.Tasks;

using UI.Animation;

/*
    This code pick one VideoController and set a videoPlayer, its also set some video options
    Like pause, play and skip.
    The video is linked with a subtitle
*/
namespace UI.Protein
{
    using Protein = UI.Protein.Info.Protein;

    public class VideoController : MonoBehaviour
    {
        [SerializeField] private string[] videoClips;
        [SerializeField] private GameObject controls;
        [SerializeField] private Transform screens;

        [SerializeField] private TransitionController myTransition;

        private VideoPlayer _videoPlayer;
        private int _actualVideoClip;
        private bool _pause;

        private void Start()
        {
            Protein.Setup(this);
            _videoPlayer = GetComponent<VideoPlayer>();
        }

        public void ChooseProtein(int index)
        {
            _videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, videoClips[index]);
            _actualVideoClip = index;
            controls.gameObject.SetActive(true);
            PlayVideo();
        }

        private IEnumerator FinishCheck()
        {
            yield return new WaitWhile(() => _videoPlayer.isPlaying);

            if (!_pause)
            {
                ShowScreen();
            }
        }

        private async void ShowScreen()
        {
            controls.SetActive(false);
            gameObject.SetActive(false); // Instantaneously stop all coroutines
            await PlayTransitionIn(); // Don't get finished in SetActive(false)
        }

        private async UniTask PlayTransitionIn()
        {
            myTransition.EnableTransition();

            screens.GetChild(_actualVideoClip).gameObject.SetActive(true);
            await UniTask.Delay(myTransition.PlayTransitionFadeIn());
            
            myTransition.DisableTransition(); //Just to be sure, not needed
        }   

        public void PauseVideo()
        {
            if (!_videoPlayer.isPlaying) return;
            _pause = true;
            StopCoroutine(FinishCheck());
            _videoPlayer.Pause();
        }

        public void PlayVideo()
        {
            if (_videoPlayer.isPlaying) return;
            _pause = false;
            _videoPlayer.Play();
            StartCoroutine(FinishCheck());
        }

        public void ToggleVideo()
        {
            if (_pause)
            {
                PlayVideo();
            }
            else
            {
                PauseVideo();
            }
        }

        public void SkipVideo()
        {
            ShowScreen();
        }
    }
}