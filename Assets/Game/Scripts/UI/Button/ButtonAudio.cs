using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Button
{
    using Button = UnityEngine.UI.Button;
    
    public class ButtonAudio : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
    {
        [SerializeField] private bool hover;
        [SerializeField] private bool click;
        
        private bool IsInteractable
        {
            get
            {
                if (!_button) return true;
                if (_button && !_button.interactable) return false;
                return !_canvasGroup || _canvasGroup.interactable;
            }
        }
        
        private Button _button;
        private CanvasGroup _canvasGroup;
        
        private void Start()
        {
            _button = GetComponent<Button>();
            _canvasGroup = GetComponentInParent<CanvasGroup>();
        }

        public void OnPointerEnter(PointerEventData pointerEventData)
        {
            if (!hover || !IsInteractable) return;
            Audio.AudioManager.Instance.Play(Audio.SoundEffectTrack.ButtonHover);
        }
        
        public void OnPointerClick(PointerEventData pointerEventData)
        {
            if (!click) return;

            var sfx = IsInteractable
                ? Audio.SoundEffectTrack.ButtonClick
                : Audio.SoundEffectTrack.ButtonDisabled;

            Audio.AudioManager.Instance.Play(sfx);
        }
    }
}