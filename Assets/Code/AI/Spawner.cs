
using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject main;
    [SerializeField] private GameObject bot;
    [SerializeField] private GameObject playerObject;

    private Game game;
    private Player player;

    private int respownCounter = 1;
    private int currentWave = 1;
    private float reloadBetweenRespawn = 5.0f;

    private const int LAST_WAVE = 10;

    public event UnityAction Winner;

    public void OnWinner()
    {
        Winner?.Invoke();
    }

    private void Awake()
    {
        game = main.GetComponent<Game>();
        player = playerObject.GetComponent<Player>();
    }

    IEnumerator Spawn()
    {
        if (IsPlayerDead()) 
        { 
            yield break; 
        }

        if (IsLastWave())
        {
            OnWinner();
            yield break;
        }

        MakeBots();

        yield return new WaitForSeconds(reloadBetweenRespawn);

        StartCoroutine(Spawn());
    }

    private bool IsPlayerDead()
    {
        return player.State == Player.DEAD;
    }

    private bool IsLastWave()
    {
        return currentWave == LAST_WAVE;
    }

    private void MakeBots()
    {
        for (int i = 0; i < respownCounter; i++)
        {
            float angle = Random.Range(0, 360) * Mathf.Deg2Rad;
            Vector3 finish = GetRespawnPosition(angle);

            CreateBot(finish, angle);
        }

        respownCounter += 1;
        currentWave += 1;
    }

    private Vector3 GetRespawnPosition(float angle)
    {
        Vector3 statr = playerObject.transform.position;
        float x = statr.x + Mathf.Cos(angle) * player.Radius;
        float y = statr.y + Mathf.Sin(angle) * player.Radius;

        Vector3 finish = new Vector3(0, 0, 0);
        finish.Set(x, y, 0);

        return finish;
    }

    private void CreateBot(Vector3 finish, float angle)
    {
        var instance = Instantiate(bot, finish, Quaternion.identity);
        instance.transform.Rotate(new Vector3(0, 0, angle * Mathf.Rad2Deg + 90));
    }

    private void OnEnable()
    {
        game.GameStart += OnGameStart;
    }

    private void OnDisable()
    {
        game.GameStart -= OnGameStart;
    }

    private void OnGameStart()
    {
        StartCoroutine(Spawn());
    }
}
