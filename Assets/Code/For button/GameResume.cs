
using UnityEngine;
using UnityEngine.Events;


public class GameResume : MonoBehaviour
{
    [SerializeField] 
    private GameObject buttonResume;
    private ButtonClick componentResume;
    public event UnityAction ResumeGame;

    private void Awake()
    {
        componentResume = buttonResume.GetComponent<ButtonClick>();
    }

    private void OnEnable() { componentResume.Click += OnResumeGame; }

    private void OnDisable() { componentResume.Click -= OnResumeGame; }

    private void OnResumeGame() { ResumeGame?.Invoke(); }
}
