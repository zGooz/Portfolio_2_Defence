
using UnityEngine;
using UnityEngine.Events;


public class GameStartOrEnd : MonoBehaviour
{
    [SerializeField] GameObject buttonGameStart;
    [SerializeField] GameObject buttonGameExit;

    private ButtonClick clickToStartButton;
    private ButtonClick clickToExitButton;

    public event UnityAction StartGame;
    public event UnityAction ExitGame;

    private void Awake()
    {
        clickToStartButton = buttonGameStart.GetComponent<ButtonClick>();
        clickToExitButton = buttonGameExit.GetComponent<ButtonClick>();
    }

    private void OnEnable()
    {
        clickToStartButton.Click += OnStartGame;
        clickToExitButton.Click += OnExitGame;
    }

    private void OnDisable()
    {
        clickToStartButton.Click -= OnStartGame;
        clickToExitButton.Click -= OnExitGame;
    }

    private void OnStartGame()
    {
        StartGame?.Invoke();
    }

    private void OnExitGame()
    {
        ExitGame?.Invoke();
    }
}
