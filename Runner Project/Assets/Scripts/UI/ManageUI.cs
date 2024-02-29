using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManageUI : MonoBehaviour
{
    public TMP_Text healthText;
    public TMP_Text coinText;
    public TMP_Text scoreText;
    public TMP_Text scoreMultiplierText;
    public Slider multiplierIntervalAmount;

    void Update(){
        healthText.text = PlayerHealth.health.ToString();
        coinText.text = ScoreManager.Instance.coinAmount.ToString();
        scoreText.text = ScoreManager.Instance.currentScore.ToString();
        scoreMultiplierText.text = "x" + ScoreManager.Instance.scoreMultiplier;
        multiplierIntervalAmount.value = ScoreManager.Instance.increaseMultiplierInterval;
    }
}
