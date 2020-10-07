
using UnityEngine;


public class LifeBox : MonoBehaviour
{
    [SerializeField] private GameObject[] _sprites;
    [SerializeField] private GameObject _player;

    private int _length;
    private Player _playerComponent;
    
    private void Awake()
    {
        _playerComponent = _player.GetComponent<Player>();
        _length = _playerComponent.Lives;
    }

    private void OnEnable() 
    {
        _playerComponent.HasDamage += OnPanchToPlayer;
    }

    private void OnDisable() 
    {
        _playerComponent.HasDamage -= OnPanchToPlayer; 
    }

    private void OnPanchToPlayer()
    {
        if (_length > 0)
        {
            int last = _length - 1;
            Destroy(_sprites[last]);
            _length--;
        }
    }
}
