using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Player;
    public Vector3 offset; // Camera position relatiove to player.

    // Update is called once per frame
    void Update()
    {
        // Cameras follows the player
        transform.position = Player.position + offset;
    }
}
