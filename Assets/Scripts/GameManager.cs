using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public float restartDelay = 1f;
    public bool walls = true; // Activate/Deactivate side walls
    public bool colorMode = true; // Activate/Deactivate  Color mode
    public bool endlessMode = false; // Activate/Deactivate infinite level
    public bool noDeathMode = false; // Activate/Deactivate no restart mode
    public bool destroyMode = false; // Activate/Deactivate god mode
    public GameObject LevelCompleteUI;

    private bool gameHasEnded = false;

    void Start()
    {
        LoadConfig(); // Load The condig
        this.LevelCompleteUI.SetActive(false);
        /*if (!walls)
        {
            foreach (GameObject wall in GameObject.FindGameObjectsWithTag("Wall"))
            {
                wall.SetActive(false);
            }
        }*/
    }

    // public method when the game has ended
    public void GameOver()
    {
        if (!gameHasEnded)
        {
            gameHasEnded = true;
            // Invoke a method Restart after the seletec delayed
            Invoke("Restart", restartDelay); 
        }
    }

    // Reaload the Scene
    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Active Canavas when the level is complete
    public void LevelComplete()
    {
        this.noDeathMode = true;
        this.LevelCompleteUI.SetActive(true);
    }

    // Load game modes settings
    private void LoadConfig()
    {
        this.walls = Convert.ToBoolean(PlayerPrefs.GetInt("walls", 1));
        this.colorMode = Convert.ToBoolean(PlayerPrefs.GetInt("colors", 1));
        this.noDeathMode = !Convert.ToBoolean(PlayerPrefs.GetInt("death", 1));
        this.endlessMode = Convert.ToBoolean(PlayerPrefs.GetInt("endless", 0));
    }
}