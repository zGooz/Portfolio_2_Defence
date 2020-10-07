
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;


[RequireComponent(typeof(AudioSource))]

public class Game : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject respawner;

    [SerializeField] private GameObject gameStarterPrefab;
    [SerializeField] private GameObject gameSuspenserPrefab;
    [SerializeField] private GameObject gameRestareterPrefab;

    [SerializeField] private AudioClip gameRun;
    [SerializeField] private AudioClip gameLosing;
    [SerializeField] private AudioClip gameWinner;

    [SerializeField] private AudioSource audioNotification;
    [SerializeField] private AudioSource backgroundMusic;

    private GameObject gameStarter;
    private GameObject gameSuspenser;
    private GameObject gameRestarter;

    public GameObject GameStarter => gameStarter;
    public GameObject GameResumer => gameSuspenser;
    public GameObject GameEnderOrRestarter => gameRestarter;

    private GameStartOrEnd scriptGameStart;
    private GameRestartOrEnd scriptGameEnd;
    private GameResume scriptGamePause;

    private Spawner respawn;
    private Player player;

    public event UnityAction GameStart;

    public const int GAME = 0;
    public const int MENU = 1;
    public const int PAUSE = 2;
    public const int WINNER = 3;
    public const int LOSE = 4;

    public int GameState { private set; get; } = MENU;

    public void OnGameStart() 
    { 
        GameStart?.Invoke();
    }

    private void Awake()
    {
        createMenuBox(ref gameStarter, ref gameStarterPrefab, ref scriptGameStart);

        respawn = respawner.GetComponent<Spawner>();
        audioNotification = GetComponent<AudioSource>();
        player = playerObject.GetComponent<Player>();
    }

    private void OnEnable()
    {
        scriptGameStart.StartGame += OnGameRun;
        scriptGameStart.ExitGame += OnGameDone;
        respawn.Winner += OnWinnder;
        player.Dead += OnLoose;
    }

    private void OnDisable()
    {
        respawn.Winner -= OnWinnder;
        player.Dead -= OnLoose;

        if (scriptGameEnd is GameRestartOrEnd)
        {
            scriptGameEnd.RestartGame -= OnGameRestart;
            scriptGameEnd.ExitGame -= OnGameDone;
        }

        if (scriptGamePause is GameResume)
        {
            scriptGamePause.ResumeGame -= OnGameResume;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameState == GAME)
            {
                GameState = PAUSE;

                createMenuBox(ref gameSuspenser, ref gameSuspenserPrefab, ref scriptGamePause);

                scriptGamePause.ResumeGame += OnGameResume;
            }
        }
    }

    private void OnGameResume()
    {
        if (GameState == MENU)
        {
            OnGameStart();
        }

        GameState = GAME;
        Destroy(gameSuspenser);
    }

    private void OnWinnder()
    {
        GameState = WINNER;

        createMenuBox(ref gameRestarter, ref gameRestareterPrefab, ref scriptGameEnd);

        scriptGameEnd.RestartGame += OnGameRestart;
        scriptGameEnd.ExitGame += OnGameDone;

        ChangeAndRunSound(audioNotification, gameWinner);
    }

    private void OnGameRun()
    {
        if (GameState == MENU) 
        { 
            OnGameStart(); 
        }

        GameState = GAME;

        scriptGameStart.StartGame -= OnGameRun;
        scriptGameStart.ExitGame -= OnGameDone;

        Destroy(gameStarter);

        ChangeAndRunSound(audioNotification, gameRun);
    }

    private void OnGameRestart()
    {
        string scene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);

        GameState = MENU;

        OnGameRun();
    }

    private void OnLoose()
    {
        // create Ender_Or_Restarter menu-box
        OnWinnder(); 

        GameState = LOSE;

        ChangeAndRunSound(audioNotification, gameLosing);
    }

    private void OnGameDone()
    {
        Debug.Log("Application.Quit();");
        Application.Quit();
    }

    private void ChangeAndRunSound(AudioSource source, AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }

    private void createMenuBox<T>(ref GameObject box, ref GameObject prefab, ref T component)
    {
        box = Instantiate(prefab, _canvas.transform);
        component = box.GetComponent<T>();
    }
}
