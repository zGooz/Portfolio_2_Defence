
using UnityEngine;


[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class Bot : MonoBehaviour
{
    private GameObject _player;
    private float _speed = 6.0f;
    private Vector3 _direction;
    private Camera _camera;
    private Player _playerComponent;

    Vector3 _selfPos;
    Vector3 _targetPos;

    private void Start()
    {
        _camera = FindObjectOfType<Camera>();

        _player = GameObject.Find("Player");
        _playerComponent = _player.GetComponent<Player>();

        _selfPos = transform.position;
        _targetPos = _player.transform.position;

        _selfPos = _camera.ScreenToWorldPoint(_selfPos);
        _targetPos = _camera.ScreenToWorldPoint(_targetPos);
        _direction = _targetPos - _selfPos;
    }

    private void Update()
    {
        if (_playerComponent.State != Player.DEAD)
        {
            this.transform.Translate(_direction * _speed * Time.deltaTime, Space.World);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);

        GameObject obj = collision.gameObject;

        if (obj.TryGetComponent<Player>(out Player p))
        {
            _playerComponent.Lives -= 1;
        }
    }
}