
using UnityEngine;
using UnityEngine.Events;


public class GameRestartOrEnd : MonoBehaviour
{
    [SerializeField] private GameObject buttonRestart;
    [SerializeField] private GameObject buttonExit;

    private ButtonClick componentRestart;
    private ButtonClick componentExit;

    public event UnityAction RestartGame;
    public event UnityAction ExitGame;

    private void Awake()
    {
        componentRestart = buttonRestart.GetComponent<ButtonClick>();
        componentExit = buttonExit.GetComponent<ButtonClick>();
    }

    private void OnEnable()
    {
        componentRestart.Click += OnRestartGame;
        componentExit.Click += OnExitGame;
    }

    private void OnDisable()
    {
        componentRestart.Click -= OnRestartGame;
        componentExit.Click -= OnExitGame;
    }

    private void OnRestartGame() { RestartGame?.Invoke(); }

    private void OnExitGame() { ExitGame?.Invoke(); }
}
