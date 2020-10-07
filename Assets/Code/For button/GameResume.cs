
using UnityEngine;
using UnityEngine.Events;


public class GameResume : MonoBehaviour
{
    [SerializeField] GameObject _buttonResumer;

    private ButtonClick _clickToRestartButton;

    public event UnityAction ResumeGame;

    private void Awake()
    {
        _clickToRestartButton = _buttonResumer.GetComponent<ButtonClick>();
    }

    private void OnEnable()
    {
        _clickToRestartButton.Click += OnGameResume;
    }

    private void OnDisable()
    {
        _clickToRestartButton.Click -= OnGameResume;
    }

    private void OnGameResume()
    {
        ResumeGame?.Invoke();
    }
}
