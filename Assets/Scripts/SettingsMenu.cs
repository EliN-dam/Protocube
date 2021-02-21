using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public GameObject focusOption;

    // Panel Objects
    private Slider difficultySelector;
    private Toggle wallOption;
    private Toggle colorOption;
    private Toggle deathOption;
    private Toggle endlessOption;

    // Game settings
    private int difficulty;
    private bool walls = true;
    private bool colorMode = true;
    private bool deathMode = true;
    private bool endlessMode = false;

    // Select the first option when the menu is enabled.
    private void OnEnable()
    {
        focusOption.GetComponent<Selectable>().Select();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Deselect the option when menu si disable;
        focusOption.GetComponent<Selectable>().OnSelect(null);

        loadConfig();

        // Initialize Components
        this.difficultySelector = GameObject.Find("Difficulty").GetComponent<Slider>();
        this.wallOption =  GameObject.Find("Walls").GetComponent<Toggle>();
        this.colorOption = GameObject.Find("Colors").GetComponent<Toggle>();
        this.deathOption = GameObject.Find("NoDeath").GetComponent<Toggle>();
        this.endlessOption = GameObject.Find("Endless").GetComponent<Toggle>();

        // Set the values
        difficultySelector.value = difficulty;
        wallOption.isOn = walls;
        this.colorOption.isOn = colorMode;
        this.deathOption.isOn = deathMode;
        this.endlessOption.isOn = endlessMode;
    }

    // Setting the difficulty
    public void SetDifficulty(float difficulty)
    {
        this.difficulty = (int)difficulty;
    }

    // Settings the walls
    public void SetWallsMode(bool mode)
    {
        this.walls = mode;
    }

    // Setting the color mode
    public void SetColorModer(bool mode)
    {
        this.colorMode = mode;
    }

    // Setting death Mode
    public void SetDeathMode(bool mode)
    {
        this.deathMode = mode;
    }

    //Setting endless mode
    public void SetEndlessMode(bool mode)
    {
        this.endlessMode = mode;
    }

    // Load the settings
    // https://www.red-gate.com/simple-talk/dotnet/c-programming/how-to-create-a-settings-menu-in-unity/
    private void loadConfig()
    {
        this.difficulty = PlayerPrefs.GetInt("difficulty", 12);
        this.walls = Convert.ToBoolean(PlayerPrefs.GetInt("walls", 1));
        this.colorMode = Convert.ToBoolean(PlayerPrefs.GetInt("colors", 1));
        this.deathMode = Convert.ToBoolean(PlayerPrefs.GetInt("death", 1));
        this.endlessMode = Convert.ToBoolean(PlayerPrefs.GetInt("endless", 0));
    }

    // Save the settings
    public void saveConfig()
    {
        PlayerPrefs.SetInt("difficulty", difficulty);
        PlayerPrefs.SetInt("walls", Convert.ToInt32(walls));
        PlayerPrefs.SetInt("colors", Convert.ToInt32(colorMode));
        PlayerPrefs.SetInt("death", Convert.ToInt32(deathMode));
        PlayerPrefs.SetInt("endless", Convert.ToInt32(endlessMode));
        // Deselect the option when menu si disable;
        focusOption.GetComponent<Selectable>().OnSelect(null);
    }
}