
using UnityEngine;


public class Rotation : MonoBehaviour
{
    [SerializeField] private Camera cameraObject;

    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        if (! StateIsInGame())
        {
            return;
        }

        if (IsPlayerNotDead())
        {
            LookToMouse();
        }
    }

    private bool StateIsInGame()
    {
        return player.ManagerStateData.GameState == Game.GAME;
    }

    private bool IsPlayerNotDead()
    {
        return player.State != Player.DEAD;
    }

    private void LookToMouse()
    {
        Vector3 screenMouse = Input.mousePosition;
        Vector3 worldMouse = cameraObject.ScreenToWorldPoint(screenMouse);

        this.transform.LookAt(new Vector3(0, 0, worldMouse.z), worldMouse);
    }
}
