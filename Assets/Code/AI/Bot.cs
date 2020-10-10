
using UnityEngine;


[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]

public class Bot : MonoBehaviour
{
    private GameObject player;
    private Camera cameraObject;
    private Rigidbody2D body;

    private float speed = 2.4f;
    private Vector3 direction;
    private Vector3 force;

    private void Start()
    {
        cameraObject = FindObjectOfType<Camera>();
        body = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        direction = GetDirectionToMove();
        force = GetForce(direction);
    }

    private void Update()
    {
        MoveToPlayer();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;

        if (IsBot(other))
        {
            return;
        }

        if (IsBullet(other))
        {
            Destroy(other);
        }

        Destroy(this.gameObject);
    }

    private bool IsBot(GameObject instance)
    {
        return instance.TryGetComponent<Bot>(out Bot b);
    }

    private void MoveToPlayer()
    {
        body.AddForce(force, ForceMode2D.Impulse);
    }

    private bool IsBullet(GameObject instance)
    {
        return instance.TryGetComponent<Bullet>(out Bullet b);
    }

    private Vector3 GetDirectionToMove()
    {
        Vector3 selfPosition = transform.position;
        Vector3 targetPosition = player.transform.position;

        selfPosition = cameraObject.ScreenToWorldPoint(selfPosition);
        targetPosition = cameraObject.ScreenToWorldPoint(targetPosition);

        Vector3 direction = targetPosition - selfPosition;
        direction.Set(direction.x, direction.y, 0);

        return direction;
    }

    private Vector3 GetForce(Vector3 direction)
    {
        return direction * speed * Time.deltaTime;
    }
}
