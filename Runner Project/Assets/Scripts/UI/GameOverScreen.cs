using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public TMP_Text scoreText;

    public void ActivateGameOverScreen(int score){
        gameObject.SetActive(true);
        scoreText.text = score + " x " + ScoreManager.Instance.coinAmount;
    }

    public void RestartButton(){
        SceneManager.LoadScene("GameScene");
        PlayerHealth.gameOver = false;
        PlayerHealth.health = 3;
    }
}
