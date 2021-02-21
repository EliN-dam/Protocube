using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreCount : MonoBehaviour
{
    public GameObject Score;
    public GameObject focusOption;
    public int speed = 100;

    private GameManager game;
    private TextMeshProUGUI ScoreText;
    private bool showResult = false;
    private int value;
    private int i;

    // Start is called before the first frame update
    void Start()
    {
        ScoreText = GameObject.Find("FinalScore").GetComponent<TextMeshProUGUI>();
        game = FindObjectOfType<GameManager>();
    }

    private void OnEnable()
    {
        focusOption.GetComponent<Selectable>().Select();
    }

    // https://answers.unity.com/questions/44074/score-count-up-to-target-score-works-tooo-fast.html
    private void Update()
    {
        if (showResult)  // If event is triggered
        {
            // Shout count result on screen determiend by speed
            for (int j = 0; j < speed; j++) {
                if (i <= value)
                {
                    i++;
                    ScoreText.text = i.ToString();
                }
            }
        }
    }

    // Enables CountResult
    public void CountResult()
    {
        value = Convert.ToInt32(Score.GetComponent<Text>().text);
        i = 0;
        showResult = true;
    }

    // Back to main menu
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
}