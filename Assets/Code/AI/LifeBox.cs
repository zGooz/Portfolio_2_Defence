
using UnityEngine;
using UnityEngine.Events;


public class LifeBox : MonoBehaviour
{
    [SerializeField] private GameObject[] lives;
    [SerializeField] private GameObject playerObject;

    private int liveAmount;
    private Player player;
    public event UnityAction Dead;

    private void Awake()
    {
        player = playerObject.GetComponent<Player>();
        liveAmount = player.Lives;
    }

    private void OnEnable() { player.HasDamage += OnPanchToPlayer; }
    private void OnDisable() { player.HasDamage -= OnPanchToPlayer; }

    private void OnPanchToPlayer()
    {
        TakeOneLive();
        bool isDead = liveAmount == 0;

        if (isDead)
        {
            player.State = Player.DEAD;
            OnDead();
        }
    }

    private void TakeOneLive()
    {
        int lastIndex = liveAmount - 1;
        Destroy(lives[lastIndex]);
        liveAmount -= 1;
    }

    public void OnDead() { Dead?.Invoke(); }
}
