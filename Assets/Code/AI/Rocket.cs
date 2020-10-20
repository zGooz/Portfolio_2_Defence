
using System.Collections;
using UnityEngine;


[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class Rocket : MonoBehaviour
{
    // [SerializeField] private Camera _camera; // ??? Not set field. ???

    [SerializeField] GameObject tracePrefab;

    private Camera cameraObject;
    private Vector3 force;
    private Rigidbody2D body;
    private Vector3 direction;

    public float Speed { get; } = 22.0f;

    private void Awake()
    {
        cameraObject = FindObjectOfType<Camera>();
        body = GetComponent<Rigidbody2D>();
        direction = GetDirection();
        force = GetForce(direction);

        StartCoroutine(CreateTrace());
    }

    private void Update()
    {
        Move();
    }

    IEnumerator CreateTrace()
    {
        yield return new WaitForSeconds(0.08f);

        CreateRocketTrace();
        StartCoroutine(CreateTrace());
    }

    private void Move()
    {
        body.AddForce(force, ForceMode2D.Impulse);
    }

    private void CreateRocketTrace()
    {
        var trace = Instantiate(tracePrefab, this.transform.position, Quaternion.identity);

        Destroy(trace, 1.0f);
    }

    private Vector3 GetDirection()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector3 mouseWorldPosition = cameraObject.ScreenToWorldPoint(mouseScreenPosition);

        this.transform.LookAt(new Vector3(0, 0, mouseWorldPosition.z), mouseWorldPosition);

        mouseWorldPosition.Set(mouseWorldPosition.x, mouseWorldPosition.y, 0);
        mouseWorldPosition.Normalize();

        return mouseWorldPosition;
    }

    private Vector3 GetForce(Vector3 direction)
    {
        return direction * Speed * Time.deltaTime;
    }
}
