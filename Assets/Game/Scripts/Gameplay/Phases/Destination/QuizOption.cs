using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Phases.Destination
{
    [RequireComponent(typeof(Button))]
    public class QuizOption : MonoBehaviour
    {
        public bool Correct { get; set; }
        public string Answer { get; set; }

        private TextMeshProUGUI _text;
        private Button _button;
        private Image _image;
        private QuizManager _quizManager;

        private void Start()
        {
            _button = GetComponent<Button>();
            _image = GetComponent<Image>();
            _text = GetComponentInChildren<TextMeshProUGUI>();
            _quizManager = GetComponentInParent<QuizManager>();
            _button.onClick.AddListener(Choose);
            _text.text = Answer;
        }

        private void Choose()
        {
            _image.color = _quizManager.Choose(Correct);
            _button.interactable = false;
        }
    }
}