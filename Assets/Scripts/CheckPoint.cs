using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private GameManager game;
    private MapGenerator map;

    // Start is called before the first frame update
    void Start()
    {
        game = FindObjectOfType<GameManager>();
        map = FindObjectOfType<MapGenerator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Create new chunks when player reach to checkPoint.
            if (game.endlessMode)
            {
                map.NewChunck();
            }
            this.enabled = false; // Prevent collision twice.
        }
    }
}
