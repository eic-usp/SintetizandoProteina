using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class QuizOption : MonoBehaviour
{
    [SerializeField] private bool correct;
    public bool Correct => correct;
    private Button _button;
    private TextMeshProUGUI _text;
    private Image _image;
    private QuizManager _quizManager;

    private void Start()
    {
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _quizManager = GetComponentInParent<QuizManager>();
        _button.onClick.AddListener(Choose);
    }

    private void Choose()
    {
        _quizManager.Choose(correct);
        _image.color = (correct ? Color.green : Color.red);
        _button.interactable = false;
    }
}
