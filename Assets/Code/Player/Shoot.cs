
using UnityEngine;


public class Shoot : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;

    private const int TIME_BETWEEN_SHOTS = 90;

    private Bullet _bulletComponent;
    private Player _playerComponent;
    private int _timeBetweenShots = TIME_BETWEEN_SHOTS;

    private void Awake()
    {
        _bulletComponent = _bulletPrefab.GetComponent<Bullet>();
        _playerComponent = GetComponent<Player>();
    }
    
    private void Update()
    {
        if (_playerComponent.ManagerStateData.GameState == GameProcess.GAME)
        {
            if (_playerComponent.State != Player.DEAD)
            {
                _timeBetweenShots = Mathf.Max(--_timeBetweenShots, 0);

                if (Input.GetMouseButtonDown(0))
                {
                    if (_timeBetweenShots == 0)
                    {
                        var bullet = Instantiate(_bulletPrefab, this.transform.position, Quaternion.identity);

                        Destroy(bullet.gameObject, _playerComponent.Radius / _bulletComponent.Speed);

                        _timeBetweenShots = TIME_BETWEEN_SHOTS;
                    }
                }
            }
        }
    }
}
