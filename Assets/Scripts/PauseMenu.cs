using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject focusOption;
    public GameObject pauseUI;
    public GameObject scoreUI;
    public static bool gamePause = false;

    // Select the first option when the menu is enabled.
    // https://answers.unity.com/questions/1159573/eventsystemsetselectedgameobject-doesnt-highlight.html
    private void OnEnable()
    {
        focusOption.GetComponent<Selectable>().Select();
    }

    private void Start()
    {
        // Deselect the option when menu si disable;
        focusOption.GetComponent<Selectable>().OnSelect(null);
    }

    // Press select button in the gamepad
    public void SelectOption()
    {
        if (gamePause) { Resume(); }
        else { Pause(); }
    }

    // Pause the game
    private void Pause()
    {
        scoreUI.SetActive(false); // Disable score text
        pauseUI.SetActive(true); // Activate the UI
        Time.timeScale = 0; // Pause the time
        gamePause = true;
    }

    // Back to the game
    public void Resume()
    {
        scoreUI.SetActive(true); // Enable back score label
        pauseUI.SetActive(false); // Disable de UI
        Time.timeScale = 1; // Bring back the time cycle
        gamePause = false;
        // make sure to deselect the option when menu si disable;
        focusOption.GetComponent<Selectable>().OnSelect(null);
    }

    // Go back to main menu
    public void Menu()
    {
        Resume();
        SceneManager.LoadScene("Menu");
    }
}