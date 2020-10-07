
using UnityEngine;


[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class Bullet : MonoBehaviour
{
    // [SerializeField] private Camera _camera; // ??? Not set field. ???

    private Camera _camera;
    private Vector3 _screenMouse;
    private Vector3 _worldMouse;

    public float Speed { get; } = 7.0f;

    private void Awake()
    {
        _camera = FindObjectOfType<Camera>();

        _screenMouse = Input.mousePosition;
        _worldMouse = _camera.ScreenToWorldPoint(_screenMouse);
        _worldMouse.Set(_worldMouse.x, _worldMouse.y, 0);
        _worldMouse.Normalize();
    }

    private void Update()
    {
        this.transform.Translate(_worldMouse * Speed * Time.deltaTime);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;

        if (obj.TryGetComponent<Bot>(out Bot b))
        {
            Destroy(obj);
            Destroy(this.gameObject);
        }
    }
}
