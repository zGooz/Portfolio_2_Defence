
using UnityEngine;


public class Rotation : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private Player _playerComponent;

    private void Awake()
    {
        _playerComponent = GetComponent<Player>();
    }

    private void Update()
    {
        if (_playerComponent.ManagerStateData.GameState == GameProcess.GAME)
        {
            if (_playerComponent.State != Player.DEAD)
            {
                Vector3 screenMouse = Input.mousePosition;
                Vector3 worldMouse = _camera.ScreenToWorldPoint(screenMouse);

                transform.LookAt(new Vector3(0, 0, 0), worldMouse);
            }
        }
    }
}
