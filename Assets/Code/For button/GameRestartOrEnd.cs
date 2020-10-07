
using UnityEngine;
using UnityEngine.Events;


public class GameRestartOrEnd : MonoBehaviour
{
    [SerializeField] GameObject _buttonRestarter;
    [SerializeField] GameObject _buttonClosed;

    private ButtonClick _clickToRestartButton;
    private ButtonClick _clickToCloseButton;

    public event UnityAction GameRestart;
    public event UnityAction GameDone;

    private void Awake()
    {
        _clickToRestartButton = _buttonRestarter.GetComponent<ButtonClick>();
        _clickToCloseButton = _buttonClosed.GetComponent<ButtonClick>();
    }

    private void OnEnable()
    {
        _clickToRestartButton.Click += OnGameRun;
        _clickToCloseButton.Click += OnGameDone;
    }

    private void OnDisable()
    {
        _clickToRestartButton.Click -= OnGameRun;
        _clickToCloseButton.Click -= OnGameDone;
    }

    private void OnGameRun()
    {
        GameRestart?.Invoke();
    }

    private void OnGameDone()
    {
        GameDone?.Invoke();
    }
}
