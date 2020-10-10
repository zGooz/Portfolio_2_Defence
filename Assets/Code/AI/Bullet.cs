
using UnityEngine;


[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class Bullet : MonoBehaviour
{
    // [SerializeField] private Camera _camera; // ??? Not set field. ???

    private Camera cameraObject;
    private Vector3 mouseScreenPosition;
    private Vector3 mouseWorldPosition;
    private Vector3 force;
    private Rigidbody2D body;

    public float Speed { get; } = 22.0f;

    private void Awake()
    {
        cameraObject = FindObjectOfType<Camera>();

        mouseScreenPosition = Input.mousePosition;
        mouseWorldPosition = cameraObject.ScreenToWorldPoint(mouseScreenPosition);

        this.transform.LookAt(new Vector3(0, 0, mouseWorldPosition.z), mouseWorldPosition);

        mouseWorldPosition.Set(mouseWorldPosition.x, mouseWorldPosition.y, 0);
        mouseWorldPosition.Normalize();

        force = mouseWorldPosition * Speed * Time.deltaTime;
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        body.AddForce(force, ForceMode2D.Impulse);
    }
}
