using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame(){
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
        PlayerHealth.Restart();
    }

    public void ExitGame(){
        Application.Quit();
    }
}
