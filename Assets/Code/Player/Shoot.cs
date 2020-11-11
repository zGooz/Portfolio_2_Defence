
using UnityEngine;


public class Shoot : MonoBehaviour
{
    [SerializeField] 
    private GameObject _bulletPrefab;
    private const int TIME_BETWEEN_SHOTS = 30;
    private Camera viewer;
    private Rocket rocket;
    private Player player;
    private float radius = 3.2f;
    private int reload = TIME_BETWEEN_SHOTS;

    private void Awake()
    {
        viewer = FindObjectOfType<Camera>();
        rocket = _bulletPrefab.GetComponent<Rocket>();
        player = GetComponent<Player>();
    }
    
    private void Update()
    {
        bool stateIsInGame = player.ManagerStateData.GameState == Game.GAME;
        if (! stateIsInGame) { return; }
        bool isPlayerLive = player.State != Player.DEAD;
        if (! isPlayerLive) { return; }
        WaitPossibilityForShoot();
        bool isMousePress = Input.GetMouseButtonDown(0);

        if (isMousePress)
        {
            bool isCanShoot = reload == 0;

            if (isCanShoot)
            {
                CreateBullet();
                RunPossibility();
            }
        }
    }

    private void WaitPossibilityForShoot() { reload = Mathf.Max(--reload, 0); }
    private void RunPossibility() { reload = TIME_BETWEEN_SHOTS; }

    private void CreateBullet()
    {
        Vector3 position = getPlaceToCreate();
        CreateBullet(position);
    }

    private Vector3 getPlaceToCreate()
    {
        Vector3 position = this.transform.position;
        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector3 mouseWorldPosition = viewer.ScreenToWorldPoint(mouseScreenPosition);

        mouseWorldPosition.Set(mouseWorldPosition.x, mouseWorldPosition.y, 0);
        mouseWorldPosition.Normalize();

        position.Set (
            position.x + mouseWorldPosition.x * radius,
            position.y + mouseWorldPosition.y * radius,
            0
        );

        return position;
    }

    private void CreateBullet(Vector3 position)
    {
        var bullet = Instantiate(_bulletPrefab, position, Quaternion.identity);
        Destroy(bullet.gameObject, (player.Radius / this.rocket.Speed) + 3.0f);
    }
}
