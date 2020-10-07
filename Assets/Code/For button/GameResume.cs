
using UnityEngine;
using UnityEngine.Events;


public class GameResume : MonoBehaviour
{
    [SerializeField] GameObject buttonGameResume;

    private ButtonClick clickToResumeButton;

    public event UnityAction ResumeGame;

    private void Awake()
    {
        clickToResumeButton = buttonGameResume.GetComponent<ButtonClick>();
    }

    private void OnEnable()
    {
        clickToResumeButton.Click += OnResumeGame;
    }

    private void OnDisable()
    {
        clickToResumeButton.Click -= OnResumeGame;
    }

    private void OnResumeGame()
    {
        ResumeGame?.Invoke();
    }
}
