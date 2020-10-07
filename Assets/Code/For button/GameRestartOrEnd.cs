
using UnityEngine;
using UnityEngine.Events;


public class GameRestartOrEnd : MonoBehaviour
{
    [SerializeField] GameObject buttonGameRestart;
    [SerializeField] GameObject buttonGameExit;

    private ButtonClick clickToRestartButton;
    private ButtonClick clickToExitButton;

    public event UnityAction RestartGame;
    public event UnityAction ExitGame;

    private void Awake()
    {
        clickToRestartButton = buttonGameRestart.GetComponent<ButtonClick>();
        clickToExitButton = buttonGameExit.GetComponent<ButtonClick>();
    }

    private void OnEnable()
    {
        clickToRestartButton.Click += OnRestartGame;
        clickToExitButton.Click += OnExitGame;
    }

    private void OnDisable()
    {
        clickToRestartButton.Click -= OnRestartGame;
        clickToExitButton.Click -= OnExitGame;
    }

    private void OnRestartGame()
    {
        RestartGame?.Invoke();
    }

    private void OnExitGame()
    {
        ExitGame?.Invoke();
    }
}
