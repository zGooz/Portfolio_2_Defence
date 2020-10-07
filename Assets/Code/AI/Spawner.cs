
using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _main;
    [SerializeField] private GameObject _bot;
    [SerializeField] private GameObject _player;

    private GameProcess _mainComponent;
    private Player _playerComponent;
    private int _botAmount = 1;

    private const int LAST_WAVE = 10;

    public event UnityAction Winner;

    public void OnWinner()
    {
        Winner?.Invoke();
    }

    private void Awake()
    {
        _mainComponent = _main.GetComponent<GameProcess>();
        _playerComponent = _player.GetComponent<Player>();
    }

    private float _reload = 5.0f;

    private void OnEnable() 
    { 
        _mainComponent.GameStart += OnGameStart; 
    }

    private void OnDisable() 
    { 
        _mainComponent.GameStart -= OnGameStart; 
    }

    private void OnGameStart() 
    { 
        StartCoroutine(Spawn()); 
    }

    IEnumerator Spawn()
    {
        if (_playerComponent.State == Player.DEAD) 
        { 
            yield break; 
        }

        if (_botAmount == LAST_WAVE)
        {
            OnWinner();
            yield break;
        }

        for (int i = 0; i < _botAmount; i++)
        {
            float angle = Random.Range(0, 360) * Mathf.Deg2Rad;

            Vector3 statr = _player.transform.position;
            float x = statr.x + Mathf.Cos(angle) * _playerComponent.Radius;
            float y = statr.y + Mathf.Sin(angle) * _playerComponent.Radius;

            Vector3 finish = new Vector3(0, 0, 0);
            finish.Set(x, y, -10);

            var bot = Instantiate(_bot, finish, Quaternion.identity);
            bot.transform.Rotate(new Vector3(0, 0, angle * Mathf.Rad2Deg + 90));
        }

        _botAmount += 1;

        yield return new WaitForSeconds(_reload);

        StartCoroutine(Spawn());
    }
}
