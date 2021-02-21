using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject focusOption;

    // Select the first option when the menu is enabled.
    // https://answers.unity.com/questions/1142958/buttonselect-doesnt-highlight.html?page=3&pageSize=5&sort=votes
    private void OnEnable()
    {
        focusOption.GetComponent<Selectable>().Select();
    }

    private void Start()
    {
        // Deselect the option when menu si disable;
        focusOption.GetComponent<Selectable>().OnSelect(null);
    }

    // Load the scene level
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    // Exit the game
    public void Quit()
    {
        Application.Quit();
    }
}