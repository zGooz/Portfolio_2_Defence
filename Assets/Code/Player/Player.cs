
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject main;
 
    public const int LIVE = 0;
    public const int DEAD = 1;
    public int State { get; set; } = LIVE;
    public float Radius { get; } = 18.0f;

    public int liveAmount = 5;
    private Game game;

    public Game ManagerStateData => game;

    public event UnityAction HasDamage;

    private void Awake()
    {
        game = main.GetComponent<Game>();
    }

    public void OnHasDamage() 
    { 
        HasDamage?.Invoke();
    }

    public int Lives
    {
        set
        {
            if (IsLive())
            {
                liveAmount = value;
                OnHasDamage();
            }
        }
        get => liveAmount;
    }

    private bool IsLive()
    {
        return State != DEAD;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;

        if (IsBot(other))
        {
            SetDamage();
        }
    }

    private void SetDamage()
    {
        if (IsLive())
        {
            Lives -= 1;
        }
    }

    private bool IsBot(GameObject instance)
    {
        return instance.TryGetComponent<Bot>(out Bot b);
    }
}
