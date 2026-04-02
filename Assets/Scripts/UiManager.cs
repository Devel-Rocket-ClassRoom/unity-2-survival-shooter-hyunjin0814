using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public Slider healthSlider;
    public TextMeshProUGUI scoreText;

    private int score;

    private void Awake()
    {
        score = 0;
    }

    public void SetHealthSlide(float amount)
    {
        healthSlider.value = amount;
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = $"SCORE: {score}";
    }
}