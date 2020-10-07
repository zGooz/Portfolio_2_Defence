
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
    public float Radius { get; } = 40.0f;

    public int _liveAmount = 5;
    private Game game;

    public Game ManagerStateData => game;

    public event UnityAction Dead;
    public event UnityAction HasDamage;

    private void Awake()
    {
        game = main.GetComponent<Game>();
    }

    public void OnDead()
    {
        Dead?.Invoke();
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
                _liveAmount = value;

                if (IsCanNotLive())
                {
                    State = DEAD;
                    OnDead();
                }

                OnHasDamage();
            }
        }
        get => _liveAmount;
    }

    private bool IsLive()
    {
        return State != DEAD;
    }

    private bool IsCanNotLive()
    {
        return _liveAmount == 0;
    }
}
