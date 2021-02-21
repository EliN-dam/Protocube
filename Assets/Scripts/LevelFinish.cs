using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinish : MonoBehaviour
{
    private GameManager game;

    // Start is called before the first frame update
    void Start()
    {
        game = FindObjectOfType<GameManager>();
    }

    // Show congratulations screen
    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("Player")) 
        {
            game.LevelComplete();
        }
    }
}