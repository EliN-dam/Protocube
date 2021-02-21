using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    // Load game menu
    public void Back()
    {
        //Application.Quit();
        SceneManager.LoadScene("Menu");
    }
}
