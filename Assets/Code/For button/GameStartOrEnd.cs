
using UnityEngine;
using UnityEngine.Events;


public class GameStartOrEnd : MonoBehaviour
{
    [SerializeField] private GameObject buttonStart;
    [SerializeField] private GameObject buttonExit;

    private ButtonClick componentStart;
    private ButtonClick componentExit;

    public event UnityAction StartGame;
    public event UnityAction ExitGame;

    private void Awake()
    {
        componentStart = buttonStart.GetComponent<ButtonClick>();
        componentExit = buttonExit.GetComponent<ButtonClick>();
    }

    private void OnEnable()
    {
        componentStart.Click += OnStartGame;
        componentExit.Click += OnExitGame;
    }

    private void OnDisable()
    {
        componentStart.Click -= OnStartGame;
        componentExit.Click -= OnExitGame;
    }

    private void OnStartGame() { StartGame?.Invoke(); }

    private void OnExitGame() { ExitGame?.Invoke(); }
}
