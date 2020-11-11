
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class Player : MonoBehaviour
{
    [SerializeField] 
    private GameObject main;
    private Game game;

    public const int LIVE = 0;
    public const int DEAD = 1;
    public int liveAmount;

    public int State { get; set; } = LIVE;
    public float Radius { get; } = 18.0f;
    public Game ManagerStateData => game;
    public event UnityAction HasDamage;

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

    private void Awake() 
    {
        game = main.GetComponent<Game>();
        liveAmount = GameObject.FindGameObjectsWithTag("lives").Length;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        bool isBot = other.TryGetComponent<Bot>(out Bot b);
        if (isBot) { SetDamage(); }
    }

    public void OnHasDamage() { HasDamage?.Invoke(); }
    private bool IsLive() { return State != DEAD; }
    private void SetDamage() { if (IsLive()) { Lives -= 1; } }
}
