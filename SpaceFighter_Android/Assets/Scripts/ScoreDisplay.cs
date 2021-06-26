using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private Score score;

    private void Start()
    {
        scoreText = GetComponent<Text>();
        score = FindObjectOfType<Score>();
    }

    private void Update()
    {
        scoreText.text = score.GetScore().ToString();
    }
}
