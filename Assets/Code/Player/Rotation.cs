
using UnityEngine;


public class Rotation : MonoBehaviour
{
    [SerializeField] 
    private Camera cameraObject;
    private Player player;

    private void Awake() { player = GetComponent<Player>(); }

    private void Update()
    {
        bool stateIsInGame = player.ManagerStateData.GameState == Game.GAME;
        if (! stateIsInGame) { return; }
        bool isPlayerLive = player.State != Player.DEAD;
        if (isPlayerLive) { LookToMouse(); }
    }

    private void LookToMouse()
    {
        Vector3 screenMouse = Input.mousePosition;
        Vector3 worldMouse = cameraObject.ScreenToWorldPoint(screenMouse);
        this.transform.LookAt(new Vector3(0, 0, worldMouse.z), worldMouse);
    }
}
