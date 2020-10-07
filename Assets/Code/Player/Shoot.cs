
using UnityEngine;


public class Shoot : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;

    private const int TIME_BETWEEN_SHOTS = 90;

    private Bullet bullet;
    private Player player;
    private int reload = TIME_BETWEEN_SHOTS;

    private void Awake()
    {
        bullet = _bulletPrefab.GetComponent<Bullet>();
        player = GetComponent<Player>();
    }
    
    private void Update()
    {
        if (! StateIsInGame())
        {
            return;
        }

        if (IsPlayerNotDead())
        {
            WaitPossibilityForShoot();

            if (Input.GetMouseButtonDown(0))
            {
                if (IsCanShoot())
                {
                    CreateBullet();
                    RunPossibility();
                }
            }
        }
    }

    private bool StateIsInGame()
    {
        return player.ManagerStateData.GameState == Game.GAME;
    }

    private bool IsPlayerNotDead()
    {
        return player.State != Player.DEAD;
    }

    private void WaitPossibilityForShoot()
    {
        reload = Mathf.Max(--reload, 0);
    }

    private bool IsCanShoot()
    {
        return reload == 0;
    }

    private void RunPossibility()
    {
        reload = TIME_BETWEEN_SHOTS;
    }

    private void CreateBullet()
    {
        var bullet = Instantiate(_bulletPrefab, this.transform.position, Quaternion.identity);

        Destroy(bullet.gameObject, player.Radius / this.bullet.Speed);
    }
}
