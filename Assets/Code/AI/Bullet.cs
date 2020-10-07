
using UnityEngine;


[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class Bullet : MonoBehaviour
{
    // [SerializeField] private Camera _camera; // ??? Not set field. ???

    private Camera cameraObject;
    private Vector3 mouseScreenPosition;
    private Vector3 mouseWorldPosition;
    private Vector3 direction;

    public float Speed { get; } = 7.0f;

    private void Awake()
    {
        cameraObject = FindObjectOfType<Camera>();

        mouseScreenPosition = Input.mousePosition;
        mouseWorldPosition = cameraObject.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.Set(mouseWorldPosition.x, mouseWorldPosition.y, 0);
        mouseWorldPosition.Normalize();

        direction = mouseWorldPosition * Speed * Time.deltaTime;
    }

    private void Update()
    {
        Move();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;

        if (IsBot(other))
        {
            Destroy(other);
            Destroy(this.gameObject);
        }
    }

    private void Move()
    {
        this.transform.Translate(direction);
    }

    private bool IsBot(GameObject instance)
    {
        return instance.TryGetComponent<Bot>(out Bot b);
    }
}
