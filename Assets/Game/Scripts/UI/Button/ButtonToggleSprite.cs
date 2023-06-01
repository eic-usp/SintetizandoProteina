using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonToggleSprite : MonoBehaviour
{
    [SerializeField] private GameObject stateOnObject;
    [SerializeField] private GameObject stateOffObject;
    [SerializeField] private bool startOn;

    public enum State
    {
        Off,
        On
    };

    public State CurrentState
    {
        get => _currentState;
        private set
        {
            _currentState = value;
            stateOnObject.SetActive(value == State.On);
            stateOffObject.SetActive(value == State.Off);
        }
    }

    private State _currentState;
    private Button _button;
    
    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(Toggle);
        CurrentState = startOn ? State.On : State.Off;
    }

    private void Toggle() => CurrentState = (CurrentState == State.On) ? State.Off : State.On;
}
