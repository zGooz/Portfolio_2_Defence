
using UnityEngine;


public class LifeBox : MonoBehaviour
{
    [SerializeField] private GameObject[] lives;
    [SerializeField] private GameObject playerObject;

    private int liveAmount;
    private Player player;
    
    private void Awake()
    {
        player = playerObject.GetComponent<Player>();
        liveAmount = player.Lives;
    }

    private void OnEnable() 
    {
        player.HasDamage += OnPanchToPlayer;
    }

    private void OnDisable() 
    {
        player.HasDamage -= OnPanchToPlayer; 
    }

    private void OnPanchToPlayer()
    {
        if (IsPlayerLive())
        {
            int lastIndex = liveAmount - 1;
            Destroy(lives[lastIndex]);
            liveAmount -= 1;
        }
    }

    private bool IsPlayerLive()
    {
        return liveAmount > 0;
    }
}
