
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;


[RequireComponent(typeof(AudioSource))]

public class Game : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject liveBox;
    [SerializeField] private GameObject respawner;
    [SerializeField] private GameObject PrefabStarter;
    [SerializeField] private GameObject PrefabSuspense;
    [SerializeField] private GameObject PrefabRestaret;
    [SerializeField] private AudioClip soundRun;
    [SerializeField] private AudioClip soundLosing;
    [SerializeField] private AudioClip soundWinner;
    [SerializeField] private AudioSource audioNotification;
    [SerializeField] private AudioSource backgroundMusic;

    private GameObject starter;
    private GameObject suspenser;
    private GameObject restarter;
    private GameStartOrEnd componentGameStart;
    private GameRestartOrEnd componentGameEnd;
    private GameResume componentGamePause;
    private Spawner respawn;
    private Player player;
    private LifeBox lives;

    public GameObject GameStarter => starter;
    public GameObject GameResumer => suspenser;
    public GameObject GameEnderOrRestarter => restarter;

    public event UnityAction GameStart;
    public event UnityAction YouWinner;
    public event UnityAction YouLouse;

    public const int GAME = 0;
    public const int MENU = 1;
    public const int PAUSE = 2;
    public const int WINNER = 3;
    public const int LOSE = 4;

    public int GameState { private set; get; } = MENU;

    private void Awake()
    {
        createMenuBox(ref starter, ref PrefabStarter, ref componentGameStart);
        respawn = respawner.GetComponent<Spawner>();
        audioNotification = GetComponent<AudioSource>();
        player = playerObject.GetComponent<Player>();
        lives = liveBox.GetComponent<LifeBox>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameState == GAME)
            {
                GameState = PAUSE;
                createMenuBox(ref suspenser, ref PrefabSuspense, ref componentGamePause);
                componentGamePause.ResumeGame += OnGameResume;
            }
        }
    }

    private void OnEnable()
    {
        componentGameStart.StartGame += OnStartGame;
        componentGameStart.ExitGame += OnExitGame;
        respawn.Winner += OnWinnder;
        lives.Dead += OnLoose;
    }

    private void OnDisable()
    {
        respawn.Winner -= OnWinnder;
        lives.Dead -= OnLoose;

        if (componentGameEnd is GameRestartOrEnd)
        {
            componentGameEnd.RestartGame -= OnGameRestart;
            componentGameEnd.ExitGame -= OnExitGame;
        }

        if (componentGamePause is GameResume)
        {
            componentGamePause.ResumeGame -= OnGameResume;
        }
    }

    public void OnGameStart() { GameStart?.Invoke(); }

    private void OnGameResume()
    {
        if (GameState == MENU) { OnGameStart(); }
        GameState = GAME;
        Destroy(suspenser);
    }

    private void OnWinnder()
    {
        bool isBeforeLosed = GameState != LOSE;

        if (! isBeforeLosed)
        {
            GameState = WINNER;
            OnYouWinner();
        }

        OnYouWinner();
        createMenuBox(ref restarter, ref PrefabRestaret, ref componentGameEnd);
        componentGameEnd.RestartGame += OnGameRestart;
        componentGameEnd.ExitGame += OnExitGame;
        ChangeAndRunSound(audioNotification, soundWinner);
    }

    private void OnStartGame()
    {
        if (GameState == MENU) { OnGameStart(); }
        GameState = GAME;
        componentGameStart.StartGame -= OnStartGame;
        componentGameStart.ExitGame -= OnExitGame;
        Destroy(starter);
        ChangeAndRunSound(audioNotification, soundRun);
    }

    private void OnGameRestart()
    {
        string scene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
        GameState = MENU;
        OnStartGame();
    }

    private void OnLoose()
    {
        GameState = LOSE;
        // create Ender_Or_Restarter menu-box
        OnWinnder(); 
        OnYouLouse();
        ChangeAndRunSound(audioNotification, soundLosing);
    }

    private void OnExitGame()
    {
        Debug.Log("Application.Quit();");
        Application.Quit();
    }

    private void OnYouWinner() { YouWinner?.Invoke(); }
    private void OnYouLouse() { YouLouse?.Invoke(); }

    private void ChangeAndRunSound(AudioSource source, AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }

    private void createMenuBox<T>(ref GameObject box, ref GameObject prefab, ref T component)
    {
        box = Instantiate(prefab, canvas.transform);
        component = box.GetComponent<T>();
    }
}
