
using UnityEngine;


[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class Bot : MonoBehaviour
{
    private GameObject playerObject;
    private float speed = 6.0f;
    private Vector3 direction;
    private Camera cameraObject;
    private Player player;

    Vector3 selfPosition;
    Vector3 targetPosition;

    private void Start()
    {
        cameraObject = FindObjectOfType<Camera>();

        playerObject = GameObject.Find("Player");
        player = playerObject.GetComponent<Player>();

        selfPosition = transform.position;
        targetPosition = playerObject.transform.position;
        selfPosition = cameraObject.ScreenToWorldPoint(selfPosition);
        targetPosition = cameraObject.ScreenToWorldPoint(targetPosition);

        direction = targetPosition - selfPosition;
    }

    private void Update()
    {
        if (IsPlayerNotDead())
        {
            MoveToPlayer();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);

        GameObject other = collision.gameObject;

        if (IsPlayer(other))
        {
            SetDamege();
        }
    }

    private bool IsPlayerNotDead()
    {
        return player.State != Player.DEAD;
    }

    private void MoveToPlayer()
    {
        Vector3 translation = direction * speed * Time.deltaTime;

        this.transform.Translate(translation, Space.World);
    }

    private bool IsPlayer(GameObject instance)
    {
        return instance.TryGetComponent<Player>(out Player p);
    }

    private void SetDamege()
    {
        player.Lives -= 1;
    }
}