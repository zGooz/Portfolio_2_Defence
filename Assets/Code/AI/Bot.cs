
using UnityEngine;


[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]

public class Bot : MonoBehaviour
{
    [SerializeField] 
    private GameObject bangPrefab;
    private GameObject player;
    private Camera viewer;
    private Rigidbody2D body;
    private float speed = 2.4f;
    private Vector3 direction;
    private Vector3 force;

    private void Start()
    {
        viewer = FindObjectOfType<Camera>();
        body = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        direction = GetDirectionToMove();
        force = GetForce(direction);
    }

    private void Update() { MoveToPlayer(); }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        bool isBot = other.TryGetComponent<Bot>(out Bot b);
        if (isBot) { return; }
        bool isBullet = other.TryGetComponent<Rocket>(out Rocket r);

        if (isBullet)
        {
            CreateBang();
            Destroy(other);
        }

        Destroy(this.gameObject);
    }

    private void MoveToPlayer() { body.AddForce(force, ForceMode2D.Impulse); }

    private void CreateBang()
    {
        Vector3 vector = this.transform.position;
        GameObject bang = Instantiate(bangPrefab, vector, Quaternion.identity);
        Destroy(bang, 1.0f);
    }

    private Vector3 GetDirectionToMove()
    {
        Vector3 selfPosition = transform.position;
        Vector3 targetPosition = player.transform.position;

        selfPosition = viewer.ScreenToWorldPoint(selfPosition);
        targetPosition = viewer.ScreenToWorldPoint(targetPosition);

        Vector3 direction = targetPosition - selfPosition;
        direction.Set(direction.x, direction.y, 0);

        return direction;
    }

    private Vector3 GetForce(Vector3 direction)
    {
        return direction * speed * Time.deltaTime;
    }
}
