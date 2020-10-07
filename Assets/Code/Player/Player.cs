
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject _gameManager;

    public const int LIVE = 0;
    public const int DEAD = 1;
    public int State { get; set; } = LIVE;
    public float Radius { get; } = 40.0f;

    public int _liveAmount = 5;
    private GameProcess _managerGameProcess;

    public GameProcess ManagerStateData => _managerGameProcess;

    public event UnityAction Dead;
    public event UnityAction HasDamage;

    private void Awake()
    {
        _managerGameProcess = _gameManager.GetComponent<GameProcess>();
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
        get => _liveAmount;
        set
        {
            if (State != DEAD)
            {
                _liveAmount = value;

                if (_liveAmount == 0)
                {
                    State = DEAD;
                    OnDead();
                }

                OnHasDamage();
            }
        }
    }
}
