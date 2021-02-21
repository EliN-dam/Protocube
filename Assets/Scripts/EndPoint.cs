using UnityEngine;

public class EndPoint : MonoBehaviour
{
    private GameManager game;

    // Start is called before the first frame update
    void Start()
    {
        game = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            game.LevelComplete(); // Show level complete screen.
            this.enabled = false; // Prevent collision twice.
        }
    }
}
