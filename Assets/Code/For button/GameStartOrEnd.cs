
using UnityEngine;
using UnityEngine.Events;


public class GameStartOrEnd : MonoBehaviour
{
    [SerializeField] GameObject _buttonRuner;
    [SerializeField] GameObject _buttonClosed;

    private ButtonClick _clickToRunButton;
    private ButtonClick _clickToCloseButton;

    public event UnityAction GameRun;
    public event UnityAction GameDone;

    private void Awake()
    {
        _clickToRunButton = _buttonRuner.GetComponent<ButtonClick>();
        _clickToCloseButton = _buttonClosed.GetComponent<ButtonClick>();
    }

    private void OnEnable()
    {
        _clickToRunButton.Click += OnGameRun;
        _clickToCloseButton.Click += OnGameDone;
    }

    private void OnDisable()
    {
        _clickToRunButton.Click -= OnGameRun;
        _clickToCloseButton.Click -= OnGameDone;
    }

    private void OnGameRun()
    {
        GameRun?.Invoke();
    }

    private void OnGameDone()
    {
        GameDone?.Invoke();
    }
}
