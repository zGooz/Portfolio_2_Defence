﻿
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
        int lastIndex = liveAmount - 1;
        Destroy(lives[lastIndex]);
        liveAmount -= 1;

        if (IsLiveNotExists())
        {
            player.State = Player.DEAD;
            OnDead();
        }
    }

    public void OnDead()
    {
        Dead?.Invoke();
    }

    private bool IsLiveNotExists()
    {
        return liveAmount == 0;
    }
}
