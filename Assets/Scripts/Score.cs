using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Transform Player;
    private Text ScoreText;
    private float multiplier;

    // Start is called before the first frame update
    void Start()
    {
        ScoreText = GetComponent<Text>();
        this.multiplier = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // Show the score without any decimal
        this.ScoreText.text = (Player.position.z * multiplier).ToString("0");
    }

    public void increaseMultiplier()
    {
        this.multiplier++;
    }

    public void decreaseMultiplier()
    {
        this.multiplier *= 0.75f;
    }
}
