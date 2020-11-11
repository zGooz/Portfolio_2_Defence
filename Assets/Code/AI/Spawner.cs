
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
    private int botAmount = 1;
    private int wave = 1;
    private float reload = 5.0f;
    private const int LAST_WAVE = 10;

    public event UnityAction Winner;

    private void Awake()
    {
        game = main.GetComponent<Game>();
        player = playerObject.GetComponent<Player>();
    }

    private void OnEnable() { game.GameStart += OnGameStart; }

    private void OnDisable() { game.GameStart -= OnGameStart; }

    IEnumerator Spawn()
    {
        bool isPlayerDead = player.State == Player.DEAD;
        if (isPlayerDead) { yield break; }
        bool isLastWave = wave == LAST_WAVE;

        if (isLastWave)
        {
            OnWinner();
            yield break;
        }

        MakeBots();
        yield return new WaitForSeconds(reload);
        StartCoroutine(Spawn());
    }

    private void OnGameStart() { StartCoroutine(Spawn()); }

    public void OnWinner() { Winner?.Invoke(); }

    private void MakeBots()
    {
        for (int i = 0; i < botAmount; i++)
        {
            float angle = Random.Range(0, 360) * Mathf.Deg2Rad;
            Vector3 finish = GetRespawnPosition(angle);
            CreateBot(finish, angle);
        }

        botAmount += 1;
        wave += 1;
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
}
