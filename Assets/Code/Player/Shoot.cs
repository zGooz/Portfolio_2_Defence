
using UnityEngine;


public class Shoot : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;

    private const int TIME_BETWEEN_SHOTS = 30;

    private Camera cameraObject;
    private Rocket bullet;
    private Player player;
    private float createAreaRadius = 3.2f;
    private int reload = TIME_BETWEEN_SHOTS;

    private void Awake()
    {
        cameraObject = FindObjectOfType<Camera>();
        bullet = _bulletPrefab.GetComponent<Rocket>();
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
        Vector3 position = getPlaceToCreate();

        CreateBullet(position);
    }

    private Vector3 getPlaceToCreate()
    {
        Vector3 position = this.transform.position;
        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector3 mouseWorldPosition = cameraObject.ScreenToWorldPoint(mouseScreenPosition);

        mouseWorldPosition.Set(mouseWorldPosition.x, mouseWorldPosition.y, 0);
        mouseWorldPosition.Normalize();

        position.Set (
            position.x + mouseWorldPosition.x * createAreaRadius,
            position.y + mouseWorldPosition.y * createAreaRadius,
            0
        );

        return position;
    }

    private void CreateBullet(Vector3 position)
    {
        var bullet = Instantiate(_bulletPrefab, position, Quaternion.identity);

        Destroy(bullet.gameObject, (player.Radius / this.bullet.Speed) + 3.0f);
    }
}
