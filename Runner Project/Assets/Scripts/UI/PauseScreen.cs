using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    private bool pauseState = false;

    public void TogglePauseScreen(){
        if(!pauseState){
            pauseState = true;
            Time.timeScale = 0;
            gameObject.SetActive(true);
        }
        else{
            pauseState = false;
            Time.timeScale = 1;
            gameObject.SetActive(false);
        }
    }

    public void ReturnToMenu(){
        SceneManager.LoadScene(0);
    }
}
