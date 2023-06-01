using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using Cysharp.Threading.Tasks;

using UI.Animation;

/*
    This code pick one VideoChoice and set a videoPlayer, its also set some video options
    Like pause, play and skip.
    The video is linked with a subtitle
*/
namespace UI.Protein
{
    using Protein = UI.Protein.Info.Protein;

    public class VideoChoice : MonoBehaviour
    {
        [SerializeField] private string[] videoClips;
        [SerializeField] private Transform screens;

        [SerializeField] TransitionController myTransition;

        private VideoPlayer videoPlayer;
        private int actualVideoClip;
        
        private bool pause;

        private void Start()
        {
            Protein.Setup(this);
            videoPlayer = GetComponent<VideoPlayer>();
        }

        public void ChooseProtein(int index)
        {
            videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, videoClips[index]);
            actualVideoClip = index;
            transform.GetChild(0).gameObject.SetActive(true); //The buttons
            PlayVideo();
        }

        private IEnumerator FinishCheck()
        {
            while (videoPlayer.isPlaying)
            {
                yield return null;
            }

            if (!pause)
            {
                ShowScreen();
            }
        }

        private async void ShowScreen()
        {
            gameObject.SetActive(false); // Instantaneously stop all coroutines
            await PlayTransitionIn(); // Don't get finished in SetActive(false)
        }

        private async UniTask PlayTransitionIn()
        {
            myTransition.EnableTransition();

            screens.GetChild(actualVideoClip).gameObject.SetActive(true);
            await UniTask.Delay(myTransition.PlayTransitionFadeIn());
            
            myTransition.DisableTransition(); //Just to be sure, not needed
        }   

        public void StopVideo()
        {
            if (!videoPlayer.isPlaying) return;
            pause = true;

            StopCoroutine(FinishCheck());
            videoPlayer.Pause();
        }

        public void PlayVideo()
        {
            if (videoPlayer.isPlaying) return;
            pause = false;

            videoPlayer.Play();
            StartCoroutine(FinishCheck());
        }

        public void SkipVideo()
        {
            ShowScreen();
        }
    }
}